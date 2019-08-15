namespace Xtensions.Build
{
    using System;
    using static Nuke.Common.IO.PathConstruction;

    public class BuildPaths
    {
        public const string ContextName = "common";
        private static readonly string ExternalConfigRoot = Environment.GetEnvironmentVariable("CABINET");

        private readonly AbsolutePath rootDirectory;

        public BuildPaths(AbsolutePath rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public static string ExternalConfigDirectory => Combine(ExternalConfigRoot, ContextName);

        public string SrcDirectory => Combine(this.rootDirectory, "src");

        public string TestsDirectory => Combine(this.rootDirectory, "tests");

        public string ArtifactsDirectory => Combine(this.rootDirectory, "artifacts");

        public string ConfigDirectory => Combine(this.rootDirectory, "config");
    }
}
