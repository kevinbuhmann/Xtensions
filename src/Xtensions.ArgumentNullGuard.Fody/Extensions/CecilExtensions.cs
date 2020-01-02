namespace Xtensions.ArgumentNullGuard.Fody.Extensions
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Mono.Cecil;

    internal static class CecilExtensions
    {
        public static bool IsGeneratedCode(this ICustomAttributeProvider element)
        {
            IReadOnlyCollection<string> generatedCodeAttributeNames = new[]
            {
                nameof(GeneratedCodeAttribute),
                nameof(CompilerGeneratedAttribute),
            };

            return element.CustomAttributes.Any(attribute => generatedCodeAttributeNames.Contains(attribute.AttributeType.Name));
        }

        public static bool ShouldInjectNullCheck(this ParameterDefinition parameter, MethodDefinition method)
        {
            return parameter.IsOut == false
                && parameter.ParameterType.IsReferenceType()
                && parameter.IsNullableReferenceTypeParameter(method) == false;
        }

        private static bool IsReferenceType(this TypeReference type)
        {
            if (type.IsValueType)
            {
                return false;
            }
            else if (type is ByReferenceType byReferenceType)
            {
                return byReferenceType.ElementType.IsReferenceType();
            }
            else if (type is GenericParameter genericParameter)
            {
                return genericParameter.HasEnumConstraint()
                    ? false
                    : genericParameter.HasNotNullableValueTypeConstraint == false;
            }
            else
            {
                return true;
            }
        }

        private static bool HasEnumConstraint(this GenericParameter genericParameter)
        {
            return genericParameter.HasConstraints
                && genericParameter.Constraints.All(constraint => constraint.ConstraintType.FullName == "System.Enum");
        }
    }
}
