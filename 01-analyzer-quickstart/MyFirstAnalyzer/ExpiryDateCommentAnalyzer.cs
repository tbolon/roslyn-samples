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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <expiration date="2024-25-10">This method is obsolete and will be removed in future versions.</expiration>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeSymbol(SymbolStartAnalysisContext context)
        {
            var method = context.Symbol as IMethodSymbol;
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var x = context.Node as MethodDeclarationSyntax;
            if (x.HasLeadingTrivia)
            {
                var comment = (SyntaxTrivia)x.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia));
                var text = comment.ToFullString();
                if (text.StartsWith("// obsolete:"))
                {
                    var m = Regex.Match(text, @"//\s*obsolete:\s*((\d{2})/(\d{2})/(\d{4}))");
                    if (m.Success && DateTime.TryParseExact(m.Groups[1].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiry))

                    {
                        if (expiry > DateTime.Today)
                        {
                            var identifier = x.ChildTokens().FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));
                            var loc = identifier.GetLocation() ?? x.GetLocation();
                            var diagnostic = Diagnostic.Create(Rule, loc, messageArgs: expiry.ToString("dd/MM/yyyy"));
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
        }
    }
}
