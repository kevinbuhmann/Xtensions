namespace Xtensions.Build
{
    using Nuke.Common;
    using Xtensions.Build.NukeExtensions;
    using Xtensions.Build.Targets;

    public class BuildTargets : NukeBuild
    {
        public BuildTargets()
        {
            Instance = this;
            ParameterInjector.InjectParameters(this);
        }

        public static BuildTargets Instance { get; private set; }

        public static BuildPaths Paths { get; } = new BuildPaths(RootDirectory);

        [Parameter(@"Specifies build configuration: Fast|Verify|Prod")]
        public BuildConfiguration Configuration { get; } = BuildConfiguration.Release;

        public Target Clean { get; } = new CleanTarget().Define;

        public Target Build { get; } = new BuildTarget().Define;

        public Target Test { get; } = new TestTarget().Define;

        public Target CoverageReport { get; } = new CoverageReportTarget().Define;
    }
}
