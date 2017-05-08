using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Parser;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Parser
    {
        [TestMethod]
        public void Empty_struct()
        {
            var transformer = Parser("struct test { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Struct_tsstyle_fields_1()
        {
            var transformer = Parser("struct test { hello : string; t2 : number; }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Struct_tsstyle_fields_2()
        {
            var transformer = Parser("struct test { hello : string t2 : number }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Struct_csstyle_fields_1()
        {
            var transformer = Parser("struct test { string hello; number t2; }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Struct_csstyle_fields_2()
        {
            var transformer = Parser("struct test { string hello number t2 }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Walk_post_increment()
        {
            var transformer = Parser("i++");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Walk_pre_increment()
        {
            var transformer = Parser("++i");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("++i", ast.ToString());
        }

        [TestMethod]
        public void Walk_pre_increment_decrement_1()
        {
            var transformer = Parser("++i; --i");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("++i --i", ast.ToString());
        }

        [TestMethod]
        public void Walk_pre_increment_decrement_no_separation_gives_error_1()
        {
            var transformer = Parser("++i --i");
            var ast = transformer.Parse();
            var message = string.Join(Environment.NewLine, transformer.Errors);
            Assert.AreEqual(true, transformer.HasErrors, message);
            Assert.AreEqual("[Error] Invalid unary expression found. The operand of an increment or decrement operator must be a variable, property or indexer. Did you forget to separate the expression using a semicolon? \r\nExample: \'++i--\' could be \'++i; --i\'. At line 1", message);
        }
        [TestMethod]
        public void Walk_pre_increment_decrement_no_separation_gives_error_2()
        {
            var transformer = Parser("++a --b");
            var ast = transformer.Parse();
            var message = string.Join(Environment.NewLine, transformer.Errors);
            Assert.AreEqual(true, transformer.HasErrors, message);
            Assert.AreEqual("[Error] Invalid unary expression found. The operand of an increment or decrement operator must be a variable, property or indexer. Did you forget to separate the expression using a semicolon? \r\nExample: \'++a--\' could be \'++a; --b\'. At line 1", message);
        }
        //[TestMethod]
        //public void Console_log_constant_expr_3()
        //{
        //    var transformer = Parser("struct j { s:number} fn test(any console) { let f =j f.s=99 console.log(f.s) }");
        //    var ast = transformer.Transform();
        //    Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        //    Assert.AreEqual("++i --i", ast.ToString());
        //}


        [TestMethod]
        public void Declare_Variable_With_Type()
        {
            var transformer = Parser("let j : number = 0");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var j : number = 0", ast.ToString());
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
        public void Access_literal_with_element_index_2()
        {
            var transformer = Parser("\"hello world\"[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("\"hello world\"[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_3()
        {
            var transformer = Parser("1[0].test()");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1[0].test()", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_4()
        {
            var transformer = Parser("1[0][1].test()");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1[0][1].test()", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_5()
        {
            var transformer = Parser("1[0][1].test[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("1[0][1].test[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_6()
        {
            var transformer = Parser("let j = 1[0][1] + test[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var j : number = 1[0][1] + test[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_7()
        {
            var transformer = Parser("let b = 1[0][1].ohoh() + test[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var b : any = 1[0][1].ohoh() + test[0]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_1()
        {
            var transformer = Parser("helloWorld[0][1]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("helloWorld[0][1]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_3()
        {
            var transformer = Parser("\"hello\"[0][0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("\"hello\"[0][0]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_2()
        {
            var transformer = Parser("helloWorld[0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("helloWorld[0]", ast.ToString());
        }

        [TestMethod]
        public void Foreach_invoke_function_on_item()
        {
            var transformer = Parser("foreach(let a in b) { a.j() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("foreach (var a in b) { a.j() }", ast.ToString());
        }

        [TestMethod]
        public void For_invoke_function_on_item()
        {

            var transformer = Parser("for(let i = 0; i < k.Length; i++) { let a = x[i] a.j() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("for (var i : number = 0; i < k.Length; i++) { var a : any = x[i] a.j() }", ast.ToString());
        }

        [TestMethod]
        public void Loop_invoke_function_on_item()
        {
            var transformer = Parser("loop { a.j() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("loop { a.j() }", ast.ToString());
        }

        [TestMethod]
        public void Do_while_true()
        {
            var transformer = Parser("do { a.test() } while(true)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("do { a.test() } while (true)", ast.ToString());
        }

        [TestMethod]
        public void Do_while_Expression()
        {
            var transformer = Parser("do { a.test() } while(true && false != true)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("do { a.test() } while (true && false != true)", ast.ToString());
        }


        [TestMethod]
        public void while_true()
        {
            var transformer = Parser("while(true) { a.test() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("while (true) { a.test() }", ast.ToString());
        }

        [TestMethod]
        public void while_Expression()
        {
            var transformer = Parser("while(true && false != true) { a.test() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("while (true && false != true) { a.test() }", ast.ToString());
        }


        [TestMethod]
        public void Empty_switch()
        {
            var transformer = Parser("switch(a+b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Switch_simple_one_case()
        {
            var transformer = Parser("switch(a) { case 0: break; }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_1()
        {
            var transformer = Parser("switch(a+b) { case 99+apa: a.j() break }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Switch_invoke_function_on_item_2()
        {
            var transformer = Parser("switch(a+b) { case 99+apa: a.j() return }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_3()
        {
            var transformer = Parser("switch(a+b) { case 99+apa: a.j() break case aa+apa: a.c() return }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_4()
        {
            var transformer = Parser("switch(a+b) { case 99+apa: a.j() return case aa+apa: a.c() break }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Assign_global_variable_1()
        {
            var transformer = Parser("let c = 0");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Assign_global_variable_2()
        {
            var transformer = Parser("let c = 0 + 51 * 2");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Assign_global_variable_3()
        {
            var transformer = Parser("let c = 0 + 51 * 2 > 0");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Assign_global_variable_4()
        {
            var transformer = Parser("let c = 0 + 51 * 2 > 0 || j < 0 && i");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Assign_global_variable_5()
        {
            var transformer = Parser("let c = 0 + 51 * 2 > 0 || j < 0 && i let x = 0");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }



        [TestMethod]
        public void Assign_global_variable_7()
        {
            var transformer = Parser("let c = [124]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : array = [124]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_8()
        {
            var transformer = Parser("let c = [124,9295]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : array = [124,9295]", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_9()
        {
            var transformer = Parser("let c = [124,\"asdasd\",9295]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : array = [124,\"asdasd\",9295]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_10()
        {
            var transformer = Parser("let c:any = []");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : any = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_6()
        {
            var transformer = Parser("let c = []");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : array = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_11()
        {
            var transformer = Parser("c = []");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("c = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_12()
        {
            var transformer = Parser("c = [\"\"].Length");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("c = [\"\"].Length", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_13()
        {
            var transformer = Parser("c = [\"\"].Length");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("c = [\"\"].Length", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_14()
        {
            var transformer = Parser("test([\"\"].Length)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("test([\"\"].Length)", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_15()
        {
            var transformer = Parser("test(a[\"\"].Length)");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("test(a[\"\"].Length)", ast.ToString());
        }


        [TestMethod]
        public void Assign_global_variable_16()
        {
            var transformer = Parser("let c = [124,9295][0]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : any = [124,9295][0]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_17()
        {
            var transformer = Parser("let c = [124,9295][0]++");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : number = [124,9295][0]++", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_18()
        {
            var transformer = Parser("let c = [124,9295][0]++ + [0,9][1]");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
            Assert.AreEqual("var c : number = [124,9295][0]++ + [0,9][1]", ast.ToString());
        }

        [TestMethod]
        public void Invoke_instanced_method()
        {
            var transformer = Parser("fn test() { apa.test3() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Invoke_instanced_method_2()
        {
            var transformer = Parser("fn test() { apa.apa2.apa3() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Invoke_instanced_method_3()
        {
            var transformer = Parser("fn test() { apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Invoke_instanced_method_4()
        {
            var transformer = Parser("fn test() { return apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }
        [TestMethod]
        public void Invoke_instanced_method_5()
        {
            var transformer = Parser("fn test() : string { return apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }
        [TestMethod]
        public void Simple_if()
        {
            var transformer = Parser("if (helloWorld) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr()
        {
            var transformer = Parser("if (helloWorld || true && fal2se || j > 0) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_2()
        {
            var transformer = Parser("if ((helloWorld || true) && fal2se || j > 0) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_3()
        {
            var transformer = Parser("if (((helloWorld || true) || false) && (fal2se || j > 0)) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Simple_if_expr_4()
        {
            var transformer = Parser("if ((helloWorld || true) && (fal2se || j > 0)) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_5()
        {
            var transformer = Parser("if ((true || (helloWorld || true)) && (fal2se || j > 0)) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_6()
        {
            var transformer = Parser("if (a || ((b && x) || c)) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_7()
        {
            var transformer = Parser("if (a == b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }
        [TestMethod]
        public void Simple_if_expr_8()
        {
            var transformer = Parser("if (a != b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_9()
        {
            var transformer = Parser("if (a != !b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_if_expr_10()
        {
            var transformer = Parser("if (!a != !!b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_ifElse()
        {
            var transformer = Parser("if (helloWorld) { } else { console.log(\"hehehe\"); }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Simple_ifElseIf()
        {
            var transformer = Parser("if (helloWorld) { } else if(test) { console.log(\"hehehe\"); } else { bleeh() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }



        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes()
        {
            var transformer = Parser("fn test() { test() }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_2()
        {
            var transformer = Parser("fn test() -> string { test() return \"\" }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_3()
        {
            var transformer = Parser("fn test() -> string { let j = test() return \"\" }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_4()
        {
            var transformer = Parser("fn test() { test(\"hello world\") }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_5()
        {
            var transformer = Parser("fn test() { test(\"hello world\"); }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_6()
        {
            var transformer = Parser("fn test() { test(\"hello world\") test(\"hello world\") }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_7()
        {
            var transformer = Parser("fn test() { test(\"hello world\" + 25) test(\"hello world\" + 99) }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Empty_Function_With_Parameters_1()
        {
            var transformer = Parser("fn test(any a) {  }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }

        [TestMethod]
        public void Empty_Function_With_Parameters_2_jsstyle()
        {
            var transformer = Parser("fn test(any a, number b) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }


        [TestMethod]
        public void Empty_Function_With_Parameters_3_tsstyle()
        {
            var transformer = Parser("fn test(a : any, b : number) { }");
            var ast = transformer.Parse();
            Assert.AreEqual(false, transformer.HasErrors, string.Join(Environment.NewLine, transformer.Errors));
        }



        private LanguageParser Parser(string code)
        {
            return new LanguageParser(new Lexer(code).Tokenize());
        }
    }
}