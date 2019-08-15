namespace Xtensions.Build.Targets
{
    using EnsureThat;
    using Nuke.Common;
    using Nuke.Common.Tooling;
    using Xtensions.Build.NukeExtensions;
    using static Nuke.Common.IO.PathConstruction;

    public class CoverageReportTarget : BaseTarget
    {
        public override ITargetDefinition Define(ITargetDefinition target)
        {
            EnsureArg.IsNotNull(target, nameof(target));

            return target
                .Description("Shows code coverage report")
                .DependsOn(Program.Targets.Test)
                .Executes(() => ProcessTasks.StartProcess(
                    toolPath: "cmd",
                    arguments: $@"/c {Combine(TestTarget.CoverageReportDirectory, "index.htm")}"));
        }
    }
}
