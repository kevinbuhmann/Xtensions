namespace Xtensions.ArgumentNullGuard.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal class MethodProcessor
    {
        private readonly HelperMethods helperMethods;

        public MethodProcessor(HelperMethods helperMethods)
        {
            this.helperMethods = helperMethods;
        }

        public void ProcessMethod(MethodDefinition method)
        {
            IReadOnlyCollection<Instruction> guardInstructions = method.Parameters
                .Where(parameter => ShouldInjectNullGuard(parameter, method))
                .SelectMany(parameter => this.GetNullGuardInstructions(parameter))
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

        private IEnumerable<Instruction> GetNullGuardInstructions(ParameterDefinition parameter)
        {
            yield return Instruction.Create(OpCodes.Ldarg, parameter);

            if (parameter.ParameterType.IsByReference)
            {
                yield return Instruction.Create(OpCodes.Ldind_Ref);
            }

            yield return Instruction.Create(OpCodes.Ldstr, parameter.Name);
            yield return Instruction.Create(OpCodes.Call, this.helperMethods.GetEnsureNotNullMethod());
        }
    }
}
