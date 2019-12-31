namespace Xtensions.ArgumentNullGuard.Fody
{
    using System;
    using Mono.Cecil;
    using Reflection = System.Reflection;

    internal class LibraryMethods
    {
        private readonly ModuleDefinition moduleDefinition;

        private MethodReference? argumentNullExceptionWithMessageConstructor;
        private MethodReference? concatThreeStringsMethod;

        public LibraryMethods(ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;
        }

        public MethodReference GetArgumentNullExceptionWithMessageConstructor()
        {
            if (this.argumentNullExceptionWithMessageConstructor == null)
            {
                Reflection.ConstructorInfo argumentNullExceptionWithMessageConstructor = typeof(ArgumentNullException).GetConstructor(
                    types: new[] { typeof(string), typeof(string) });

                this.argumentNullExceptionWithMessageConstructor = this.moduleDefinition.ImportReference(argumentNullExceptionWithMessageConstructor);
            }

            return this.argumentNullExceptionWithMessageConstructor;
        }

        public MethodReference GetConcatThreeStringsMethod()
        {
            if (this.concatThreeStringsMethod == null)
            {
                Reflection.MethodInfo concatThreeStringsMethod = typeof(string).GetMethod(
                    name: "Concat",
                    types: new[] { typeof(string), typeof(string), typeof(string) });

                this.concatThreeStringsMethod = this.moduleDefinition.ImportReference(concatThreeStringsMethod);
            }

            return this.concatThreeStringsMethod;
        }
    }
}
