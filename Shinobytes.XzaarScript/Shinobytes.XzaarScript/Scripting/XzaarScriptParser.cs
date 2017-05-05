using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarScriptParser
    {
        public EntryNode Parse(IReadOnlyList<XzaarSyntaxToken> tokens)
        {
            var current = 0;
            var ast = new EntryNode();
            XzaarNode node = null;

            // first step of the AST is to just get the recursion right / the depth tree.

            while ((node = WalkTokens(ref current, tokens)) != null)
            {
                ast.AddChild(node);
                if (current >= tokens.Count) break;
            }
            return ast;
        }


        private XzaarNode WalkTokens(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens)
        {
            var token = tokens[current];
            // Parse expression tree

            if (token.Type == "RPARAN") throw new XzaarScriptParserException("Unexpected ending paranthesis ')' found.");
            if (token.Type == "LPARAN") return WalkExpression(ref current, tokens);
            if (token.Type == "RBRACKET") throw new XzaarScriptParserException("Unexpected ending bracket ']' found.");
            if (token.Type == "LBRACKET") return WalkArrayIndex(ref current, tokens);
            if (token.Type == "RBRACE") throw new XzaarScriptParserException("Unexpected ending curly bracket '}' found.");
            if (token.Type == "LBRACE") return WalkBody(ref current, tokens);

            current++;
            if (token.Key == XzaarNodeTypes.LITERAL)
            {
                object outputValue = token.Value;
                if (token.Type == "NUMBER")
                {
                    // it will stop working if you convert the following line to a conditional op because the output type will always be a double if so.
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression, 
                    if (outputValue.ToString().Contains("."))
                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
                    else
                        outputValue = int.Parse(outputValue.ToString());
                }

                if (token.Type == "IF" || token.Type == "ELSE") return new ConditionNode(token.Type, -1);
                if (token.Type == "WHILE" || token.Type == "LOOP" || token.Type == "DO" || token.Type == "FOR" ||
                    token.Type == "FOREACH") return new LoopNode(token.Type, nodeIndex: -1);

                if (token.Type == "CASE") return new CaseNode(null, null, -1);
                if (token.Type == "GOTO") return new GotoNode(string.Empty, -1);
                if (token.Type == "BREAK") return new BreakNode(-1);
                if (token.Type == "CONTINUE") return new ContinueNode(-1);
                if (token.Type == "RETURN") return new ReturnNode(null, -1);
                if (token.Type == "SWITCH" || token.Type == "MATCH") return new MatchNode(null, null, -1);
                if (token.Type == "FUNCTION" || token.Type == "EXTERN" || token.Type == "STRUCT" ||
                    token.Type == "CLASS" || token.Type == "INTERFACE" || token.Type == "VARIABLE")
                    return new DefinitionNode(token.Type, -1);

                return new LiteralNode(token.Type, outputValue, -1);
            }
            return new AnyNode(
                // GetNodeType(token.Key),
                token.Key,
                token.Type,
                token.Value, -1);
        }

        private XzaarNode WalkExpression(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens)
        {
            var token = AssertNextExpressionToken(ref current, tokens, ')');
            return WalkExpressionContent(ref current, tokens, token, new ExpressionNode(-1), "RPARAN", ')');
        }

        private XzaarNode WalkArrayIndex(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens)
        {
            var token = AssertNextExpressionToken(ref current, tokens, ']');
            return WalkExpressionContent(ref current, tokens, token, new ArrayIndexNode(-1), "RBRACKET", ']');
        }

        private XzaarNode WalkBody(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens)
        {
            var token = AssertNextExpressionToken(ref current, tokens, '}');
            return WalkExpressionContent(ref current, tokens, token, new BodyNode(-1), "RBRACE", '}');
        }

        private static XzaarSyntaxToken AssertNextExpressionToken(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, char endingToken)
        {
            XzaarSyntaxToken token;
            if (current + 1 >= tokens.Count)
                throw new XzaarScriptParserException("Unexpected end of script. No matching ending '" + endingToken + "' was found.");

            token = tokens[++current];
            return token;
        }

        private XzaarNode WalkExpressionContent(
            ref int current,
            IReadOnlyList<XzaarSyntaxToken> tokens,
            XzaarSyntaxToken token,
            XzaarNode node,
            string endingTokenType,
            char endingToken)
        {
            while (token != null && token.Type != endingTokenType)
            {
                node.AddChild(WalkTokens(ref current, tokens));
                if (current >= tokens.Count)
                    throw new XzaarScriptParserException("No matching ending '" + endingToken + "' was found.");

                token = tokens[current];
            }
            current++;
            return node;
        }
    }
}