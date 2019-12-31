namespace Xtensions.ArgumentNullGuard.Fody.MethodProcessors
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal abstract class StateMachineMethodProcessor
    {
        private readonly HelperMethods helperMethods;

        public StateMachineMethodProcessor(HelperMethods helperMethods)
        {
            this.helperMethods = helperMethods;
        }

        public void ProcessMethod(MethodDefinition method, CustomAttribute asyncStateMachineAttribute)
        {
            TypeDefinition stateMachineTypeDefinition = (TypeDefinition)asyncStateMachineAttribute.ConstructorArguments.Single().Value;

            IReadOnlyCollection<ParameterDefinition> parametersToCheck = method.Parameters
                .Where(parameter => parameter.ShouldInjectNullCheck(method))
                .ToList();

            IReadOnlyCollection<FieldDefinition> stateMachineFieldsToCheck = parametersToCheck
                .Select(parameter => stateMachineTypeDefinition.Fields.SingleOrDefault(field => field.Name == parameter.Name))
                .Where(field => field != null) // release mode builds don't include fields for unused parameters
                .ToList();

            if (stateMachineFieldsToCheck.Any())
            {
                IReadOnlyCollection<Instruction> guardInstructions = stateMachineFieldsToCheck
                    .SelectMany(field => this.helperMethods.GetInstructionsToCallEnsureNotNull(field))
                    .ToList();

                MethodDefinition moveNextMethod = stateMachineTypeDefinition.Methods.Single(x => x.Name == "MoveNext");

                moveNextMethod.Body.SimplifyMacros();

                moveNextMethod.Body.Instructions.Insert(
                    index: moveNextMethod.Body.Instructions.IndexOf(this.GetEntryInstruction(moveNextMethod, stateMachineTypeDefinition)),
                    items: guardInstructions);

                moveNextMethod.Body.OptimizeMacros();
            }

            // Since release mode builds don't include fields for unused parameters, those parameters have to be checked directly.
            // This is here to provide behavior that is as consistent as possible optimized and non-optimized builds.
            // The difference is that unused parameters will be checked immediately when the method is executed instead of
            // being checked in the first step of the state machine.
            IReadOnlyCollection<ParameterDefinition> unusedParametersToCheck = parametersToCheck
                .Where(parameter => stateMachineFieldsToCheck.SingleOrDefault(field => field.Name == parameter.Name) == null)
                .ToList();

            if (unusedParametersToCheck.Any())
            {
                IReadOnlyCollection<Instruction> guardInstructions = unusedParametersToCheck
                    .SelectMany(parameter => this.helperMethods.GetInstructionsToCallEnsureNotNull(parameter))
                    .ToList();

                method.Body.SimplifyMacros();
                method.Body.Instructions.Prepend(guardInstructions);
                method.Body.OptimizeMacros();
            }
        }

        protected abstract Instruction GetEntryInstruction(MethodDefinition moveNextMethod, TypeDefinition stateMachineTypeDefinition);
    }
}
