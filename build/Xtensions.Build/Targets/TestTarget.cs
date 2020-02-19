namespace Xtensions.Build.Targets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using EnsureThat;
    using Nuke.Common;
    using Nuke.Common.IO;
    using Nuke.Common.Tools.DotNet;
    using Xtensions.Build.NukeExtensions;

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
                    DotNetTasks.DotNet(arguments: "tool restore");

                    IEnumerable<string> testProjectFiles = PathConstruction.GlobFiles(
                        directory: BuildTargets.Paths.TestsDirectory,
                        patterns: $@"**/*.Tests.csproj");

                    try
                    {
                        foreach (string testProjectFile in testProjectFiles)
                        {
                            FileInfo testProjectFileInfo = new FileInfo(testProjectFile);
                            string assemblyFileName = testProjectFileInfo.Name.Replace(".csproj", ".dll", StringComparison.InvariantCulture);
                            string testAssemblyFilePattern = $@"bin/{Program.Targets.Configuration}/**/{assemblyFileName}";
                            string testAssemblyFile = PathConstruction.GlobFiles(testProjectFileInfo.Directory.FullName, testAssemblyFilePattern).Single();
                            string project = testProjectFileInfo.Name.Replace(".Tests.csproj", string.Empty, StringComparison.InvariantCulture);

                            IReadOnlyCollection<string> coverletArguments = new[]
                            {
                                testAssemblyFile,
                                "--target dotnet",
                                $"--targetargs \"test -c {Program.Targets.Configuration} {testProjectFile} --no-build\"",
                                $"--include [{project}]*",
                                $"--threshold {this.CodeCoverageThreshold}",
                                $"--output {PathConstruction.Combine(CoverageDirectory, $"{project}.xml")}",
                                "--format opencover",
                            };

                            DotNetTasks.DotNet(arguments: $"tool run coverlet {string.Join(separator: " ", coverletArguments)}");
                        }
                    }
                    finally
                    {
                        IReadOnlyCollection<string> reportGeneratorArguments = new[]
                        {
                            $"--reports:{CoverageDirectory}/*.xml",
                            $"--targetdir:{CoverageReportDirectory}",
                            "--reporttypes:Html",
                        };

                        DotNetTasks.DotNet(arguments: $"tool run reportgenerator {string.Join(separator: " ", reportGeneratorArguments)}");
                    }
                });
        }
    }
}
