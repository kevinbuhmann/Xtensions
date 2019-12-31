namespace Xtensions.ArgumentNullGuard.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using global::Fody;
    using Mono.Cecil;
    using Xtensions.ArgumentNullGuard.Fody.Extensions;
    using Xtensions.ArgumentNullGuard.Fody.MethodProcessors;

    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            LibraryMethods libraryMethods = new LibraryMethods(this.ModuleDefinition);
            HelperMethods helperMethods = new HelperMethods(this.ModuleDefinition, libraryMethods);
            AsyncMethodProcessor asyncMethodProcessor = new AsyncMethodProcessor(helperMethods);
            IteratorMethodProcessor iteratorMethodProcessor = new IteratorMethodProcessor(helperMethods);
            NormalMethodProcessor normalMethodProcessor = new NormalMethodProcessor(helperMethods);

            IEnumerable<MethodDefinition> methodsToProcess = this.ModuleDefinition.GetTypes()
                .ToList() // prevent processing dynamically added types
                .Where(type => type.IsInterface == false && type.IsGeneratedCode() == false)
                .SelectMany(type => type.Methods)
                .Where(method => method.Body != null && method.IsGeneratedCode() == false);

            foreach (MethodDefinition method in methodsToProcess)
            {
                if (method.TryGetAttribute(name: nameof(AsyncStateMachineAttribute), out CustomAttribute asyncStateMachineAttribute))
                {
                    asyncMethodProcessor.ProcessMethod(method, asyncStateMachineAttribute);
                }
                else if (method.TryGetAttribute(name: nameof(IteratorStateMachineAttribute), out CustomAttribute iteratorStateMachineAttribute))
                {
                    iteratorMethodProcessor.ProcessMethod(method, iteratorStateMachineAttribute);
                }
                else
                {
                    normalMethodProcessor.ProcessMethod(method);
                }
            }
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
        }
    }
}
