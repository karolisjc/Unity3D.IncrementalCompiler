﻿using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shaman.Roslyn.LinqRewrite.DataStructures;

namespace Shaman.Roslyn.LinqRewrite.RewriteRules
{
    public static class RewriteForEach
    {
        public static ExpressionSyntax Rewrite(RewriteParameters p)
            => p.Rewrite.RewriteAsLoop(
                p.Code.CreatePrimitiveType(SyntaxKind.VoidKeyword),
                Enumerable.Empty<StatementSyntax>(),
                Enumerable.Empty<StatementSyntax>(),
                p.Collection,
                p.Chain,
                (inv, arguments, param) =>
                {
                    var lambda = inv.Lambda ?? new Lambda((AnonymousFunctionExpressionSyntax) inv.Arguments.First());

                    var statement = p.Code.InlineOrCreateMethod(lambda,
                        p.Code.CreatePrimitiveType(SyntaxKind.VoidKeyword), param, isVoid: true);

                    if (statement.Item1.Count > 0)
                    {
                        return SyntaxFactory.Block(statement.Item1);
                    }
                    return SyntaxFactory.ExpressionStatement(statement.Item2);
                }
            );
    }
}
