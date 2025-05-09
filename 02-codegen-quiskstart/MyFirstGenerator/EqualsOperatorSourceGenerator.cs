using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace MyFirstGenerator.Machin
{
    [Generator]
    public class EqualsOperatorSourceGenerator : IIncrementalGenerator
    {
        public const string Attribute = @"
namespace MyFirstGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    internal sealed class EqualsOperatorAttribute : System.Attribute { }
}";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
                ctx.AddSource("MyFirstGenerator.BasicAttribute.g.cs", SourceText.From(Attribute, Encoding.UTF8)));

            IncrementalValuesProvider<EqualsOperatorClassDetails?> classesToGenerate = context
                .SyntaxProvider
                .ForAttributeWithMetadataName(
                    "MyFirstGenerator.EqualsOperatorAttribute",
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .WithTrackingName("EqualsOperatorClassDetails");

            context.RegisterSourceOutput(classesToGenerate, static (spc, source) => Execute(source, spc));
        }

        private static void Execute(EqualsOperatorClassDetails? classDetails, SourceProductionContext ctx)
        {
            if (classDetails is { } value)
            {
                var sb = new StringBuilder();

                sb.Append($@"
namespace {classDetails.Value.Namespace}
{{
    partial class {classDetails.Value.Name}
    {{");
                sb.Append($@"
        public static bool operator==({classDetails.Value.Name} left, {classDetails.Value.Name} right) => left.Equals(right);
        public static bool operator!=({classDetails.Value.Name} left, {classDetails.Value.Name} right) => !left.Equals(right);");

                sb.Append(@"
    }
}");

                ctx.AddSource($"MyFirstGenerator.{value.Namespace}.{value.Name}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
            }
        }

        private static EqualsOperatorClassDetails? GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext ctx)
        {
            var classNode = ctx.TargetNode as ClassDeclarationSyntax;
            if (classNode == null)
                return null;

            // code très très simple qui suppose beaucoup (trop) de choses sur la structure du fichier...
            var nsNode = ctx.TargetNode.Parent as NamespaceDeclarationSyntax;
            if (nsNode == null)
                return null;

            return new EqualsOperatorClassDetails(nsNode.Name.ToString(), classNode.Identifier.ToString());
        }
    }

    public readonly record struct EqualsOperatorClassDetails
    {
        public readonly string Namespace;
        public readonly string Name;

        public EqualsOperatorClassDetails(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }
    }
}