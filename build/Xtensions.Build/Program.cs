namespace Xtensions.Build
{
    using Nuke.Common;

    public class Program : NukeBuild
    {
        public static BuildTargets Targets => BuildTargets.Instance;

        public static int Main()
        {
            return Execute<BuildTargets>();
        }
    }
}
