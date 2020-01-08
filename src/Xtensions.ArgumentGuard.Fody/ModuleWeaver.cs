namespace Xtensions.ArgumentGuard.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Fody;
    using Mono.Cecil;
    using Xtensions.ArgumentGuard.Fody.Extensions;
    using Xtensions.ArgumentGuard.Fody.LibraryReferences;

    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            HelperMethods helperMethods = new HelperMethods(
                moduleDefinition: this.ModuleDefinition,
                enumReferences: new EnumReferences(this),
                exceptionReferences: new ExceptionReferences(this),
                nullableReferences: new NullableReferences(this),
                objectReferences: new ObjectReferences(this),
                reflectionReferences: new ReflectionReferences(this),
                stringReferences: new StringReferences(this));
            MethodProcessor methodProcessor = new MethodProcessor(helperMethods);

            IEnumerable<MethodDefinition> methodsToProcess = this.ModuleDefinition.GetTypes()
                .ToList() // prevent processing dynamically added types
                .Where(type => type.IsInterface == false && type.IsGeneratedCode() == false)
                .SelectMany(type => type.Methods)
                .Where(method => method.Body != null && method.IsGeneratedCode() == false);

            foreach (MethodDefinition method in methodsToProcess)
            {
                methodProcessor.ProcessMethod(method);
            }
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
        }
    }
}
