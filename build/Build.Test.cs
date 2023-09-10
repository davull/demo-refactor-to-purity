using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

public partial class Build
{
    AbsolutePath TestResultDirectory => OutputDirectory / ".test-results";
    AbsolutePath CoverageReportDirectory => OutputDirectory / ".coverage";
    IEnumerable<Project> TestProjects => Solution.GetAllProjects("*.Test");

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testConfigurations =
                from project in TestProjects
                from targetFramework in project.GetTargetFrameworks()
                select (project, targetFramework);

            try
            {
                DotNetTest(_ => _
                        .SetConfiguration(Configuration)
                        .SetNoBuild(InvokedTargets.Contains(Compile))
                        .SetSettingsFile(RootDirectory / "test.runsettings")
                        .SetCollectCoverage(IsServerBuild)
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
                            title: $"{Path.GetFileNameWithoutExtension(x)} ({AzurePipelines.StageDisplayName})",
                            type: AzurePipelinesTestResultsType.VSTest,
                            files: new string[] { x }));
            }
        });

    Target CoverageReport => _ => _
        .DependsOn(Test)
        .AssuredAfterFailure()
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            var reportFiles = TestResultDirectory
                .GlobFiles("*/coverage.cobertura.xml")
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
}