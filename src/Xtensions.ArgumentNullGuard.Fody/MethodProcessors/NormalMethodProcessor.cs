namespace Xtensions.ArgumentNullGuard.Fody.MethodProcessors
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;

    internal class NormalMethodProcessor
    {
        private readonly HelperMethods helperMethods;

        public NormalMethodProcessor(HelperMethods helperMethods)
        {
            this.helperMethods = helperMethods;
        }

        public void ProcessMethod(MethodDefinition method)
        {
            IReadOnlyCollection<Instruction> guardInstructions = method.Parameters
                .Where(parameter => parameter.ShouldInjectNullCheck(method))
                .SelectMany(parameter => this.helperMethods.GetInstructionsToCallEnsureNotNull(parameter))
                .ToList();

            method.Body.SimplifyMacros();
            method.Body.Instructions.Prepend(guardInstructions);
            method.Body.OptimizeMacros();
        }
    }
}
