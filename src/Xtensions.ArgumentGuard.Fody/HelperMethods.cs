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
        private readonly EnumReferences enumReferences;
        private readonly ExceptionReferences exceptionReferences;
        private readonly NullableReferences nullableReferences;
        private readonly ObjectReferences objectReferences;
        private readonly ReflectionReferences reflectionReferences;
        private readonly StringReferences stringReferences;

        private TypeDefinition? argumentGuardHelpersType;
        private MethodDefinition? ensureNotNullMethod;
        private MethodDefinition? ensureNotEmptyMethod;
        private MethodDefinition? ensureValidEnumValueMethod;
        private MethodDefinition? ensureNullOrValidEnumValueMethod;

        public HelperMethods(
            ModuleDefinition moduleDefinition,
            EnumReferences enumReferences,
            ExceptionReferences exceptionReferences,
            NullableReferences nullableReferences,
            ObjectReferences objectReferences,
            ReflectionReferences reflectionReferences,
            StringReferences stringReferences)
        {
            this.moduleDefinition = moduleDefinition;
            this.enumReferences = enumReferences;
            this.exceptionReferences = exceptionReferences;
            this.objectReferences = objectReferences;
            this.nullableReferences = nullableReferences;
            this.reflectionReferences = reflectionReferences;
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

        public MethodDefinition GetEnsureNotEmptyMethod()
        {
            if (this.ensureNotEmptyMethod == null)
            {
                MethodDefinition method = new MethodDefinition(
                    name: "EnsureNotEmpty",
                    attributes: MethodAttributes.Public | MethodAttributes.Static,
                    returnType: this.moduleDefinition.TypeSystem.Void);

                ParameterDefinition valueParameter = new ParameterDefinition(
                    name: "value",
                    attributes: ParameterAttributes.None,
                    parameterType: this.moduleDefinition.TypeSystem.String);

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
                    Instruction.Create(OpCodes.Ldstr, string.Empty),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Brfalse, returnInstruction),

                    Instruction.Create(OpCodes.Ldstr, "Parameter '"),
                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Ldstr, "' is empty."),
                    Instruction.Create(OpCodes.Call, this.stringReferences.ConcatThreeStringsMethod.Value),
                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Newobj, this.exceptionReferences.ArgumentExceptionWithMessageConstructor.Value),
                    Instruction.Create(OpCodes.Throw),

                    returnInstruction,
                });

                method.Body.OptimizeMacros();

                this.GetArgumentGuardHelpersType().Methods.Add(method);
                this.ensureNotEmptyMethod = method;
            }

            return this.ensureNotEmptyMethod;
        }

        public MethodReference GetEnumGuardMethod(TypeReference enumType, bool nullable)
        {
            MethodDefinition method = this.GetEnumGuardMethod(nullable: nullable);

            GenericInstanceMethod genericMethod = new GenericInstanceMethod(method);
            genericMethod.GenericArguments.Add(enumType);

            return genericMethod;
        }

        private MethodDefinition GetEnumGuardMethod(bool nullable)
        {
            if ((nullable && this.ensureNullOrValidEnumValueMethod == null) || (!nullable && this.ensureValidEnumValueMethod == null))
            {
                MethodDefinition method = new MethodDefinition(
                    name: $"Ensure{(nullable ? "NullOr" : string.Empty)}ValidEnumValue",
                    attributes: MethodAttributes.Public | MethodAttributes.Static,
                    returnType: this.moduleDefinition.TypeSystem.Void);

                GenericParameter enumTypeParameter = new GenericParameter(name: "TEnum", owner: method)
                {
                    HasNotNullableValueTypeConstraint = nullable,
                };

                enumTypeParameter.Constraints.Add(new GenericParameterConstraint(this.enumReferences.EnumType.Value));
                method.GenericParameters.Add(enumTypeParameter);

                TypeReference valueParameterType = nullable
                    ? this.nullableReferences.NullableType.Value.MakeGenericInstanceType(enumTypeParameter)
                    : enumTypeParameter as TypeReference;

                ParameterDefinition valueParameter = new ParameterDefinition(
                    name: "value",
                    attributes: ParameterAttributes.None,
                    parameterType: valueParameterType);

                ParameterDefinition parameterNameParameter = new ParameterDefinition(
                    name: "parameterName",
                    attributes: ParameterAttributes.None,
                    parameterType: this.moduleDefinition.TypeSystem.String);

                method.Parameters.Add(valueParameter);
                method.Parameters.Add(parameterNameParameter);

                VariableDefinition firstCharVariable = new VariableDefinition(
                    variableType: this.moduleDefinition.TypeSystem.Char);

                method.Body.Variables.Add(firstCharVariable);

                Instruction returnInstruction = Instruction.Create(OpCodes.Ret);

                if (nullable)
                {
                    method.Body.Instructions.AddRange(new[]
                    {
                        Instruction.Create(OpCodes.Ldarga, valueParameter),
                        Instruction.Create(OpCodes.Call, this.nullableReferences.HasValueGetMethod.Value.MakeDeclaringTypeGeneric(enumTypeParameter)),
                        Instruction.Create(OpCodes.Brfalse, returnInstruction),
                    });
                }

                Instruction loadFirstCharVariableToCheckForMinusSignInstruction = Instruction.Create(OpCodes.Ldloc, firstCharVariable);
                Instruction loadExceptionMessageStringFormatInstruction = Instruction.Create(OpCodes.Ldstr, "{0} is not a valid value for the {1} enum.");

                method.Body.Instructions.AddRange(new[]
                {
                    Instruction.Create(OpCodes.Ldarga, valueParameter),
                    Instruction.Create(OpCodes.Constrained, valueParameterType),
                    Instruction.Create(OpCodes.Callvirt, this.objectReferences.ToStringMethod.Value),
                    Instruction.Create(OpCodes.Ldc_I4, 0),
                    Instruction.Create(OpCodes.Callvirt, this.stringReferences.IndexerGetMethod.Value),
                    Instruction.Create(OpCodes.Stloc, firstCharVariable),

                    Instruction.Create(OpCodes.Ldloc, firstCharVariable),
                    Instruction.Create(OpCodes.Ldc_I4, '0'),
                    Instruction.Create(OpCodes.Blt, loadFirstCharVariableToCheckForMinusSignInstruction),

                    Instruction.Create(OpCodes.Ldloc, firstCharVariable),
                    Instruction.Create(OpCodes.Ldc_I4, '9'),
                    Instruction.Create(OpCodes.Ble, loadExceptionMessageStringFormatInstruction),

                    loadFirstCharVariableToCheckForMinusSignInstruction,
                    Instruction.Create(OpCodes.Ldc_I4, '-'),
                    Instruction.Create(OpCodes.Bne_Un, returnInstruction),

                    loadExceptionMessageStringFormatInstruction,
                    Instruction.Create(OpCodes.Ldarg, valueParameter),
                    Instruction.Create(OpCodes.Box, valueParameterType),
                    Instruction.Create(OpCodes.Ldtoken, enumTypeParameter),
                    Instruction.Create(OpCodes.Call, this.reflectionReferences.GetTypeFromHandleMethod.Value),
                    Instruction.Create(OpCodes.Callvirt, this.reflectionReferences.MemberInfoNamePropertyGetMethod.Value),
                    Instruction.Create(OpCodes.Call, this.stringReferences.FormatTwoArgsMethod.Value),
                    Instruction.Create(OpCodes.Ldarg, parameterNameParameter),
                    Instruction.Create(OpCodes.Newobj, this.exceptionReferences.ArgumentExceptionWithMessageConstructor.Value),
                    Instruction.Create(OpCodes.Throw),
                });

                method.Body.Instructions.Add(returnInstruction);

                method.Body.OptimizeMacros();

                this.GetArgumentGuardHelpersType().Methods.Add(method);

                if (nullable)
                {
                    this.ensureNullOrValidEnumValueMethod = method;
                }
                else
                {
                    this.ensureValidEnumValueMethod = method;
                }
            }

            if (nullable)
            {
                return this.ensureNullOrValidEnumValueMethod!;
            }
            else
            {
                return this.ensureValidEnumValueMethod!;
            }
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
