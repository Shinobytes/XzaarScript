/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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