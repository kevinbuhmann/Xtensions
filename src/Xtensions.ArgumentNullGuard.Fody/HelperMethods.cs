namespace Xtensions.ArgumentNullGuard.Fody
{
    using System.Collections.Generic;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal class HelperMethods
    {
        internal const string ClassName = "ArgumentNullGuardHelpers";

        private readonly ModuleDefinition moduleDefinition;
        private readonly LibraryMethods libraryMethods;

        private TypeDefinition? argumentNullGuardHelpersType;
        private MethodDefinition? ensureNotNullMethod;

        public HelperMethods(ModuleDefinition moduleDefinition, LibraryMethods libraryMethods)
        {
            this.moduleDefinition = moduleDefinition;
            this.libraryMethods = libraryMethods;
        }

        public IEnumerable<Instruction> GetInstructionsToCallEnsureNotNull(ParameterDefinition parameter)
        {
            yield return Instruction.Create(OpCodes.Ldarg, parameter);

            if (parameter.ParameterType.IsByReference)
            {
                yield return Instruction.Create(OpCodes.Ldind_Ref);
            }

            yield return Instruction.Create(OpCodes.Ldstr, parameter.Name);
            yield return Instruction.Create(OpCodes.Call, this.GetEnsureNotNullMethod());
        }

        public IEnumerable<Instruction> GetInstructionsToCallEnsureNotNull(FieldReference field)
        {
            yield return Instruction.Create(OpCodes.Ldarg_0); // load 'this'
            yield return Instruction.Create(OpCodes.Ldfld, field);
            yield return Instruction.Create(OpCodes.Ldstr, field.Name);
            yield return Instruction.Create(OpCodes.Call, this.GetEnsureNotNullMethod());
        }

        private MethodDefinition GetEnsureNotNullMethod()
        {
            if (this.ensureNotNullMethod == null)
            {
                this.ensureNotNullMethod = new MethodDefinition(
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

                this.ensureNotNullMethod.Parameters.Add(valueParameter);
                this.ensureNotNullMethod.Parameters.Add(parameterNameParameter);

                Instruction returnInstruction = Instruction.Create(OpCodes.Ret);

                IReadOnlyCollection<Instruction> instructions = new[]
                {
                    Instruction.Create(OpCodes.Ldarg, valueParameter),
                    Instruction.Create(OpCodes.Ldnull),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Brfalse, returnInstruction),

                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Ldstr, "Parameter '"),
                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Ldstr, "' is null."),
                    Instruction.Create(OpCodes.Call, this.libraryMethods.GetConcatThreeStringsMethod()),
                    Instruction.Create(OpCodes.Newobj, this.libraryMethods.GetArgumentNullExceptionWithMessageConstructor()),
                    Instruction.Create(OpCodes.Throw),

                    returnInstruction,
                };

                this.ensureNotNullMethod.Body.Instructions.AddRange(instructions);
                this.ensureNotNullMethod.Body.OptimizeMacros();

                this.GetArgumentNullGuardHelpersType().Methods.Add(this.ensureNotNullMethod);
            }

            return this.ensureNotNullMethod;
        }

        private TypeDefinition GetArgumentNullGuardHelpersType()
        {
            if (this.argumentNullGuardHelpersType == null)
            {
                this.argumentNullGuardHelpersType = new TypeDefinition(
                    @namespace: "Xtensions.ArgumentNullGuard",
                    name: ClassName,
                    attributes: TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit,
                    baseType: this.moduleDefinition.TypeSystem.Object);

                this.moduleDefinition.Types.Add(this.argumentNullGuardHelpersType);
            }

            return this.argumentNullGuardHelpersType;
        }
    }
}
