using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

public partial class Build
{
    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetLockedMode(IsServerBuild));
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
}