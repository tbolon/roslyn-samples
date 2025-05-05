using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace MyFirstAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MF01";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            title: @"Type name contains lowercase letters",
            messageFormat: @"Type name '{0}' contains lowercase letters",
            @"Naming",
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: @"Type names should be all uppercase.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            // 👇 
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }
		
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = (INamedTypeSymbol)context.Symbol;
            if (symbol.Name == null) return;

            if (symbol.Name.ToCharArray().Any(char.IsLower))
            {
                var diagnostic = Diagnostic.Create(Rule, symbol.Locations[0], messageArgs: symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
