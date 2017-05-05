using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.UtilityVisitors;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Transformer_Reduction
    {
        [TestMethod]
        public void while_Expression()
        {
            XzaarNodeTransformer transformer;
            var tree = Reduce("while(true && false != true) { a.test() }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("while (true && false != true) { a.test() }", tree.ToString());
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_4()
        {
            XzaarNodeTransformer transformer;
            var tree = Reduce("switch(a+b) { case 99+apa: a.j() return case aa+apa: a.c() break }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("switch (a + b) { case 99 + apa: a.j() return  case aa + apa: a.c() break }", tree.ToString());
        }


        [TestMethod]
        public void Simple_if_expr_5()
        {
            XzaarNodeTransformer transformer;
            var tree = Reduce("if ((true || (helloWorld || true)) && (fal2se || j > 0)) { }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("if (true || helloWorld || true && fal2se || j > 0) {} else {}", tree.ToString());
        }

        [TestMethod]
        public void Simple_if_expr_6()
        {
            XzaarNodeTransformer transformer;
            var tree = Reduce("if (a || ((b && x) || c)) { }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("if (a || b && x || c) {} else {}", tree.ToString());
        }


        private XzaarNodeTransformer Transformer(string code)
        {
            return new XzaarNodeTransformer(new XzaarSyntaxParser(new XzaarScriptLexer(code).Tokenize()).Parse());
        }

        private XzaarAstNode Reduce(string code, out XzaarNodeTransformer transformer)
        {
            transformer = Transformer(code);
            return new XzaarNodeReducer().Process(transformer.Transform());
        }
    }

    [TestClass]
    public class XzaarScript_Transformer_TypeLookup
    {



        [TestMethod]
        public void variable_assigned_struct()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct hello { test : number } let j = hello", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j = hello", code);
        }


        [TestMethod]
        public void Invoke_simple_mathematical_expression()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let result = 5 + (1 * 1) - 992", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var result = 5 + 1 * 1 - 992", code);
        }


        [TestMethod]
        public void variable_assigned_struct_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct hello { test : number } let j j = hello", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j
j = hello", code);
        }

        [TestMethod]
        public void variable_assigned_array_empty()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = []", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = ['hello', 'wha']", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = [\"hello\", \"wha\"]", code);
        }


        [TestMethod]
        public void variable_assigned_array_empty_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = []", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var j
