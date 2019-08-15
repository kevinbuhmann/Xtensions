namespace Xtensions.Build.Targets
{
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using Nuke.Common;
    using Xtensions.Build.NukeExtensions;
    using static Nuke.Common.IO.FileSystemTasks;
    using static Nuke.Common.IO.PathConstruction;

    public class CleanTarget : BaseTarget
    {
        public override ITargetDefinition Define(ITargetDefinition target)
        {
            EnsureArg.IsNotNull(target, nameof(target));

            return target
                .Description("Deletes code artifacts")
                .Executes(() =>
                {
                    DeleteDirectory(BuildTargets.Paths.ArtifactsDirectory);

                    foreach (string directory in GetDotNetArtifactDirectories())
                    {
                        DeleteDirectory(directory);
                    }
                });
        }

        private static IEnumerable<string> GetDotNetArtifactDirectories()
        {
            return new string[] { BuildTargets.Paths.SrcDirectory, BuildTargets.Paths.TestsDirectory }
                .SelectMany(directory => GlobDirectories(directory, "**/bin", "**/obj"));
        }
    }
}
