//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Shinobytes.XzaarScript.Ast;
//using Shinobytes.XzaarScript.Ast.Nodes;
//using Shinobytes.XzaarScript.Ast.NodesOld;

//namespace Shinobytes.XzaarScript.Interpreter.Tests
//{
//    [TestClass]
//    public class XzaarScript_Lexer_Tests
//    {
//        [TestMethod]
//        public void LexerTest_0()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("1 >   0 test     \n" +
//                                        "       apa");
//            Assert.AreEqual("1", tokens[0].Value);
//            Assert.AreEqual(">", tokens[1].Value);
//            Assert.AreEqual("0", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_hexadecimal_constant_value()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("let j = 0xff");
//            Assert.AreEqual("let", tokens[0].Value);
//            Assert.AreEqual("j", tokens[1].Value);
//            Assert.AreEqual("=", tokens[2].Value);
//            Assert.AreEqual("0xff", tokens[3].Value);
//            Assert.AreEqual(4, tokens.Count);
//        }


//        [TestMethod]
//        public void LexerTest_if_then()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("if (a == 0) { a = 3 }");
//            Assert.AreEqual("if", tokens[0].Value);
//            Assert.AreEqual("(", tokens[1].Value);
//            Assert.AreEqual("a", tokens[2].Value);
//            Assert.AreEqual("==", tokens[3].Value);
//            Assert.AreEqual("0", tokens[4].Value);
//            Assert.AreEqual(")", tokens[5].Value);
//            Assert.AreEqual("{", tokens[6].Value);
//            Assert.AreEqual("a", tokens[7].Value);
//            Assert.AreEqual("=", tokens[8].Value);
//            Assert.AreEqual("3", tokens[9].Value);
//            Assert.AreEqual("}", tokens[10].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_decimal()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a = 10.0");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("10.0", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_number_and_accessor()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a = 10.jaa");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("10", tokens[2].Value);
//            Assert.AreEqual(".", tokens[3].Value);
//            Assert.AreEqual("jaa", tokens[4].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_decimal_number_and_accessor_0()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a = 10.0.jaa");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("10.0", tokens[2].Value);
//            Assert.AreEqual(".", tokens[3].Value);
//            Assert.AreEqual("jaa", tokens[4].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_decimal_number_and_accessor_1()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a = 10.0.ToString()");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("10.0", tokens[2].Value);
//            Assert.AreEqual(".", tokens[3].Value);
//            Assert.AreEqual("ToString", tokens[4].Value);
//            Assert.AreEqual("(", tokens[5].Value);
//            Assert.AreEqual(")", tokens[6].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_string_0()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = \"world ma mate:)\"");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("world ma mate:)", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_equals_string_1()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = \"world\"");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("world", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_quoute_inside_string()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = \"w'o'rld\"");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("w'o'rld", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_string_inside_quoute()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = 'w\"o\"rld'");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("w\"o\"rld", tokens[2].Value);
//        }


//        [TestMethod]
//        public void LexerTest_string_inside_string()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = \"w\\\"o\\\"rld\"");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("w\"o\"rld", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_quoute_inside_quoute()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("hello = 'w\\'o\\'rld'");
//            Assert.AreEqual("hello", tokens[0].Value);
//            Assert.AreEqual("=", tokens[1].Value);
//            Assert.AreEqual("w'o'rld", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_lt_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a < b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("<", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_lshift_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a << b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("<<", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }


//        [TestMethod]
//        public void LexerTest_a_rshift_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a >> b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual(">>", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }


//        [TestMethod]
//        public void LexerTest_minus1_lt_0()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("-1 < 0");
//            Assert.AreEqual("-1", tokens[0].Value);
//            Assert.AreEqual("<", tokens[1].Value);
//            Assert.AreEqual("0", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_1_gt_0()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("1 > 0");
//            Assert.AreEqual("1", tokens[0].Value);
//            Assert.AreEqual(">", tokens[1].Value);
//            Assert.AreEqual("0", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_lte_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a <= b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("<=", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_eq_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a == b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("==", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_neq_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a != b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("!=", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_gte_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a >= b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual(">=", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_gt_b()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a > b");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual(">", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);
//        }

//        [TestMethod]
//        public void LexerTest_a_plusplus()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a++");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("++", tokens[1].Value);
//        }


