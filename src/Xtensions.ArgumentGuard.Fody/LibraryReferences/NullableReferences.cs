namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using global::Fody;
    using Mono.Cecil;

    internal class NullableReferences
    {
        public NullableReferences(BaseModuleWeaver weaver)
        {
            this.NullableType = new Lazy<TypeReference>(() => GetNullableType(weaver));
        }

        public Lazy<TypeReference> NullableType { get; }

        private static TypeReference GetNullableType(BaseModuleWeaver weaver)
        {
            return weaver.ModuleDefinition.ImportReference(weaver.FindType(typeof(Nullable<>).FullName));
        }
    }
}
