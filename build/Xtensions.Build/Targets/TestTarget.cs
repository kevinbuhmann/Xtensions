namespace Xtensions.Build.Targets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using EnsureThat;
    using Nuke.Common;
    using Nuke.Common.IO;
    using Nuke.Common.Tooling;
    using Nuke.Common.Tools.Coverlet;
    using Nuke.Common.Tools.ReportGenerator;
    using Xtensions.Build.NukeExtensions;
    using Xtensions.Build.Tools;

    public class TestTarget : BaseTarget
    {
        public static string CoverageReportDirectory => PathConstruction.Combine(CoverageDirectory, "report");

        [Parameter("The code coverage threshold that will fail the build.")]
        public int? CodeCoverageThreshold { get; set; } = 100;

        private static string CoverageDirectory => PathConstruction.Combine(BuildTargets.Paths.ArtifactsDirectory, "coverage");

        public override ITargetDefinition Define(ITargetDefinition target)
        {
            EnsureArg.IsNotNull(target, nameof(target));

            return target
                .Description("Executes all tests")
                .DependsOn(Program.Targets.Build)
                .Executes(() =>
                {
                    FileSystemTasks.EnsureCleanDirectory(CoverageDirectory);

                    IEnumerable<string> testProjectFiles = PathConstruction.GlobFiles(
                        directory: BuildTargets.Paths.TestsDirectory,
                        patterns: $@"**/*.Tests.csproj");

                    foreach (string testProjectFile in testProjectFiles)
                    {
                        FileInfo testProjectFileInfo = new FileInfo(testProjectFile);
                        string assemblyFileName = testProjectFileInfo.Name.Replace(".csproj", ".dll", StringComparison.InvariantCulture);
                        string testAssemblyFilePattern = $@"bin/{Program.Targets.Configuration}/**/{assemblyFileName}";
                        string testAssemblyFile = PathConstruction.GlobFiles(testProjectFileInfo.Directory.FullName, testAssemblyFilePattern).Single();
                        string project = testProjectFileInfo.Name.Replace(".Tests.csproj", string.Empty, StringComparison.InvariantCulture);

                        CoverletTasks.Coverlet(settings => settings
                            .SetAssembly(testAssemblyFile)
                            .SetTarget("dotnet")
                            .SetTargetArgs($"test -c {Program.Targets.Configuration} {testProjectFile} --no-build")
                            .SetInclude($"[{project}]*")
                            .SetOutput(PathConstruction.Combine(CoverageDirectory, $"{project}.xml"))
                            .SetFormat(CoverletOutputFormat.opencover)
                            .SetThreshold(this.CodeCoverageThreshold));
                    }

                    ReportGeneratorTasks.ReportGenerator(settings => settings
                        .SetToolPath(ToolPaths.Instance.ReportGeneratorPath)
                        .SetReports($@"{CoverageDirectory}/*.xml")
                        .SetReportTypes(ReportTypes.Html)
                        .SetTargetDirectory(CoverageReportDirectory));
                });
        }
    }
}
