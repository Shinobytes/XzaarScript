using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Parser_Reduction
    {
        [TestMethod]
        public void while_Expression()
        {
            LanguageParser parser;
            var tree = Reduce("while(true && false != true) { a.test() }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("while (true && false != true) { a.test() }", tree.ToString());
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_4()
        {
            LanguageParser parser;
            var tree = Reduce("switch(a+b) { case 99+apa: a.j() return case aa+apa: a.c() break }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("switch (a + b) { case 99 + apa: a.j() return  case aa + apa: a.c() break }", tree.ToString());
        }


        [TestMethod]
        public void Simple_if_expr_5()
        {
            LanguageParser parser;
            var tree = Reduce("if ((true || (helloWorld || true)) && (fal2se || j > 0)) { }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("if (true || helloWorld || true && fal2se || j > 0) {} else {}", tree.ToString());
        }

        [TestMethod]
        public void Simple_if_expr_6()
        {
            LanguageParser parser;
            var tree = Reduce("if (a || ((b && x) || c)) { }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("if (a || b && x || c) {} else {}", tree.ToString());
        }


        private LanguageParser Transformer(string code)
        {
            return new LanguageParser(new Lexer(code).Tokenize());
        }

        private AstNode Reduce(string code, out LanguageParser parser)
        {
            parser = Transformer(code);
            return new NodeReducer().Process(parser.Parse());
        }
    }
}