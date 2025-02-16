using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Diffex.Abstractions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Diffex.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiffexAnalyzersCodeFixProvider)), Shared]
    public class DiffexAnalyzersCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiffexIgnorePrivateMemberAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the member declaration identified by the diagnostic.
            var memberDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => RemoveAttributeAsync(context.Document, memberDeclaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> RemoveAttributeAsync(Document document, MemberDeclarationSyntax memberDeclaration, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            // Find the attribute to remove.
            var newMemberDeclaration = memberDeclaration.RemoveNodes(
                memberDeclaration.AttributeLists.SelectMany(a => a.Attributes)
                    .Where(a => a.Name.ToString() == nameof(DiffexIgnoreAttribute)),
                SyntaxRemoveOptions.KeepNoTrivia);

            // Replace the old member declaration with the new one.
            var newRoot = root.ReplaceNode(memberDeclaration, newMemberDeclaration);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
