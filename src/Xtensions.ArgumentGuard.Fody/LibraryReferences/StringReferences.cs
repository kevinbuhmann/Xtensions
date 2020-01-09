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
            this.ConcatThreeStringsMethod = new Lazy<MethodReference>(() => GetConcatThreeStringsMethod(weaver));
        }

        public Lazy<MethodReference> ConcatThreeStringsMethod { get; }

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
