namespace Xtensions.ArgumentNullGuard.Fody.Extensions
{
    using System.Linq;
    using Mono.Cecil;

    // https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md
    internal static class NullableMetadataExtensions
    {
        private const string NullableAttributeName = "NullableAttribute";
        private const string NullableContextAttributeName = "NullableContextAttribute";
        private const byte AnnotatedNullableFlag = 2;

        public static bool IsNullableReferenceTypeParameter(this ParameterDefinition parameter, MethodDefinition method)
        {
            bool? annotatedNullable = IsAnnotatedNullable(
                element: parameter,
                attributeName: NullableAttributeName);

            return annotatedNullable ?? IsNullableContext(method);
        }

        private static bool IsNullableContext(MethodDefinition method)
        {
            bool? annotatedNullable = IsAnnotatedNullable(
                element: method,
                attributeName: NullableContextAttributeName);

            return annotatedNullable ?? IsNullableContext(method.DeclaringType);
        }

        private static bool IsNullableContext(TypeDefinition type)
        {
            bool? annotatedNullable = IsAnnotatedNullable(
                element: type,
                attributeName: NullableContextAttributeName);

            return annotatedNullable ?? IsNullableContext(type.DeclaringType);
        }

        private static bool? IsAnnotatedNullable(ICustomAttributeProvider element, string attributeName)
        {
            CustomAttribute? attribute = element.CustomAttributes
                .SingleOrDefault(attribute => attribute.AttributeType.Name == attributeName);

            byte? flag = null;

            if (attribute != null)
            {
                object flagOrFlags = attribute.ConstructorArguments.Single().Value;
                flag = flagOrFlags is CustomAttributeArgument[] flags ? (byte)flags.First().Value : (byte)flagOrFlags;
            }

            return flag.HasValue ? flag.Value == AnnotatedNullableFlag : null as bool?;
        }
    }
}