j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = ['hello', 'wha']", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"]", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_3()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = ['hello', 'wha'].Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"].Length", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_4()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = ['hello', 'wha'][0]", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0]", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_5()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = ['hello', 'wha'][0].Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0].Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_1()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = 'test'[0]", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = 'test'[0]", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_3()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = 1[0]", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_4()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = 1[0]", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_1()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = 'test'.Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\".Length", code);
        }


        [TestMethod]
        public void variable_assigned_constant_property_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = 'test'.Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = \"test\".Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_3()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j j = 0.Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 0.Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_4()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = 0.Length", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = 0.Length", code);
        }

        [TestMethod]
        public void for_loop()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    s.hello[i] = i
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void for_break_loop()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { break } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    break
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void Declare_struct_and_create_new_instance_with_initializer()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            XzaarNodeTransformer transformer;
            var code = TypeLookup(@"let console = $console
struct test_struct {
  number my_number
}

let a = test_struct { my_number = 1000 }
console.log(a.my_number)", out transformer);

            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var console = $console
struct test_struct {
  number my_number
}
var a = test_struct { my_number = 1000 }
console.log(a.my_number)", code);
        }

        [TestMethod]
        public void Declare_struct_and_create_new_instance_with_initializer_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            XzaarNodeTransformer transformer;
            var code = TypeLookup(@"let console = $console
struct test_struct {
  my_number : number;
  my_string : string;
}

let a = test_struct { my_number = 1000, my_string = ""hehe"" }
console.log(a.my_number)
console.log(a.my_string)", out transformer);

            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var console = $console
struct test_struct {
  number my_number
  string my_string
}
var a = test_struct { my_number = 1000, my_string = ""hehe"" }
console.log(a.my_number)
console.log(a.my_string)", code);
        }


        [TestMethod]
        public void for_while_loop()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i } } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    while (True) {
      s.hello[i] = i
    }
  }
  return s.hello
}", code);
        }



        [TestMethod]
        public void for_while_break_loop()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i break } } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    while (True) {
      s.hello[i] = i
      break
    }
  }
  return s.hello
}", code);
        }


        [TestMethod]
        public void for_loop_with_if_statement_1()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i].apa = i } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    s.hello[i].apa = i
  }
  return s.hello
}", code);
        }


        [TestMethod]
        public void for_loop_with_if_statement_2()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25) { console.log(s.hello[i+2]) } } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    if (s.hello[i] == i + 25) { 
      console.log(s.hello[i + 2])
    }
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void for_loop_with_if_statement__3()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25 * 2 / 4) { console.log(s.hello[i+2] + \" hello world\") } } return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    if (s.hello[i] == i + 25 * 2 / 4) { 
      console.log(s.hello[i + 2] + "" hello world"")
    }
  }
  return s.hello
}", code);
        }



        [TestMethod]
        public void switch_normal_case()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    case 9:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void switch_default_case()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    default:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void switch_normal_and_default_case()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    case 9:
      break
    default:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void Pre_Post_Increment_Variable_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            XzaarNodeTransformer transformer;
            var code = TypeLookup(@"let console = $console

let y = 0
let x = y++
let z = ++y

console.log(x + ' ' + y + ' ' + z)", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var console = $console
var y = 0
var x = y++
var z = ++y
console.log(x + "" "" + y + "" "" + z)", code);
        }

        [TestMethod]
        public void simple_variable_number()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = 0", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j = 0", code);
        }

        [TestMethod]
        public void simple_variable_string()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("let j = \"0\"", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"var j = ""0""", code);
        }

        [TestMethod]
        public void simple_fn_string()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("fn test() { return \"0\" }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"fn test() -> string {
  return ""0""
}", code);
        }

        [TestMethod]
        public void simple_fn_number()
        {
            XzaarNodeTransformer transformer;
            var code = TypeLookup("fn test() { return 0 }", out transformer);
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual(@"fn test() -> number {
  return 0
}", code);
        }


        private XzaarNodeTransformer Transformer(string code)
        {
            return new XzaarNodeTransformer(new XzaarSyntaxParser(new XzaarScriptLexer(code).Tokenize()).Parse());
        }

        private XzaarAstNode Reduce(string code, out XzaarNodeTransformer transformer)
        {
            transformer = Transformer(code);
            return new XzaarNodeReducer().Process(transformer.Transform());
        }

        private string TypeLookup(string code, out XzaarNodeTransformer transformer)
        {
            var ast = new XzaarNodeTypeBinder().Process(Reduce(code, out transformer));
            var analyzer = new XzaarExpressionAnalyzer();
            var analyzed = analyzer.AnalyzeExpression(ast as EntryNode);
            var codeGenerator = new CodeGeneratorVisitor();
            return codeGenerator.Visit(analyzed.GetExpression()).TrimEnd('\r', '\n');
        }
    }

    [TestClass]
    public class XzaarScript_Transformer
    {
        [TestMethod]
        public void Empty_struct()
        {
            var transformer = Transformer("struct test { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Struct_tsstyle_fields_1()
        {
            var transformer = Transformer("struct test { hello : string; t2 : number; }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Struct_tsstyle_fields_2()
        {
            var transformer = Transformer("struct test { hello : string t2 : number }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Struct_csstyle_fields_1()
        {
            var transformer = Transformer("struct test { string hello; number t2; }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Struct_csstyle_fields_2()
        {
            var transformer = Transformer("struct test { string hello number t2 }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Walk_post_increment()
        {
            var transformer = Transformer("i++");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Walk_pre_increment_decrement_1()
        {
            var transformer = Transformer("++i; --i");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("++i --i", ast.ToString());
        }

        [TestMethod]
        public void Walk_pre_increment_decrement_2()
        {
            var transformer = Transformer("++i --i");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("++i --i", ast.ToString());
        }

        //[TestMethod]
        //public void Console_log_constant_expr_3()
        //{
        //    var transformer = Transformer("struct j { s:number} fn test(any console) { let f =j f.s=99 console.log(f.s) }");
        //    var ast = transformer.Transform();
        //    Assert.AreEqual(false, transformer.HasErrors);
        //    Assert.AreEqual("++i --i", ast.ToString());
        //}


        [TestMethod]
        public void Declare_Variable_With_Type()
        {
            var transformer = Transformer("let j : number = 0");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j : number = 0", ast.ToString());
        }


        [TestMethod]
        public void Access_literal_with_element_index()
        {
            var transformer = Transformer("1[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("1[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_2()
        {
            var transformer = Transformer("\"hello world\"[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("\"hello world\"[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_3()
        {
            var transformer = Transformer("1[0].test()");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("1[0].test()", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_4()
        {
            var transformer = Transformer("1[0][1].test()");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("1[0][1].test()", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_5()
        {
            var transformer = Transformer("1[0][1].test[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("1[0][1].test[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_6()
        {
            var transformer = Transformer("let j = 1[0][1] + test[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var j : number = 1[0][1] + test[0]", ast.ToString());
        }

        [TestMethod]
        public void Access_literal_with_element_index_7()
        {
            var transformer = Transformer("let b = 1[0][1].ohoh() + test[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var b : any = 1[0][1].ohoh() + test[0]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_1()
        {
            var transformer = Transformer("helloWorld[0][1]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("helloWorld[0][1]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_3()
        {
            var transformer = Transformer("\"hello\"[0][0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("\"hello\"[0][0]", ast.ToString());
        }

        [TestMethod]
        public void Multi_element_index_2()
        {
            var transformer = Transformer("helloWorld[0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("helloWorld[0]", ast.ToString());
        }

        [TestMethod]
        public void Foreach_invoke_function_on_item()
        {
            var transformer = Transformer("foreach(let a in b) { a.j() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("foreach (var a in b) { a.j() }", ast.ToString());
        }

        [TestMethod]
        public void For_invoke_function_on_item()
        {

            var transformer = Transformer("for(let i = 0; i < k.Length; i++) { let a = x[i] a.j() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("for (var i : number = 0; i < k.Length; i++) { var a : any = x[i] a.j() }", ast.ToString());
        }

        [TestMethod]
        public void Loop_invoke_function_on_item()
        {
            var transformer = Transformer("loop { a.j() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("loop { a.j() }", ast.ToString());
        }

        [TestMethod]
        public void Do_while_true()
        {
            var transformer = Transformer("do { a.test() } while(true)");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("do { a.test() } while (true)", ast.ToString());
        }

        [TestMethod]
        public void Do_while_Expression()
        {
            var transformer = Transformer("do { a.test() } while(true && false != true)");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("do { a.test() } while (true && false != true)", ast.ToString());
        }


        [TestMethod]
        public void while_true()
        {
            var transformer = Transformer("while(true) { a.test() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("while (true) { a.test() }", ast.ToString());
        }

        [TestMethod]
        public void while_Expression()
        {
            var transformer = Transformer("while(true && false != true) { a.test() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("while (true && false != true) { a.test() }", ast.ToString());
        }


        [TestMethod]
        public void Empty_switch()
        {
            var transformer = Transformer("switch(a+b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Switch_invoke_function_on_item_1()
        {
            var transformer = Transformer("switch(a+b) { case 99+apa: a.j() break }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Switch_invoke_function_on_item_2()
        {
            var transformer = Transformer("switch(a+b) { case 99+apa: a.j() return }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_3()
        {
            var transformer = Transformer("switch(a+b) { case 99+apa: a.j() break case aa+apa: a.c() return }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Switch_invoke_function_on_item_4()
        {
            var transformer = Transformer("switch(a+b) { case 99+apa: a.j() return case aa+apa: a.c() break }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Assign_global_variable_1()
        {
            var transformer = Transformer("let c = 0");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Assign_global_variable_2()
        {
            var transformer = Transformer("let c = 0 + 51 * 2");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Assign_global_variable_3()
        {
            var transformer = Transformer("let c = 0 + 51 * 2 > 0");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Assign_global_variable_4()
        {
            var transformer = Transformer("let c = 0 + 51 * 2 > 0 || j < 0 && i");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Assign_global_variable_5()
        {
            var transformer = Transformer("let c = 0 + 51 * 2 > 0 || j < 0 && i let x = 0");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }



        [TestMethod]
        public void Assign_global_variable_7()
        {
            var transformer = Transformer("let c = [124]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : array = [124]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_8()
        {
            var transformer = Transformer("let c = [124,9295]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : array = [124,9295]", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_9()
        {
            var transformer = Transformer("let c = [124,\"asdasd\",9295]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : array = [124,\"asdasd\",9295]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_10()
        {
            var transformer = Transformer("let c:any = []");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : any = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_6()
        {
            var transformer = Transformer("let c = []");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : array = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_11()
        {
            var transformer = Transformer("c = []");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("c = []", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_12()
        {
            var transformer = Transformer("c = [\"\"].Length");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("c = [\"\"].Length", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_13()
        {
            var transformer = Transformer("c = [\"\"].Length");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("c = [\"\"].Length", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_14()
        {
            var transformer = Transformer("test([\"\"].Length)");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("test([\"\"].Length)", ast.ToString());
        }
        [TestMethod]
        public void Assign_global_variable_15()
        {
            var transformer = Transformer("test(a[\"\"].Length)");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("test(a[\"\"].Length)", ast.ToString());
        }


        [TestMethod]
        public void Assign_global_variable_16()
        {
            var transformer = Transformer("let c = [124,9295][0]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : any = [124,9295][0]", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_17()
        {
            var transformer = Transformer("let c = [124,9295][0]++");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : number = [124,9295][0]++", ast.ToString());
        }

        [TestMethod]
        public void Assign_global_variable_18()
        {
            var transformer = Transformer("let c = [124,9295][0]++ + [0,9][1]");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
            Assert.AreEqual("var c : number = [124,9295][0]++ + [0,9][1]", ast.ToString());
        }

        [TestMethod]
        public void Invoke_instanced_method()
        {
            var transformer = Transformer("fn test() { apa.test3() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Invoke_instanced_method_2()
        {
            var transformer = Transformer("fn test() { apa.apa2.apa3() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Invoke_instanced_method_3()
        {
            var transformer = Transformer("fn test() { apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Invoke_instanced_method_4()
        {
            var transformer = Transformer("fn test() { return apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }
        [TestMethod]
        public void Invoke_instanced_method_5()
        {
            var transformer = Transformer("fn test() : string { return apa.apa2.apa3.apa4.test3().test }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }
        [TestMethod]
        public void Simple_if()
        {
            var transformer = Transformer("if (helloWorld) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr()
        {
            var transformer = Transformer("if (helloWorld || true && fal2se || j > 0) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_2()
        {
            var transformer = Transformer("if ((helloWorld || true) && fal2se || j > 0) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_3()
        {
            var transformer = Transformer("if (((helloWorld || true) || false) && (fal2se || j > 0)) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Simple_if_expr_4()
        {
            var transformer = Transformer("if ((helloWorld || true) && (fal2se || j > 0)) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_5()
        {
            var transformer = Transformer("if ((true || (helloWorld || true)) && (fal2se || j > 0)) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_6()
        {
            var transformer = Transformer("if (a || ((b && x) || c)) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_7()
        {
            var transformer = Transformer("if (a == b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }
        [TestMethod]
        public void Simple_if_expr_8()
        {
            var transformer = Transformer("if (a != b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_9()
        {
            var transformer = Transformer("if (a != !b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_if_expr_10()
        {
            var transformer = Transformer("if (!a != !!b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_ifElse()
        {
            var transformer = Transformer("if (helloWorld) { } else { console.log(\"hehehe\"); }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Simple_ifElseIf()
        {
            var transformer = Transformer("if (helloWorld) { } else if(test) { console.log(\"hehehe\"); } else { bleeh() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }



        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes()
        {
            var transformer = Transformer("fn test() { test() }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_2()
        {
            var transformer = Transformer("fn test() -> string { test() return \"\" }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_3()
        {
            var transformer = Transformer("fn test() -> string { let j = test() return \"\" }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_4()
        {
            var transformer = Transformer("fn test() { test(\"hello world\") }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_5()
        {
            var transformer = Transformer("fn test() { test(\"hello world\"); }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_6()
        {
            var transformer = Transformer("fn test() { test(\"hello world\") test(\"hello world\") }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Parse_identifiers_expressions_and_scopes_7()
        {
            var transformer = Transformer("fn test() { test(\"hello world\" + 25) test(\"hello world\" + 99) }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Empty_Function_With_Parameters_1()
        {
            var transformer = Transformer("fn test(any a) {  }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }

        [TestMethod]
        public void Empty_Function_With_Parameters_2_jsstyle()
        {
            var transformer = Transformer("fn test(any a, number b) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }


        [TestMethod]
        public void Empty_Function_With_Parameters_3_tsstyle()
        {
            var transformer = Transformer("fn test(a : any, b : number) { }");
            var ast = transformer.Transform();
            Assert.AreEqual(false, transformer.HasErrors);
        }



        private XzaarNodeTransformer Transformer(string code)
        {
            return new XzaarNodeTransformer(new XzaarSyntaxParser(new XzaarScriptLexer(code).Tokenize()).Parse());
        }
    }
}