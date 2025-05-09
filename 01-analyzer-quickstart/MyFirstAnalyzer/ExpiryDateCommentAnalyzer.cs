using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyFirstAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExpiryDateCommentAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MF02";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            title: @"Date de péremption dépassée",
            messageFormat: @"La date d'expiration de ce code est dépassée depuis le {0}",
            @"Compiler",
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: @"Ce code devrait être supprimé, sa date d'expiration est dépassée.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            // 👇 on se branche sur des noeuds de type "Déclaration de méthode"
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var x = context.Node as MethodDeclarationSyntax;

            // trivia au début ?
            if (!x.HasLeadingTrivia)
            {
                return;
            }

            // single-line comment ?
            var comment = x.GetLeadingTrivia().LastOrDefault(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia));
            if (comment == null)
            {
                return;
            }

            // préfixe reconnu ?
            var text = comment.ToFullString();
            if (!text.StartsWith("// obsolete:"))
            {
                return;
            }


            // format valide ?
            var m = Regex.Match(text, @"// obsolete:\s*((\d{2})/(\d{2})/(\d{4}))");
            if (!m.Success || !DateTime.TryParseExact(m.Groups[1].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiry))
            {
                return;
            }

            // date d'expiration dépassée ?
            if (expiry >= DateTime.Today)
            {
                return;
            }

            // emplacement de la méthode
            var identifier = x.ChildTokens().FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));
            var loc = identifier.GetLocation() ?? x.GetLocation();

            // erreur de diagnostic
            var diagnostic = Diagnostic.Create(Rule, loc, messageArgs: expiry.ToString("dd/MM/yyyy"));
            context.ReportDiagnostic(diagnostic);
        }
    }
}
