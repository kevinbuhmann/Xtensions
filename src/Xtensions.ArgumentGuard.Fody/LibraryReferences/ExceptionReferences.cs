namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;

    internal class ExceptionReferences
    {
        public ExceptionReferences(BaseModuleWeaver weaver)
        {
            this.ArgumentNullExceptionWithMessageConstructor = new Lazy<MethodReference>(() => GetArgumentNullExceptionWithMessageConstructor(weaver));
        }

        public Lazy<MethodReference> ArgumentNullExceptionWithMessageConstructor { get; }

        private static MethodReference GetArgumentNullExceptionWithMessageConstructor(BaseModuleWeaver weaver)
        {
            TypeDefinition argumentNullExceptionType = weaver.FindType(typeof(ArgumentNullException).FullName);
            MethodDefinition argumentNullExceptionWithMessageConstructor = argumentNullExceptionType.Methods.Single(method =>
                method.IsConstructor
                && method.Parameters.Count == 2
                && method.Parameters[0].ParameterType.FullName == typeof(string).FullName
                && method.Parameters[1].ParameterType.FullName == typeof(string).FullName);

            return weaver.ModuleDefinition.ImportReference(argumentNullExceptionWithMessageConstructor);
        }
    }
}
