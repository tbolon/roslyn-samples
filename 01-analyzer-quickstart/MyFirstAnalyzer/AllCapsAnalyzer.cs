using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace MyFirstAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AllCapsAnalyzer : DiagnosticAnalyzer
    {
        // 👇 identifiant unique (utile pour configurer la règle)
        public const string DiagnosticId = "MF01";

        // 👇 déclaration de la "règle" qui sera rapportée
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            title: @"Type name contains lowercase letters",
            messageFormat: @"Type name '{0}' contains lowercase letters",
            @"Naming",
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: @"Type names should be all uppercase.");

        // 👇 liste des règles rapportées par cet analyseur (1 seule ici)
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            // 👇 optimisations
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            // 👇 on se branche sur l'analyse des symboles "Type nommé" (classes, variables, etc.)
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
