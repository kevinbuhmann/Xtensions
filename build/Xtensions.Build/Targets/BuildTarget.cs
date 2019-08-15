namespace Xtensions.Build.Targets
{
    using EnsureThat;
    using Nuke.Common;
    using Nuke.Common.Tooling;
    using Nuke.Common.Tools.DotNet;
    using Xtensions.Build.NukeExtensions;

    public class BuildTarget : BaseTarget
    {
        public override ITargetDefinition Define(ITargetDefinition target)
        {
            EnsureArg.IsNotNull(target, nameof(target));

            return target
                .Description("Builds all code")
                .After(Program.Targets.Clean)
                .Executes(() =>
                {
                    DotNetTasks.DotNetBuild(s => s
                        .SetWorkingDirectory(RootDirectory)
                        .SetProjectFile("Xtensions.sln")
                        .SetVerbosity(DotNetVerbosity.Minimal)
                        .SetConfiguration(Program.Targets.Configuration.ToString()));
                });
        }
    }
}
