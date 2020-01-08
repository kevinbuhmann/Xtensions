namespace Xtensions.ArgumentGuard.Fody.LibraryReferences
{
    using System;
    using System.Linq;
    using System.Reflection;
    using global::Fody;
    using Mono.Cecil;

    internal class ReflectionReferences
    {
        public ReflectionReferences(BaseModuleWeaver weaver)
        {
            this.GetTypeFromHandleMethod = new Lazy<MethodReference>(() => GetGetTypeFromHandleMethod(weaver));
            this.MemberInfoNamePropertyGetMethod = new Lazy<MethodReference>(() => GetMemberInfoNamePropertyGetMethod(weaver));
        }

        public Lazy<MethodReference> GetTypeFromHandleMethod { get; }

        public Lazy<MethodReference> MemberInfoNamePropertyGetMethod { get; }

        private static MethodReference GetGetTypeFromHandleMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition typeType = weaver.FindType(typeof(Type).FullName);
            MethodDefinition getTypeFromHandleMethod = typeType.Methods.Single(method =>
                method.IsStatic
                && method.Name == nameof(Type.GetTypeFromHandle)
                && method.Parameters.Count == 1
                && method.Parameters[0].ParameterType.FullName == typeof(RuntimeTypeHandle).FullName);

            return weaver.ModuleDefinition.ImportReference(getTypeFromHandleMethod);
        }

        private static MethodReference GetMemberInfoNamePropertyGetMethod(BaseModuleWeaver weaver)
        {
            TypeDefinition memberInfoType = weaver.FindType(typeof(MemberInfo).FullName);
            PropertyDefinition memberInfoNameProperty = memberInfoType.Properties.Single(property => property.Name == nameof(MemberInfo.Name));

            return weaver.ModuleDefinition.ImportReference(memberInfoNameProperty.GetMethod);
        }
    }
}
