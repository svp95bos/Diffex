using System.Collections.Immutable;

using Diffex.Abstractions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Diffex.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DiffexIgnoreAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "DIFFEX001";
    private static readonly LocalizableString Title = "DiffexIgnore attribute applied on private member";
    private static readonly LocalizableString MessageFormat = "Unnecessary use of DiffexIgnore attribute. Private members are always ignored.";
    private static readonly LocalizableString Description = "Detects the DiffexIgnore attribute applied to a property or field.";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, Microsoft.CodeAnalysis.CSharp.SyntaxKind.Attribute);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var attributeSyntax = (Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax)context.Node;
        var symbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;

        if (symbol?.ContainingType?.Name == nameof(DiffexIgnoreAttribute))
        {
            var diagnostic = Diagnostic.Create(Rule, attributeSyntax.GetLocation(), attributeSyntax.Parent?.Parent?.ToString());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
