namespace Xtensions.ArgumentNullGuard.Fody.Extensions
{
    using System;
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
                typeof(GeneratedCodeAttribute).FullName,
                typeof(CompilerGeneratedAttribute).FullName,
            };

            return element.CustomAttributes.Any(attribute => generatedCodeAttributeNames.Contains(attribute.AttributeType.FullName));
        }

        public static bool IsReferenceType(this TypeReference type)
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
                && genericParameter.Constraints.All(constraint => constraint.ConstraintType.FullName == typeof(Enum).FullName);
        }
    }
}
