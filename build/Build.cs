using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;


class Build : NukeBuild
{
    [CI] readonly AzurePipelines AzurePipelines;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion] readonly GitVersion GitVersioning;

    [GitRepository] readonly GitRepository Repository;

    [Solution] readonly Solution Solution;

    Project[] ProjectsToPublish => Solution
        .AllProjects
        .Where(p => p.Name != "_build" && !p.Name.EndsWith(".Test"))
        .ToArray();

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Print => _ => _
        .Before(Clean)
        .Executes(() =>
        {
            Log.Information("Commit                = {Value}", Repository.Commit);
            Log.Information("Branch                = {Value}", Repository.Branch);
            Log.Information("Tags                  = {Value}", Repository.Tags);
            Log.Information("main branch           = {Value}", Repository.IsOnMainBranch());
            Log.Information("main/master branch    = {Value}", Repository.IsOnMainOrMasterBranch());
            Log.Information("Https URL             = {Value}", Repository.HttpsUrl);
            Log.Information("SSH URL               = {Value}", Repository.SshUrl);

            Log.Information("Solution              = {Value}", Solution);
            Log.Information("Configuration         = {Value}", Configuration);
            Log.Information("SemVer                = {Value}", GitVersioning.SemVer);
            Log.Information("AssemblySemVer        = {Value}", GitVersioning.AssemblySemVer);
            Log.Information("AssemblySemFileVer    = {Value}", GitVersioning.AssemblySemFileVer);
            Log.Information("InformationalVersion  = {Value}", GitVersioning.InformationalVersion);
            Log.Information("MajorMinorPatch       = {Value}", GitVersioning.MajorMinorPatch);

            Log.Information("BuildId               = {Value}", AzurePipelines?.BuildId);
            Log.Information("BuildNumber           = {Value}", AzurePipelines?.BuildNumber);
            Log.Information("AgentWorkFolder       = {Value}", AzurePipelines?.AgentWorkFolder);
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            OutputDirectory.DeleteDirectory();

            RootDirectory
                .GlobDirectories("*/src/*/obj", "*/src/*/bin")
                .ForEach(d => d.DeleteDirectory());
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .EnableLockedMode());
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersioning.AssemblySemVer)
                .SetFileVersion(GitVersioning.AssemblySemFileVer)
                .SetInformationalVersion(GitVersioning.InformationalVersion));
        });

    AbsolutePath TestResultDirectory => OutputDirectory / ".test-results";

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testConfigurations =
                from project in Solution.GetAllProjects("*.Test")
                from targetFramework in project.GetTargetFrameworks()
                select (project, targetFramework);

            try
            {
                DotNetTest(_ => _
                        .SetConfiguration(Configuration)
                        .SetNoBuild(InvokedTargets.Contains(Compile))
                        .SetSettingsFile(RootDirectory / "test.runsettings")
                        .EnableCollectCoverage()
                        .SetDataCollector("XPlat Code Coverage")
                        .CombineWith(testConfigurations, (_, v) => _
                            .SetProjectFile(v.project)
                            .SetFramework(v.targetFramework)
                            .SetLoggers($"trx;LogFileName={v.project.Name}.trx")
                        ),
                    completeOnFailure: true);
            }
            finally
            {
                TestResultDirectory
                    .GlobFiles("*.trx")
                    .ForEach(x =>
                        AzurePipelines?.PublishTestResults(
                            $"{Path.GetFileNameWithoutExtension(x)} ({AzurePipelines.StageDisplayName})",
                            AzurePipelinesTestResultsType.VSTest,
                            new string[] { x }));
            }
        });

    AbsolutePath CoverageReportDirectory => OutputDirectory / ".coverage";

    Target CoverageReport => _ => _
        .DependsOn(Test)
        .AssuredAfterFailure()
        .Executes(() =>
        {
            var reportFiles = TestResultDirectory.GlobFiles("*/coverage.cobertura.xml")
                .Select(p => (string)p);

            ReportGenerator(_ => _
                .SetTargetDirectory(CoverageReportDirectory)
                .AddReports(reportFiles)
                .SetReportTypes(ReportTypes.Cobertura, ReportTypes.HtmlInline_AzurePipelines));

            AzurePipelines?.PublishCodeCoverage(
                AzurePipelinesCodeCoverageToolType.Cobertura,
                summaryFile: CoverageReportDirectory / "Cobertura.xml",
                reportDirectory: CoverageReportDirectory);
        });

    AbsolutePath PublishOutputDirectory => OutputDirectory / "publish";

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetPublish(_ => _
                .SetConfiguration(Configuration)
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .CombineWith(ProjectsToPublish, (_, project) => _
                    .SetProject(project)
                    .SetOutput(PublishOutputDirectory / project.Name)));
        });

    Target Zip => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            ProjectsToPublish.ForEach(project =>
            {
                var publishDirectory = PublishOutputDirectory / project.Name;
                var archiveFile = OutputDirectory / $"{project.Name}.zip";
                Log.Information("Creating archive {ArchiveFile}", archiveFile);

                publishDirectory.ZipTo(archiveFile);
            });
        });

    Target PublishArtifacts => _ => _
        .DependsOn(Zip)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            ProjectsToPublish.ForEach(project =>
            {
                var archiveFile = OutputDirectory / $"{project.Name}.zip";
                Log.Information("Publish artifact {ArchiveFile}", archiveFile);

                AzurePipelines?.UploadArtifacts("publish", archiveFile.NameWithoutExtension, archiveFile);
            });
        });

    protected override void OnBuildInitialized()
    {
        Log.Information("\ud83d\ude80 Build process started");

        base.OnBuildInitialized();
    }

    protected override void OnBuildFinished()
    {
        Log.Information("\ud83d\ude80 Build process finished");

        base.OnBuildFinished();
    }

    public static int Main() => Execute<Build>(x => x.Compile);
}