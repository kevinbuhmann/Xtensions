namespace Xtensions.ArgumentNullGuard.Fody.MethodProcessors
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal class AsyncMethodProcessor : StateMachineMethodProcessor
    {
        public AsyncMethodProcessor(HelperMethods helperMethods)
            : base(helperMethods)
        {
        }

        [ExcludeFromCodeCoverage] // Error case not able to be covered.
        protected override Instruction GetEntryInstruction(MethodDefinition moveNextMethod, TypeDefinition stateMachineTypeDefinition)
        {
            Collection<Instruction> instructions = moveNextMethod.Body.Instructions;
            VariableDefinition numVariable = moveNextMethod.Body.Variables.First();

            for (int i = 2; i < instructions.Count; ++i)
            {
                if (instructions.ElementAt(i - 2).IsLoadLocal(numVariable)
                    && instructions.ElementAt(i - 1).IsBranchFalse())
                {
                    return instructions[i];
                }
            }

            throw new Exception("Async state machine entry instruction not found.");
        }
    }
}
