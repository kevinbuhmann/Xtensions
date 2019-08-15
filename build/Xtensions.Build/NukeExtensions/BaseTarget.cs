namespace Xtensions.Build.NukeExtensions
{
    using Nuke.Common;
    using Nuke.Common.Tooling;

    public abstract class BaseTarget : NukeBuild
    {
        protected BaseTarget()
        {
            ParameterInjector.InjectParameters(this);
            ToolPathResolver.NuGetPackagesConfigFile = this.NuGetPackagesConfigFile;
        }

        public abstract ITargetDefinition Define(ITargetDefinition target);
    }
}
