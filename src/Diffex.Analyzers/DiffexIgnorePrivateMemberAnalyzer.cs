using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Diffex.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DiffexIgnorePrivateMemberAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "DIFFEX001";
    private static readonly LocalizableString Title = "DiffexIgnore attribute applied to private member";
    private static readonly LocalizableString MessageFormat = "The [DiffexIgnore] attribute should not be applied to private member '{0}'";
    private static readonly LocalizableString Description = "The [DiffexIgnore] attribute should only be applied to public or protected members.";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.PropertyDeclaration, SyntaxKind.FieldDeclaration);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var memberDeclaration = (MemberDeclarationSyntax)context.Node;
        var attributes = memberDeclaration.AttributeLists.SelectMany(a => a.Attributes);

        foreach (var attribute in attributes)
        {
            var symbolInfo = context.SemanticModel.GetSymbolInfo(attribute);
            if (symbolInfo.Symbol is IMethodSymbol methodSymbol &&
                methodSymbol.ContainingType.ToString() == "DiffexIgnoreAttribute")
            {
                var accessibility = context.SemanticModel.GetDeclaredSymbol(memberDeclaration).DeclaredAccessibility;
                if (accessibility == Accessibility.Private)
                {
                    var diagnostic = Diagnostic.Create(Rule, memberDeclaration.GetLocation(), memberDeclaration);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
