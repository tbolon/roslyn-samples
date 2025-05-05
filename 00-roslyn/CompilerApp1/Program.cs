using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using static System.Console;

// ***********************
//
// Syntax Model
// https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
//

const string programText =
@"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(42);
        }
    }
}";

SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

// root node
WriteLine($"The tree is a {root.Kind()} node.");
WriteLine($"The tree has {root.Members.Count} elements in it.");
WriteLine($"The tree has {root.Usings.Count} using directives. They are:");
foreach (UsingDirectiveSyntax element in root.Usings)
    WriteLine($"\t{element.Name}");

// namespace
MemberDeclarationSyntax firstMember = root.Members[0];
WriteLine($"The first member is a {firstMember.Kind()}.");
var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

WriteLine($"There are {helloWorldDeclaration.Members.Count} members declared in this namespace.");
WriteLine($"The first member is a {helloWorldDeclaration.Members[0].Kind()}.");

// program class
var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
WriteLine($"There are {programDeclaration.Members.Count} members declared in the {programDeclaration.Identifier} class.");
WriteLine($"The first member is a {programDeclaration.Members[0].Kind()}.");

var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];

WriteLine($"The return type of the {mainDeclaration.Identifier} method is {mainDeclaration.ReturnType}.");
WriteLine($"The method has {mainDeclaration.ParameterList.Parameters.Count} parameters.");
foreach (ParameterSyntax item in mainDeclaration.ParameterList.Parameters)
    WriteLine($"The type of the {item.Identifier} parameter is {item.Type}.");
WriteLine($"The body text of the {mainDeclaration.Identifier} method follows:");
WriteLine(mainDeclaration.Body?.ToFullString());

// args parameter
var argsParameter = mainDeclaration.ParameterList.Parameters[0];

// ***********************
//
// IOperation Syntax Model (language agnostic)
// Useful in analyzers
//

// create a compilation object (necessary for the semantic model)
var compilation = CSharpCompilation
    .Create("HelloWorld")
    .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)) // mscorlib
    .AddSyntaxTrees(tree);

// get the semantic model
var model = compilation.GetSemanticModel(tree);

// main method IOperation
var methodBodyOp = (IMethodBodyOperation?)model.GetOperation(mainDeclaration);
var blockOp = methodBodyOp?.BlockBody;

// ***********************
//
// Semantic model
// https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/semantic-analysis
//

// find the "Hello World!" literal in the main method
LiteralExpressionSyntax helloWorldString = mainDeclaration.DescendantNodes().OfType<LiteralExpressionSyntax>().Single();
TypeInfo literalInfo = model.GetTypeInfo(helloWorldString);

// get the string type
var typeSymbol = (INamedTypeSymbol?)literalInfo.Type;

// get all members
var allMembers = typeSymbol?.GetMembers();

// get all methods
var methods = allMembers?.OfType<IMethodSymbol>();

// all methods which returns a string
var publicStringReturningMethods = methods?
    .Where(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, typeSymbol) &&
    m.DeclaredAccessibility == Accessibility.Public);

// remove overloads
var distinctMethods = publicStringReturningMethods?.Select(m => m.Name).Distinct();

WriteLine($"Methods from the type {typeSymbol?.Name} used as a parameter");
foreach (string name in distinctMethods ?? Enumerable.Empty<string>())
{
    WriteLine(name);
}