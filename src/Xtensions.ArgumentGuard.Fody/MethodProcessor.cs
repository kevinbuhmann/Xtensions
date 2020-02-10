namespace Xtensions.ArgumentGuard.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentGuard.Fody.Extensions;

    internal class MethodProcessor
    {
        private readonly HelperMethods helperMethods;

        public MethodProcessor(HelperMethods helperMethods)
        {
            this.helperMethods = helperMethods;
        }

        public void ProcessMethod(MethodDefinition method)
        {
            MetadataHelpers.ValidateMetadata(method);

            IReadOnlyCollection<Instruction> guardInstructions = method.Parameters
                .SelectMany(parameter => this.GetGuardInstructions(parameter, method))
                .ToList();

            if (guardInstructions.Any())
            {
                method.Body.SimplifyMacros();
                method.Body.Instructions.Prepend(guardInstructions);
                method.Body.OptimizeMacros();
            }
        }

        private static bool ShouldInjectNullGuard(ParameterDefinition parameter, MethodDefinition method)
        {
            return parameter.IsOut == false
                && parameter.ParameterType.IsReferenceType()
                && parameter.IsNullableReferenceTypeParameter(method) == false;
        }

        private static bool ShouldInjectEmptyStringGuard(ParameterDefinition parameter)
        {
            return parameter.IsOut == false
                && parameter.ParameterType.FullName == typeof(string).FullName
                && parameter.HasCustomAttribute(typeof(AllowEmptyAttribute)) == false;
        }

        private static bool ShouldInjectEnumGuard(ParameterDefinition parameter, out TypeReference enumType, out bool nullable)
        {
            nullable = default;
            enumType = default!;

            return parameter.IsOut == false
                && parameter.ParameterType.IsEnumType(out enumType, out nullable);
        }

        private static IEnumerable<Instruction> GetInstructionsToCallGuardMethod(ParameterDefinition parameter, MethodReference guardMethod)
        {
            yield return Instruction.Create(OpCodes.Ldarg, parameter);

            if (parameter.ParameterType.IsByReference)
            {
                yield return Instruction.Create(OpCodes.Ldind_Ref);
            }

            yield return Instruction.Create(OpCodes.Ldstr, parameter.Name);
            yield return Instruction.Create(OpCodes.Call, guardMethod);
        }

        private IEnumerable<Instruction> GetGuardInstructions(ParameterDefinition parameter, MethodDefinition method)
        {
            if (ShouldInjectNullGuard(parameter, method))
            {
                foreach (Instruction instruction in this.GetNullGuardInstructions(parameter))
                {
                    yield return instruction;
                }
            }

            if (ShouldInjectEmptyStringGuard(parameter))
            {
                foreach (Instruction instruction in this.GetEmptyStringGuardInstructions(parameter))
                {
                    yield return instruction;
                }
            }

            if (ShouldInjectEnumGuard(parameter, out TypeReference enumType, out bool nullable))
            {
                foreach (Instruction instruction in this.GetEnumGuardInstructions(parameter, enumType, nullable))
                {
                    yield return instruction;
                }
            }
        }

        private IEnumerable<Instruction> GetNullGuardInstructions(ParameterDefinition parameter)
        {
            return GetInstructionsToCallGuardMethod(
                parameter: parameter,
                guardMethod: this.helperMethods.GetEnsureNotNullMethod());
        }

        private IEnumerable<Instruction> GetEmptyStringGuardInstructions(ParameterDefinition parameter)
        {
            return GetInstructionsToCallGuardMethod(
                parameter: parameter,
                guardMethod: this.helperMethods.GetEnsureNotEmptyMethod());
        }

        private IEnumerable<Instruction> GetEnumGuardInstructions(ParameterDefinition parameter, TypeReference enumType, bool nullable)
        {
            return GetInstructionsToCallGuardMethod(
                parameter: parameter,
                guardMethod: this.helperMethods.GetEnumGuardMethod(enumType, nullable: nullable));
        }
    }
}
