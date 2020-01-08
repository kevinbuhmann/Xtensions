namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using global::Fody;
    using Mono.Cecil;

    internal class EnumReferences
    {
        public EnumReferences(BaseModuleWeaver weaver)
        {
            this.EnumType = new Lazy<TypeReference>(() => GetEnumType(weaver));
        }

        public Lazy<TypeReference> EnumType { get; }

        private static TypeReference GetEnumType(BaseModuleWeaver weaver)
        {
            return weaver.ModuleDefinition.ImportReference(weaver.FindType(typeof(Enum).FullName));
        }
    }
}
