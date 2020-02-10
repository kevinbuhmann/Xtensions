namespace Xtensions.ArgumentGuard.Fody
{
    using global::Fody;
    using Mono.Cecil;
    using Xtensions.ArgumentGuard.Fody.Extensions;

    public static class ValidationHelpers
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

        public static void ValidateEnumConstraints(MethodDefinition method)
        {
            foreach (ParameterDefinition parameter in method.Parameters)
            {
                if (parameter.ParameterType is GenericParameter genericParameter && genericParameter.HasEnumConstraint() && genericParameter.HasNotNullableValueTypeConstraint == false)
                {
                    throw new WeavingException($"Generic parameter '{genericParameter.Name}' of method (or one of its containing types) '{method.DeclaringType.FullName}.{method.Name}' should be constrained to value types (enums are always value types).");
                }
            }
        }
    }
}
