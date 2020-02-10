namespace Xtensions.ArgumentGuard.Fody.Extensions
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
                return genericParameter.HasNotNullableValueTypeConstraint == false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsEnumType(this TypeReference type, out TypeReference enumType, out bool nullable)
        {
            if (type is GenericInstanceType genericInstanceType)
            {
                nullable = type.FullName.StartsWith(typeof(Nullable<>).FullName, StringComparison.Ordinal);
                enumType = nullable ? genericInstanceType.GenericArguments.Single() : type;
            }
            else
            {
                nullable = false;
                enumType = type;
            }

            if (enumType is ByReferenceType byReferenceType)
            {
                return byReferenceType.ElementType.IsEnumType(out enumType, out nullable);
            }
            else if (enumType is GenericParameter genericParameter)
            {
                return genericParameter.HasEnumConstraint();
            }
            else
            {
                return enumType.IsValueType && enumType.Resolve().IsEnum;
            }
        }

        public static bool HasCustomAttribute(this ICustomAttributeProvider element, Type attributeType)
        {
            return element.CustomAttributes.Any(attribute => attribute.AttributeType.FullName == attributeType.FullName);
        }

        public static bool HasEnumConstraint(this GenericParameter genericParameter)
        {
            return genericParameter.HasConstraints
                && genericParameter.Constraints.Any(constraint => constraint.ConstraintType.FullName == typeof(Enum).FullName);
        }
    }
}
