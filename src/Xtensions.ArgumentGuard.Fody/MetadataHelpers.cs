namespace Xtensions.ArgumentGuard.Fody
{
    using global::Fody;
    using Mono.Cecil;
    using Xtensions.ArgumentGuard.Fody.Extensions;

    public static class MetadataHelpers
    {
        public static void ValidateMetadata(MethodDefinition method)
        {
            foreach (ParameterDefinition parameter in method.Parameters)
            {
                if (parameter.ParameterType.FullName != typeof(string).FullName && parameter.HasCustomAttribute(typeof(AllowEmptyAttribute)))
                {
                    throw new WeavingException($"Attribute '{typeof(AllowEmptyAttribute).FullName}' is not allowed on parameter '{parameter.Name}' of method '{method.DeclaringType.FullName}.{method.Name}'.");
                }
            }
        }
    }
}
