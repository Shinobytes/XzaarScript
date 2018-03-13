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
 
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Parser;

namespace Shinobytes.XzaarScript.UnitTests
{
    [TestClass]
    public class XzaarScript_Parser_simple
    {
        [TestMethod]
        public void a_greater_than_b()
        {
            var transformer = Parser("a > b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a > b", ast.ToString());
        }

        [TestMethod]
        public void a_equals_b()
        {
            var transformer = Parser("a == b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a == b", ast.ToString());
        }

        [TestMethod]
        public void a_not_equals_b()
        {
            var transformer = Parser("a != b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a != b", ast.ToString());
        }


        [TestMethod]
        public void a_greater_than_equals_b()
        {
            var transformer = Parser("a >= b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a >= b", ast.ToString());
        }

        [TestMethod]
        public void a_less_than_b()
        {
            var transformer = Parser("a < b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a < b", ast.ToString());
        }

        [TestMethod]
        public void a_less_than_equals_b()
        {
            var transformer = Parser("a <= b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a <= b", ast.ToString());
        }


        [TestMethod]
        public void a_less_than_equals_b_or_a_equals_b()
        {
            var transformer = Parser("a <= b || a == b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a <= b || a == b", ast.ToString());
        }

        [TestMethod]
        public void a_less_than_equals_b_and_a_equals_b()
        {
            var transformer = Parser("a <= b && a == b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a <= b && a == b", ast.ToString());
        }

        [TestMethod]
        public void arithmetic_simple()
        {
            var transformer = Parser("1 + 2");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1 + 2", ast.ToString());
        }

        [TestMethod]
        public void arithmetic_simple_2()
        {
            var transformer = Parser("1 + 2 * 3");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1 + 2 * 3", ast.ToString());
        }

        [TestMethod]
        public void arithmetic_expression()
        {
            var transformer = Parser("1 + (2 * 3) / 2");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1 + 2 * 3 / 2", ast.ToString());
        }

