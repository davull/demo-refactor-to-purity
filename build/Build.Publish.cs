using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

public partial class Build
{
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
                var source = PublishOutputDirectory / project.Name;
                var archive = OutputDirectory / $"{project.Name}.zip";

                Log.Information("Creating archive {Archive}", archive);

                source.ZipTo(archive);
            });
        });

    Target PublishArtifacts => _ => _
        .DependsOn(Zip)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            ProjectsToPublish.ForEach(project =>
            {
                var archive = OutputDirectory / $"{project.Name}.zip";

                Log.Information("Publish artifact {Archive}", archive);

                AzurePipelines?.UploadArtifacts("publish", archive.NameWithoutExtension, archive);
            });
        });
}