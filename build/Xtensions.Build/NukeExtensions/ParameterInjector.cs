namespace Xtensions.Build.NukeExtensions
{
    using System.Reflection;
    using EnsureThat;
    using Nuke.Common.Execution;

    public static class ParameterInjector
    {
        public static void InjectParameters(object value)
        {
            EnsureArg.IsNotNull(value, nameof(value));

            MethodInfo method = typeof(InjectionUtility).GetMethod(nameof(InjectionUtility.InjectValues)).MakeGenericMethod(value.GetType());
            method.Invoke(null, new object[] { value, null });
        }
    }
}