//        [TestMethod]
//        public void LexerTest_a_minusminus()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokens = lexer.Tokenize("a--");
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual("--", tokens[1].Value);
//        }


//        [TestMethod]
//        public void LexerTest_complex()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var tokenString = "a > b && c * j < 4 || apa == 5 && ( x - y == z || z * y > x )";
//            var tokens = lexer.Tokenize(tokenString);
//            Assert.AreEqual(27, tokens.Count);
//            Assert.AreEqual("a", tokens[0].Value);
//            Assert.AreEqual(">", tokens[1].Value);
//            Assert.AreEqual("b", tokens[2].Value);

//            var values = tokenString.Split(' ');
//            for (var i = 0; i < values.Length; i++)
//            {
//                var expected = values[i];
//                var actual = tokens[i].Value;
//                Assert.AreEqual(expected, actual, $"Expected: {expected}. Actual: {actual}. At: {i}");
//            }
//        }

//        [TestMethod]
//        public void ParserTest_if_then()
//        {

//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var tokens = lexer.Tokenize("if (a == 0) { a = 3 }");
//            var ast = parser.Parse(tokens);

//            Assert.AreEqual("if", tokens[0].Value);
//            Assert.AreEqual("(", tokens[1].Value);
//            Assert.AreEqual("a", tokens[2].Value);
//            Assert.AreEqual("==", tokens[3].Value);
//            Assert.AreEqual("0", tokens[4].Value);
//            Assert.AreEqual(")", tokens[5].Value);
//            Assert.AreEqual("{", tokens[6].Value);
//            Assert.AreEqual("a", tokens[7].Value);
//            Assert.AreEqual("=", tokens[8].Value);
//            Assert.AreEqual("3", tokens[9].Value);
//            Assert.AreEqual("}", tokens[10].Value);
//        }


//        [TestMethod]
//        public void ParserTest_a_lt_b_mul_c()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var tokens = lexer.Tokenize("a < (b * c)");

//            Assert.IsTrue(ValidateTokens(tokens, "a", "<", "(", "b", "*", "c", ")"),
//                $"Failed to validate tokens! Expected: 'a < (b * c)' but got '{string.Join(" ", tokens.Select(s => s.Value))}'");

//            var ast = parser.Parse(tokens);

//            Assert.AreEqual(3, ast.Children.Count);
//            Assert.AreEqual(XzaarAstNodeTypes.PROGRAM, ast.NodeType);
//            Assert.AreEqual(XzaarAstNodeTypes.LITERAL, ast[0].NodeType);
//            Assert.AreEqual(XzaarAstNodeTypes.EQUALITY_OPERATOR, ast[1].NodeType);
//            if (ast[2].NodeType != XzaarAstNodeTypes.BODY && ast[2].NodeType != XzaarAstNodeTypes.EXPRESSION)
//                Assert.Fail("Expected the last node type to be of type body or expression but was " + ast[2].NodeType);
//            // Assert.AreEqual(XzaarNodeTypes.BODY, ast[2].NodeType);
//        }

//        [TestMethod]
//        public void TransformTest_a_lt_b_mul_c()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var tokens = lexer.Tokenize("a < (b * c)");

//            Assert.IsTrue(ValidateTokens(tokens, "a", "<", "(", "b", "*", "c", ")"),
//                $"Failed to validate tokens! Expected: 'a < (b * c)' but got '{string.Join(" ", tokens.Select(s => s.Value))}'");

//            var ast = parser.Parse(tokens);

//            ast = transformer.Transform(ast);

//            Assert.AreEqual(1, ast.Children.Count);
//            Assert.AreEqual(XzaarAstNodeTypes.PROGRAM, ast.NodeType);
//            Assert.AreEqual(XzaarAstNodeTypes.EQUALITY, ast[0].NodeType);
//        }

//        private bool ValidateTokens(IReadOnlyList<XzaarSyntaxToken> tokens, params string[] tokenValues)
//        {
//            if (tokens.Count != tokenValues.Length) return false;
//            for (var i = 0; i < tokens.Count; i++)
//            {
//                if (tokens[i].Value != tokenValues[i]) return false;
//            }
//            return true;
//        }
//    }
//}
