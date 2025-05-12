using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace MyFirstGenerator
{
    [UsesVerify]
    [TestClass]
    public partial class EqualsOperatorSourceGeneratorTests
    {
        [TestMethod]
        public Task Baseline()
        {
            var driver = BuildDriver(null);
            return Verify(driver);
        }

        [TestMethod]
        public Task BasicAttribute()
        {
            var source = @"
namespace MyNamespace
{
    [MyFirstGenerator.EqualsOperator]
    public partial class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is MyClass other)
                return Id == other.Id && Name == other.Name;
            return false;
        }
    }
}";

            var driver = BuildDriver(source, out var output, out var diagnostics);
            return Verify(driver);
        }

        static GeneratorDriver BuildDriver(
            [StringSyntax("C#-Test")] string? source) => BuildDriver(source, out _, out _);

        static GeneratorDriver BuildDriver(
            [StringSyntax("C#-Test")] string? source, 
            out Compilation outputCompilation, 
            out ImmutableArray<Diagnostic> diagnostics)
        {
            IEnumerable<SyntaxTree>? syntaxTrees = null;
            if (source != null)
            {
                syntaxTrees = [CSharpSyntaxTree.ParseText(source)];
            }

            var compilation = CSharpCompilation.Create(
                "VerifyTests",
                syntaxTrees: syntaxTrees,
                references: [
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // corelib
                ]
                );

            var generator = new EqualsOperatorSourceGenerator();
            var driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(compilation, out outputCompilation, out diagnostics);
        }
    }
}
