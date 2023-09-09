using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    [GitRepository]
    readonly GitRepository Repository;

    [CI] readonly AzurePipelines AzurePipelines;

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Print => _ => _
        .Before(Clean)
        .Executes(() =>
        {
            Log.Information("Commit             = {Value}", Repository.Commit);
            Log.Information("Branch             = {Value}", Repository.Branch);
            Log.Information("Tags               = {Value}", Repository.Tags);
            Log.Information("main branch        = {Value}", Repository.IsOnMainBranch());
            Log.Information("main/master branch = {Value}", Repository.IsOnMainOrMasterBranch());
            Log.Information("Https URL          = {Value}", Repository.HttpsUrl);
            Log.Information("SSH URL            = {Value}", Repository.SshUrl);

            Log.Information("Solution           = {Value}", Solution);

            Log.Information("BuildId            = {Value}", AzurePipelines?.BuildId);
            Log.Information("BuildNumber        = {Value}", AzurePipelines?.BuildNumber);
            Log.Information("AgentWorkFolder    = {Value}", AzurePipelines?.AgentWorkFolder);
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            RootDirectory
                .GlobDirectories(
                    "*/src/*/obj",
                    "*/src/*/bin",
                    "*/test/*/obj",
                    "*/test/*/bin")
                .ForEach(d => d.DeleteDirectory());
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetConfiguration(Configuration));
        });

    AbsolutePath TestResultDirectory => RootDirectory / "output" / ".test-results";

    Target Test => _ => _
        .DependsOn(Compile)
        .Partition(4)
        .Executes(() =>
        {
            var allTestConfigurations =
                from project in Solution.GetAllProjects("*.Test")
                from targetFramework in project.GetTargetFrameworks()
                select (project, targetFramework);

            var relevantTestConfigurations = Partition.GetCurrent(allTestConfigurations);

            try
            {
                DotNetTest(_ => _
                        .SetConfiguration(Configuration)
                        .SetNoBuild(InvokedTargets.Contains(Compile))
                        .SetResultsDirectory(TestResultDirectory)
                        .CombineWith(relevantTestConfigurations, (_, v) => _
                            .SetProjectFile(v.project)
                            .SetFramework(v.targetFramework)
                            .SetLoggers($"trx;LogFileName={v.project.Name}.trx")
                        ),
                    completeOnFailure: true);
            }
            finally
            {
                TestResultDirectory.GlobFiles("*.trx").ForEach(x =>
                    AzurePipelines?.PublishTestResults(
                        title: $"{Path.GetFileNameWithoutExtension(x)} ({AzurePipelines.StageDisplayName})",
                        type: AzurePipelinesTestResultsType.VSTest,
                        files: new string[] { x }));
            }
        });
}