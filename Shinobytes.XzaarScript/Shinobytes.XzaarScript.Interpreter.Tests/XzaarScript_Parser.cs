using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Parser
    {
        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes()
        {
            var parser = Parser("fn test() { test() }");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.Expression, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Scope, basicAst.Children[3].Type);
        }

        
         [TestMethod]
        public void Parse_identifiers_and_numbers_1()
        {
            var parser = Parser("let j = 0.Length");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
            Assert.AreEqual(XzaarSyntaxKind.MemberAccess, basicAst.Children[4].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[5].Type);
        }

        [TestMethod]
        public void Parse_identifiers_and_numbers()
        {
            var parser = Parser("let j = 9999");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }

        [TestMethod]
        public void Parse_identifiers_and_strings()
        {
            var parser = Parser("let j = \"9999\"");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }

        [TestMethod]
        public void Parse_identifiers_and_hex()
        {
            var parser = Parser("let j = 0xff");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }


        [TestMethod]
        public void Parse_identifiers_strings_and_comments_1()
        {
            var parser = Parser("//hello world\nlet j = \"9999\"");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }


        [TestMethod]
        public void Parse_identifiers_strings_and_comments_2()
        {
            var parser = Parser("/*hello world*/let j = \"9999\"");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }


        [TestMethod]
        public void Parse_identifiers_strings_and_comments_3()
        {
            var parser = Parser("/*hello world*/let j =/*hello world*/ \"9999\"");
            var basicAst = parser.Parse();
            Assert.AreEqual(true, basicAst.HasChildren);

            Assert.AreEqual(XzaarSyntaxKind.Keyword, basicAst.Children[0].Type);
            Assert.AreEqual(XzaarSyntaxKind.Identifier, basicAst.Children[1].Type);
            Assert.AreEqual(XzaarSyntaxKind.AssignmentOperator, basicAst.Children[2].Type);
            Assert.AreEqual(XzaarSyntaxKind.Literal, basicAst.Children[3].Type);
        }

        private XzaarSyntaxParser Parser(string code)
        {
            return new XzaarSyntaxParser(new XzaarScriptLexer(code).Tokenize());
        }
    }
}