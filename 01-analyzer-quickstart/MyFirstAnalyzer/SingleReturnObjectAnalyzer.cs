using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MyFirstAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SingleReturnObjectAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MF03";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            title: @"Multiple clause return",
            messageFormat: @"Une seule clause return avec valeur est autorisée",
            @"Compiler",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: @"Vous devez supprimer les autres clauses return pour n'en garder qu'une seule.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterCodeBlockStartAction<SyntaxKind>(ctx =>
            {
                // 👇 on s'intéresse uniquement aux déclarations de méthodes
                if (ctx.OwningSymbol.Kind != SymbolKind.Method)
                {
                    return;
                }

                // 👇 uniquement celles se terminant par "Load"
                var methodSymbol = (IMethodSymbol)ctx.OwningSymbol;
                if (!methodSymbol.Name.StartsWith("Load", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                // 👇 uniquement dans des classes dont le nom se termine par "Loader"
                var classSymbol = methodSymbol.ContainingSymbol as INamedTypeSymbol;
                if (classSymbol == null || !classSymbol.Name.EndsWith("Loader", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                // 👇 statefull : on créé un analyseur spécifique pour cette méthode
                var analyzer = new MethodAnalyzer();
                ctx.RegisterSyntaxNodeAction(analyzer.AnalyzeReturnNode, SyntaxKind.ReturnStatement);
                ctx.RegisterCodeBlockEndAction(analyzer.CodeBlockEndAction);
            });
        }

        private sealed class MethodAnalyzer
        {
            private HashSet<SyntaxToken> _returnTokens;

            public bool HasReturnObject { get; set; }

            /// <summary>
            /// Analyse le noeud "ReturnStatementSyntax" pour vérifier le type de valeur renvoyée.
            /// </summary>
            public void AnalyzeReturnNode(SyntaxNodeAnalysisContext context)
            {
                ReturnStatementSyntax returnNode = (ReturnStatementSyntax)context.Node;

                if (returnNode.Expression.Kind() == SyntaxKind.NullLiteralExpression)
                {
                    return; // 👈 renvoi null OK
                }

                if (_returnTokens == null)
                {
                    _returnTokens = new HashSet<SyntaxToken>();
                }

                _returnTokens.Add(returnNode.GetFirstToken()); // 👈 Token "return"
            }

            /// <summary>
            /// Termine le traitement et ajoute les diagnostics.
            /// </summary>
            public void CodeBlockEndAction(CodeBlockAnalysisContext context)
            {
                // 👇 si pas de return mémorisé ou un seul OK
                if (_returnTokens == null || _returnTokens.Count <= 1)
                    return;

                // 👇 sinon on génère un diagnostic pour chaque token "return"
                foreach (var token in _returnTokens)
                {
                    Diagnostic diagnostic = Diagnostic.Create(Rule, token.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }

        }
    }
}
