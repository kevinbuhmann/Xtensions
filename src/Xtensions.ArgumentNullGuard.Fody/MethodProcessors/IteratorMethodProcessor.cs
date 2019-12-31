namespace Xtensions.ArgumentNullGuard.Fody.MethodProcessors
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal class IteratorMethodProcessor : StateMachineMethodProcessor
    {
        public IteratorMethodProcessor(HelperMethods helperMethods)
            : base(helperMethods)
        {
        }

        [ExcludeFromCodeCoverage] // Error case not able to be covered.
        protected override Instruction GetEntryInstruction(MethodDefinition moveNextMethod, TypeDefinition stateMachineTypeDefinition)
        {
            Collection<Instruction> instructions = moveNextMethod.Body.Instructions;
            FieldDefinition stateField = stateMachineTypeDefinition.Fields
                .Single(field => field.Name.EndsWith("__state", StringComparison.Ordinal));

            for (int i = 2; i < instructions.Count; ++i)
            {
                if (instructions.ElementAt(i - 2).IsLoadInt32Constant(value: -1)
                    && instructions.ElementAt(i - 1).IsSetField(stateField.Name))
                {
                    return instructions[i];
                }
            }

            throw new Exception("Iterator state machine entry instruction not found.");
        }
    }
}
