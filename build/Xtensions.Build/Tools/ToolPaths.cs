namespace Xtensions.Build.Tools
{
    using System.Collections.Generic;
    using Nuke.Common.Tooling;

    internal class ToolPaths
    {
        private static ToolPaths _instance;

        private readonly IDictionary<string, string> toolPaths = new Dictionary<string, string>();

        private ToolPaths()
        {
        }

        public static ToolPaths Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ToolPaths();
                }

                return _instance;
            }
        }

        public string ReportGeneratorPath => this.GetToolPath(key: nameof(this.ReportGeneratorPath), packageId: "ReportGenerator", packageExecutable: "ReportGenerator.exe", framework: "netcoreapp3.0");

        private string GetToolPath(string key, string packageId, string packageExecutable, string framework = null)
        {
            if (this.toolPaths.ContainsKey(key) == false)
            {
                this.toolPaths[key] = ToolPathResolver.GetPackageExecutable(packageId: packageId, packageExecutable: packageExecutable, framework: framework);
            }

            return this.toolPaths[key];
        }
    }
}
