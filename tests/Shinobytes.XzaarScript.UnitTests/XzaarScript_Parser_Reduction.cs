/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.UnitTests
{
    [TestClass]
    public class XzaarScript_Parser_Reduction
    {
        [TestMethod]
        public void while_Expression()
        {
            var tree = Reduce("while(true && false != true) { a.test() }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("while (true && false != true) { a.test() }", tree.ToString());
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_4()
        {
            var tree = Reduce("switch(a+b) { case 99+apa: a.j() return case aa+apa: a.c() break }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("switch (a + b) { case 99 + apa: a.j() return  case aa + apa: a.c() break }", tree.ToString());
        }


        [TestMethod]
        public void Simple_if_expr_5()
        {
            var tree = Reduce("if ((true || (helloWorld || true)) && (fal2se || j > 0)) { }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("if (true || helloWorld || true && fal2se || j > 0) {} else {}", tree.ToString());
        }

        [TestMethod]
        public void Simple_if_expr_6()
        {
            var tree = Reduce("if (a || ((b && x) || c)) { }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("if (a || b && x || c) {} else {}", tree.ToString());
        }


        private SyntaxParser Transformer(string code)
        {
            return new SyntaxParser(new Lexer(code).Tokenize());
        }

        private AstNode Reduce(string code, out SyntaxParser parser)
        {
            parser = Transformer(code);
            return parser.Parse(); // new NodeReducer().Process(parser.Parse());
        }
    }
}