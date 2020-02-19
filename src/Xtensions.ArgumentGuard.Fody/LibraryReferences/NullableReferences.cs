namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;

    internal class NullableReferences
    {
        public NullableReferences(BaseModuleWeaver weaver)
        {
            this.NullableType = new Lazy<TypeReference>(() => GetNullableType(weaver));
            this.HasValueGetMethod = new Lazy<MethodReference>(() => GetHasValueGetMethod(weaver));
        }

        public Lazy<TypeReference> NullableType { get; }

        public Lazy<MethodReference> HasValueGetMethod { get; }

        private static TypeReference GetNullableType(BaseModuleWeaver weaver)
        {
            return weaver.ModuleDefinition.ImportReference(weaver.FindType(typeof(Nullable<>).FullName));
        }

        private static MethodReference GetHasValueGetMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition nullableType = weaver.FindType(typeof(Nullable<>).FullName);
            PropertyDefinition hasValueProperty = nullableType.Properties.Single(property => property.Name == nameof(Nullable<int>.HasValue));

            return weaver.ModuleDefinition.ImportReference(hasValueProperty.GetMethod);
        }
    }
}
