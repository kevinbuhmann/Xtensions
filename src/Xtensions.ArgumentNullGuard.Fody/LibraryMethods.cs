namespace Xtensions.ArgumentNullGuard.Fody
{
    using System;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;

    internal class LibraryMethods
    {
        private readonly BaseModuleWeaver weaver;

        private MethodReference? argumentNullExceptionWithMessageConstructor;
        private MethodReference? concatThreeStringsMethod;

        public LibraryMethods(BaseModuleWeaver weaver)
        {
            this.weaver = weaver;
        }

        public MethodReference GetArgumentNullExceptionWithMessageConstructor()
        {
            if (this.argumentNullExceptionWithMessageConstructor == null)
            {
                TypeDefinition argumentNullExceptionType = this.weaver.FindType(nameof(ArgumentNullException));
                MethodDefinition argumentNullExceptionWithMessageConstructor = argumentNullExceptionType.Methods.Single(method =>
                    method.IsConstructor
                    && method.Parameters.Count == 2
                    && method.Parameters[0].ParameterType.Name == nameof(String)
                    && method.Parameters[1].ParameterType.Name == nameof(String));

                this.argumentNullExceptionWithMessageConstructor = this.weaver.ModuleDefinition.ImportReference(argumentNullExceptionWithMessageConstructor);
            }

            return this.argumentNullExceptionWithMessageConstructor;
        }

        public MethodReference GetConcatThreeStringsMethod()
        {
            if (this.concatThreeStringsMethod == null)
            {
                TypeDefinition stringType = this.weaver.FindType(nameof(String));
                MethodDefinition concatThreeStringsMethod = stringType.Methods.Single(method =>
                    method.Name == nameof(string.Concat)
                    && method.IsStatic
                    && method.Parameters.Count == 3
                    && method.Parameters[0].ParameterType.Name == nameof(String)
                    && method.Parameters[1].ParameterType.Name == nameof(String)
                    && method.Parameters[2].ParameterType.Name == nameof(String));

                this.concatThreeStringsMethod = this.weaver.ModuleDefinition.ImportReference(concatThreeStringsMethod);
            }

            return this.concatThreeStringsMethod;
        }
    }
}
