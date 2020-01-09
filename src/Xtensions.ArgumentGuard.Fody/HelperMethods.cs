namespace Xtensions.ArgumentGuard.Fody
{
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentGuard.Fody.Extensions;
    using Xtensions.ArgumentGuard.Fody.LibraryReferences;

    internal class HelperMethods
    {
        internal const string ClassName = "ArgumentGuardHelpers";

        private readonly ModuleDefinition moduleDefinition;
        private readonly ExceptionReferences exceptionReferences;
        private readonly StringReferences stringReferences;

        private TypeDefinition? argumentGuardHelpersType;
        private MethodDefinition? ensureNotNullMethod;

        public HelperMethods(ModuleDefinition moduleDefinition, ExceptionReferences exceptionReferences, StringReferences stringReferences)
        {
            this.moduleDefinition = moduleDefinition;
            this.exceptionReferences = exceptionReferences;
            this.stringReferences = stringReferences;
        }

        public MethodDefinition GetEnsureNotNullMethod()
        {
            if (this.ensureNotNullMethod == null)
            {
                MethodDefinition method = new MethodDefinition(
                    name: "EnsureNotNull",
                    attributes: MethodAttributes.Public | MethodAttributes.Static,
                    returnType: this.moduleDefinition.TypeSystem.Void);

                ParameterDefinition valueParameter = new ParameterDefinition(
                    name: "value",
                    attributes: ParameterAttributes.None,
                    parameterType: this.moduleDefinition.TypeSystem.Object);

                ParameterDefinition parameterNameParameter = new ParameterDefinition(
                    name: "parameterName",
                    attributes: ParameterAttributes.None,
                    parameterType: this.moduleDefinition.TypeSystem.String);

                method.Parameters.Add(valueParameter);
                method.Parameters.Add(parameterNameParameter);

                Instruction returnInstruction = Instruction.Create(OpCodes.Ret);

                method.Body.Instructions.AddRange(new[]
                {
                    Instruction.Create(OpCodes.Ldarg, valueParameter),
                    Instruction.Create(OpCodes.Ldnull),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Brfalse, returnInstruction),

                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Ldstr, "Parameter '"),
                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Ldstr, "' is null."),
                    Instruction.Create(OpCodes.Call, this.stringReferences.ConcatThreeStringsMethod.Value),
                    Instruction.Create(OpCodes.Newobj, this.exceptionReferences.ArgumentNullExceptionWithMessageConstructor.Value),
                    Instruction.Create(OpCodes.Throw),

                    returnInstruction,
                });

                method.Body.OptimizeMacros();

                this.GetArgumentGuardHelpersType().Methods.Add(method);
                this.ensureNotNullMethod = method;
            }

            return this.ensureNotNullMethod;
        }

        private TypeDefinition GetArgumentGuardHelpersType()
        {
            if (this.argumentGuardHelpersType == null)
            {
                TypeDefinition type = new TypeDefinition(
                    @namespace: "Xtensions.ArgumentGuard",
                    name: ClassName,
                    attributes: TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit,
                    baseType: this.moduleDefinition.TypeSystem.Object);

                this.moduleDefinition.Types.Add(type);
                this.argumentGuardHelpersType = type;
            }

            return this.argumentGuardHelpersType;
        }
    }
}