        [TestMethod]
        public void Access_member_with_indexer()
        {
            var transformer = Parser("a[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_member_with_indexer_expression()
        {
            var transformer = Parser("a[(a.b)]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a[a.b]", ast.ToString());
        }

        [TestMethod]
        public void Access_member_with_indexer_expression_2()
        {
            var transformer = Parser("a[(a.b).Length]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a[a.b.Length]", ast.ToString());
        }


        [TestMethod]
        public void Access_member_with_indexer_expression_3()
        {
            var transformer = Parser("a[1 + 2]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a[1 + 2]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index()
        {
            var transformer = Parser("1[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1[0]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index()
        {
            var transformer = Parser("helloWorld[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("helloWorld[0]", ast.ToString());
        }

        [TestMethod]
        public void define_global_variable_assign_0()
        {
            var transformer = Parser("let i = 0");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void define_global_variable_assign_str()
        {
            var transformer = Parser("let i = 'hello'");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void define_global_variable_assign_str_2()
        {
            var transformer = Parser("let i = \"hello\"");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void loop_empty_body()
        {
            var transformer = Parser("loop { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void loop_break_one_line()
        {
            var transformer = Parser("loop break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void loop_break()
        {
            var transformer = Parser("loop { break; }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }



        [TestMethod]
        public void define_global_function_without_body_and_parameters()
        {
            var transformer = Parser("fn test() {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void define_global_function_without_body_with_parameters()
        {
            var transformer = Parser("fn test(i:i32) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void define_global_function_without_body_with_parameters_2()
        {
            var transformer = Parser("fn test(i:i32, b:string) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void define_global_function_without_body_with_parameters_and_explicit_return_type()
        {
            var transformer = Parser("fn test(i:i32) -> void {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void if_true_empty_body()
        {
            var transformer = Parser("if(true) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void if_true_one_liner()
        {
            var transformer = Parser("if(true) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void if_true_else_empty_body()
        {
            var transformer = Parser("if(true) {} else {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void if_true_else_if_empty_body()
        {
            var transformer = Parser("if(true) {} else if(true) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void for_no_expressions_with_empty_body()
        {
            var transformer = Parser("for(;;) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }
        [TestMethod]
        public void for_no_expressions_with_one_liner()
        {
            var transformer = Parser("for(;;) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void for_with_empty_body()
        {
            var transformer = Parser("for(let i = 0; i < 9999; i++) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void for_with_one_linery()
        {
            var transformer = Parser("for(let i = 0; i < 9999; i++) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void foreach_with_empty_body()
        {
            var transformer = Parser("foreach(let j in js) {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void foreach_with_one_liner()
        {
            var transformer = Parser("foreach(let j in js) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void foreach_in_expression_with_one_liner()
        {
            var transformer = Parser("foreach(let j in (a)) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void do_while_forever_empty_body()
        {
            var transformer = Parser("do {} while(true);");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void do_while_forever_one_liner()
        {
            var transformer = Parser("do break; while(true);");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void do_while_forever_one_liner_2()
        {
            var transformer = Parser("do if(true) break; while(true);");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void assign_member_access_direct()
        {
            var transformer = Parser("let j = a.b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void member_access_direct()
        {
            var transformer = Parser("a.b");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void member_access_direct_indexer()
        {
            var transformer = Parser("a[9].b[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void deep_member_access_direct()
        {
            var transformer = Parser("a.b.c.d.e.f.g.h.i.j.k.l.m.n.o.p.q.r.s.t");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void deep_member_access_direct_indexers()
        {
            var transformer = Parser("a.b[0].c.d.e.f[0].g[125].h[2*1].i.j.k.l[4-1].m.n[0].o.p.q.r[0].s.t");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void struct_definition_empty()
        {
            var transformer = Parser("struct a {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }
        [TestMethod]
        public void struct_definition_with_fields()
        {
            var transformer = Parser("struct a { test:i32; test2:i32; }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void struct_definition_with_fields_2()
        {
            var transformer = Parser("struct a { test:i32, test2:i32 }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void struct_definition_with_fields_3()
        {
            var transformer = Parser("struct a { i32 test, i32 test2 }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void struct_definition_with_fields_4()
        {
            var transformer = Parser("struct a { i32 test; i32 test2 }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void FunctionInvocation_direct()
        {
            var transformer = Parser("hello()");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("hello()", ast.ToString());
        }

        [TestMethod]
        public void Member_FunctionInvocation_direct()
        {
            var transformer = Parser("a.hello()");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a.hello()", ast.ToString());
        }

        [TestMethod]
        public void while_true_empty_body()
        {
            var transformer = Parser("while(true) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("while (true) {  }", ast.ToString());
        }

        [TestMethod]
        public void while_true_one_liner()
        {
            var transformer = Parser("while(true) break;");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("while (true) { break }", ast.ToString());
        }

        [TestMethod]
        public void while_true_call_member_test_function()
        {
            var transformer = Parser("while(true) { a.test() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("while (true) { a.test() }", ast.ToString());
        }

        [TestMethod]
        public void Member_FunctionInvocation_direct_2()
        {
            var transformer = Parser("a[0].hello()");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("a[0].hello()", ast.ToString());
        }


        [TestMethod]
        public void FunctionInvocation_direct_with_args()
        {
            var transformer = Parser("hello(a,b,c,d)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("hello(a, b, c, d)", ast.ToString());
        }

        [TestMethod]
        public void FunctionInvocation_direct_with_args_2()
        {
            var transformer = Parser("hello(a,(b+2),(1) + c, (2*3) / d)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("hello(a, b + 2, 1 + c, 2 * 3 / d)", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction()
        {
            var transformer = Parser("let a = () => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = () => {}", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction_with_param_implicittypes_simple()
        {
            var transformer = Parser("let a = x => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (x : any) => {}", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction_with_param_explicittypes_simple_error()
        {
            var transformer = Parser("let a = x:i32 => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(true, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            //Assert.AreEqual("var a : any = (x : any) => {}", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction_with_param_implicittypes()
        {
            var transformer = Parser("let a = (b) => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (b : any) => {}", ast.ToString());
        }


        [TestMethod]
        public void assign_arrowfunction_with_param_explicittypes()
        {
            var transformer = Parser("let a = (b:i32,c:str) => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (b : i32, c : str) => {}", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction_with_param_mixed_implicitexplicit_types_0()
        {
            var transformer = Parser("let a = (b, c, d:i32) => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (b : any, c : any, d : i32) => {}", ast.ToString());
        }


        [TestMethod]
        public void assign_arrowfunction_with_param_mixed_implicitexplicit_types_1()
        {
            var transformer = Parser("let a = (b, d:i32, c) => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (b : any, d : i32, c : any) => {}", ast.ToString());
        }

        [TestMethod]
        public void assign_arrowfunction_with_param_mixed_implicitexplicit_types_2()
        {
            var transformer = Parser("let a = (b, d:i32[], c) => {}");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (b : any, d : i32[], c : any) => {}", ast.ToString());
        }

        [TestMethod]
        public void invoke_arrowfunction_01()
        {
            var transformer = Parser("let a = () => {}; a();");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = () => {} a()", ast.ToString());
        }

        [TestMethod]
        public void invoke_arrowfunction_02()
        {
            var transformer = Parser("let a = (x) => {}; a(123);");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var a : any = (x : any) => {} a(123)", ast.ToString());
        }

        [TestMethod]
        public void assign_function_ref()
        {
            var transformer = Parser("fn test() { return 0 } let a = test; a(123);");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("fn test() { return 0 } var a : any = test a(123)", ast.ToString());
        }

        [TestMethod]
        public void reassign_function_with_lambda()
        {
            var transformer = Parser("fn test() { return 0 }; test = () => { };");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("fn test() { return 0 } test = () => {}", ast.ToString());
        }

        private SyntaxParser Parser(string code)
        {
            return new SyntaxParser(new Lexer(code).Tokenize());
        }
    }
}