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
                .Where(parameter => parameter.ShouldInjectNullCheck(method))
                .SelectMany(parameter => this.helperMethods.GetInstructionsToCallEnsureNotNull(parameter))
                .ToList();

            if (guardInstructions.Any())
            {
                method.Body.SimplifyMacros();
                method.Body.Instructions.Prepend(guardInstructions);
                method.Body.OptimizeMacros();
            }
        }
    }
}
