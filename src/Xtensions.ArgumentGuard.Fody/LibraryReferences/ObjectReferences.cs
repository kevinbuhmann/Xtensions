namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;

    internal class ObjectReferences
    {
        public ObjectReferences(BaseModuleWeaver weaver)
        {
            this.ToStringMethod = new Lazy<MethodReference>(() => GetToStringMethod(weaver));
        }

        public Lazy<MethodReference> ToStringMethod { get; }

        private static MethodReference GetToStringMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition objectType = weaver.FindType(typeof(object).FullName);
            MethodDefinition objectToStringMethod = objectType.Methods.Single(method =>
                method.IsStatic == false
                && method.Name == nameof(object.ToString)
                && method.Parameters.Count == 0);

            return weaver.ModuleDefinition.ImportReference(objectToStringMethod);
        }
    }
}
