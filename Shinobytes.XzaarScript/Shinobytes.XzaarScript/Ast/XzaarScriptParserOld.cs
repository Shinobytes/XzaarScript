//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using Shinobytes.XzaarScript.Ast.Nodes;

//namespace Shinobytes.XzaarScript.Ast
//{
//    public class XzaarScriptParserOld
//    {
//        public EntryNode Parse(IReadOnlyList<XzaarSyntaxToken> tokens)
//        {
//            var current = 0;
//            var ast = new EntryNode(null);
//            XzaarAstNode node = null;

//            // first step of the AST is to just get the recursion right / the depth tree.
//            int nodeIndex = 0;
//            while ((node = WalkTokens(ref current, tokens, ref nodeIndex)) != null)
//            {
//                if (node.NodeType != XzaarAstNodeTypes.COMMENT)
//                    ast.AddChild(node);
//                if (current >= tokens.Count) break;
//            }
//            return ast;
//        }


//        private XzaarAstNode WalkTokens(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, ref int nodeIndex)
//        {
//            var token = tokens[current];
//            // Parse expression tree

//            if (token.Type == "RPARAN") throw new XzaarScriptParserException("Unexpected ending paranthesis ')' found at line: " + token.SourceLine + ".");
//            if (token.Type == "LPARAN") return WalkExpression(ref current, tokens, ref nodeIndex);
//            if (token.Type == "RBRACKET") throw new XzaarScriptParserException("Unexpected ending bracket ']' found at line: " + token.SourceLine + ".");
//            if (token.Type == "LBRACKET") return WalkArrayIndex(ref current, tokens, ref nodeIndex);
//            if (token.Type == "RBRACE") throw new XzaarScriptParserException("Unexpected ending curly bracket '}' found at line: " + token.SourceLine + ".");
//            if (token.Type == "LBRACE") return WalkBody(ref current, tokens, ref nodeIndex);

//            current++;
//            if (token.Key == XzaarAstNodeTypes.LITERAL)
//            {
//                object outputValue = token.Value;
//                if (token.Type == "NUMBER")
//                {
//                    // it will stop working if you convert the following line to a conditional op because the output type will always be a double if so.
//                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression, 
//                    if (outputValue.ToString().Contains("."))
//                    {
//                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
//                    }
//                    else if (outputValue.ToString().ToLower().StartsWith("0x"))
//                    {
//                        outputValue = Convert.ToInt32(outputValue.ToString().Substring(2), 16);
//                    }
//                    else
//                    {
//                        outputValue = int.Parse(outputValue.ToString());
//                    }
//                }

//                if (token.Type == "IF" || token.Type == "ELSE") return new ConditionNode(token.Type, nodeIndex++);
//                if (token.Type == "WHILE" || token.Type == "LOOP" || token.Type == "DO" || token.Type == "FOR" ||
//                    token.Type == "FOREACH") return new LoopNode(token.Type, null, nodeIndex++);

//                if (token.Type == "CASE") return new CaseNode(null, null, nodeIndex++);
//                if (token.Type == "GOTO") return new GotoNode(string.Empty, nodeIndex++);
//                if (token.Type == "BREAK") return new BreakNode(nodeIndex++);
//                if (token.Type == "CONTINUE") return new ContinueNode(nodeIndex++);
//                if (token.Type == "RETURN") return new ReturnNode(null, nodeIndex++);
//                if (token.Type == "SWITCH" || token.Type == "MATCH") return new MatchNode(null, null, nodeIndex++);
//                if (token.Type == "FUNCTION" || token.Type == "EXTERN" || token.Type == "STRUCT" ||
//                    token.Type == "CLASS" || token.Type == "INTERFACE" || token.Type == "VARIABLE")
//                    return new DefinitionNode(token.Type, nodeIndex++);

//                return new LiteralNode(token.Type, outputValue, nodeIndex++);
//            }
//            return new AnyNode(
//                // GetNodeType(token.Key),
//                token.Key,
//                token.Type,
//                token.Value, 
//                nodeIndex++);
//        }

//        private XzaarAstNode WalkExpression(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, ref int nodeIndex)
//        {
//            var token = AssertNextExpressionToken(ref current, tokens, ')');
//            return WalkExpressionContent(ref current, tokens, token, new ExpressionNode(nodeIndex++), "RPARAN", ')', ref nodeIndex);
//        }

//        private XzaarAstNode WalkArrayIndex(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, ref int nodeIndex)
//        {
//            var token = AssertNextExpressionToken(ref current, tokens, ']');
//            return WalkExpressionContent(ref current, tokens, token, new ArrayIndexNode(nodeIndex++), "RBRACKET", ']', ref nodeIndex);
//        }

//        private XzaarAstNode WalkBody(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, ref int nodeIndex)
//        {
//            var token = AssertNextExpressionToken(ref current, tokens, '}');
//            return WalkExpressionContent(ref current, tokens, token, new BodyNode(nodeIndex++), "RBRACE", '}', ref nodeIndex);
//        }

//        private static XzaarSyntaxToken AssertNextExpressionToken(ref int current, IReadOnlyList<XzaarSyntaxToken> tokens, char endingToken)
//        {
//            XzaarSyntaxToken token;
//            if (current + 1 >= tokens.Count)
//                throw new XzaarScriptParserException("Unexpected end of script. No matching ending '" + endingToken + "' was found.");

//            token = tokens[++current];
//            return token;
//        }

//        private XzaarAstNode WalkExpressionContent(
//            ref int current,
//            IReadOnlyList<XzaarSyntaxToken> tokens,
//            XzaarSyntaxToken token,
//            XzaarAstNode node,
//            string endingTokenType,
//            char endingToken, ref int nodeIndex)
//        {
//            while (token != null && token.Type != endingTokenType)
//            {
//                var n = WalkTokens(ref current, tokens, ref nodeIndex);
//                if (n.NodeType != XzaarAstNodeTypes.COMMENT)
//                    node.AddChild(n);
//                if (current >= tokens.Count)
//                    throw new XzaarScriptParserException("No matching ending '" + endingToken + "' was found.");

//                token = tokens[current];
//            }
//            current++;
//            return node;
//        }
//    }
//}