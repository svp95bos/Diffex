using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

using Diffex.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Diffex.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiffexIgnoreCodeFixProvider)), Shared]
public class DiffexIgnoreCodeFixProvider : CodeFixProvider
{
    private const string Title = "Remove DiffexIgnore attribute";

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiffexIgnoreAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var attributeSyntax = root.FindNode(diagnosticSpan).FirstAncestorOrSelf<AttributeSyntax>();

        context.RegisterCodeFix(
            Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                title: Title,
                createChangedDocument: c => RemoveAttributeAsync(context.Document, attributeSyntax, c),
                equivalenceKey: Title),
            diagnostic);
    }

    private async Task<Document> RemoveAttributeAsync(Document document, AttributeSyntax attributeSyntax, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root.RemoveNode(attributeSyntax, SyntaxRemoveOptions.KeepNoTrivia);
        return document.WithSyntaxRoot(newRoot);
    }
}
