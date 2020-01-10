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
                TypeDefinition argumentNullExceptionType = this.weaver.FindType(typeof(ArgumentNullException).FullName);
                MethodDefinition argumentNullExceptionWithMessageConstructor = argumentNullExceptionType.Methods.Single(method =>
                    method.IsConstructor
                    && method.Parameters.Count == 2
                    && method.Parameters[0].ParameterType.FullName == typeof(string).FullName
                    && method.Parameters[1].ParameterType.FullName == typeof(string).FullName);

                this.argumentNullExceptionWithMessageConstructor = this.weaver.ModuleDefinition.ImportReference(argumentNullExceptionWithMessageConstructor);
            }

            return this.argumentNullExceptionWithMessageConstructor;
        }

        public MethodReference GetConcatThreeStringsMethod()
        {
            if (this.concatThreeStringsMethod == null)
            {
                TypeDefinition stringType = this.weaver.FindType(typeof(string).FullName);
                MethodDefinition concatThreeStringsMethod = stringType.Methods.Single(method =>
                    method.Name == nameof(string.Concat)
                    && method.IsStatic
                    && method.Parameters.Count == 3
                    && method.Parameters[0].ParameterType.FullName == typeof(string).FullName
                    && method.Parameters[1].ParameterType.FullName == typeof(string).FullName
                    && method.Parameters[2].ParameterType.FullName == typeof(string).FullName);

                this.concatThreeStringsMethod = this.weaver.ModuleDefinition.ImportReference(concatThreeStringsMethod);
            }

            return this.concatThreeStringsMethod;
        }
    }
}
