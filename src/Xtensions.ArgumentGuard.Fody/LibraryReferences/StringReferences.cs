namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;

    internal class StringReferences
    {
        public StringReferences(BaseModuleWeaver weaver)
        {
            this.IndexerGetMethod = new Lazy<MethodReference>(() => GetIndexerGetMethod(weaver));
            this.FormatTwoArgsMethod = new Lazy<MethodReference>(() => GetFormatTwoArgsMethod(weaver));
            this.ConcatThreeStringsMethod = new Lazy<MethodReference>(() => GetConcatThreeStringsMethod(weaver));
        }

        public Lazy<MethodReference> IndexerGetMethod { get; }

        public Lazy<MethodReference> FormatTwoArgsMethod { get; }

        public Lazy<MethodReference> ConcatThreeStringsMethod { get; }

        private static MethodReference GetIndexerGetMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition stringType = weaver.FindType(typeof(string).FullName);
            PropertyDefinition indexer = stringType.Properties.Single(property =>
                property.Parameters.Count == 1
                && property.Parameters[0].ParameterType.FullName == typeof(int).FullName);

            return weaver.ModuleDefinition.ImportReference(indexer.GetMethod);
        }

        private static MethodReference GetFormatTwoArgsMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition stringType = weaver.FindType(typeof(string).FullName);
            MethodDefinition formatTwoArgsMethod = stringType.Methods.Single(method =>
                method.IsStatic
                && method.Name == nameof(string.Format)
                && method.Parameters.Count == 3
                && method.Parameters[0].ParameterType.FullName == typeof(string).FullName
                && method.Parameters[1].ParameterType.FullName == typeof(object).FullName
                && method.Parameters[2].ParameterType.FullName == typeof(object).FullName);

            return weaver.ModuleDefinition.ImportReference(formatTwoArgsMethod);
        }

        private static MethodReference GetConcatThreeStringsMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition stringType = weaver.FindType(typeof(string).FullName);
            MethodDefinition concatThreeStringsMethod = stringType.Methods.Single(method =>
                method.IsStatic
                && method.Name == nameof(string.Concat)
                && method.Parameters.Count == 3
                && method.Parameters[0].ParameterType.FullName == typeof(string).FullName
                && method.Parameters[1].ParameterType.FullName == typeof(string).FullName
                && method.Parameters[2].ParameterType.FullName == typeof(string).FullName);

            return weaver.ModuleDefinition.ImportReference(concatThreeStringsMethod);
        }
    }
}
