namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::Fody;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;

    public class BaseModuleWeaverTests : IDisposable
    {
        private string? assemblyPath;
        private string? weavedAssemblyPath;
        private bool disposed = false;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected static void InvokeMethod(MethodInfo method, IReadOnlyCollection<object?>? parameters, object? target = null)
        {
            try
            {
                method.Invoke(obj: target, parameters: parameters.ToArray());
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw targetInvocationException.InnerException!;
            }
        }

        protected Assembly WeaveAssembly(string sourceCode, OptimizationLevel optimizationLevel)
        {
            this.assemblyPath = CompileAssembly(sourceCode, optimizationLevel);

            ModuleWeaver weaver = new ModuleWeaver();
            TestResult testResult = weaver.ExecuteTestRun(assemblyPath: this.assemblyPath, runPeVerify: false);

            this.weavedAssemblyPath = testResult.AssemblyPath;

            return testResult.Assembly;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed == false)
            {
                if (disposing)
                {
                    if (this.assemblyPath != null)
                    {
                        File.Delete(this.assemblyPath);
                    }

                    if (this.weavedAssemblyPath != null)
                    {
                        File.Delete(this.weavedAssemblyPath);
                    }
                }

                this.disposed = true;
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Sorry.")]
        private static string CompileAssembly(string sourceCode, OptimizationLevel optimizationLevel)
        {
            CSharpParseOptions parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);

            SyntaxTree syntaxTree = SyntaxFactory.ParseSyntaxTree(
                text: SourceText.From(sourceCode),
                options: parseOptions);

            CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(
                outputKind: OutputKind.DynamicallyLinkedLibrary,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                nullableContextOptions: NullableContextOptions.Enable,
                optimizationLevel: optimizationLevel);

            IReadOnlyCollection<MetadataReference> references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.CodeDom.Compiler.GeneratedCodeAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            };

            string assemblyPath = Path.GetTempFileName();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: Path.GetFileName(assemblyPath),
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: compilationOptions);

            ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();

            if (diagnostics.Any())
            {
#pragma warning disable CA2201 // Do not raise reserved exception types
                throw new Exception(string.Join(Environment.NewLine, diagnostics.Select(diagnostic => diagnostic.ToString())));
#pragma warning restore CA2201 // Do not raise reserved exception types
            }

            compilation.Emit(outputPath: assemblyPath);

            return assemblyPath;
        }
    }
}
