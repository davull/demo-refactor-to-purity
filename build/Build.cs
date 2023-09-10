using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;

public partial class Build : NukeBuild
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


    protected override void OnBuildInitialized()
    {
        Log.Information("\ud83d\ude80 Build process started");

        base.OnBuildInitialized();
    }

    public static int Main() => Execute<Build>(x => x.Compile);
}