using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Compiler.Compilers;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.UtilityVisitors;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Parser_TypeLookup
    {



        [TestMethod]
        public void conditional_assign()
        {
            LanguageParser parser;
            var code = TypeLookup("let i = 1 > 0 ? true : false", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var i = 1 > 0 ? True : False", code);
        }

        [TestMethod]
        public void invoke_function_with_conditional()
        {
            LanguageParser parser;
            var code = TypeLookup("fn test(a:bool) -> void { } test(1 > 0 ? true : false)", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test(a:bool) {
}
test(1 > 0 ? True : False)", code);
        }


        [TestMethod]
        public void variable_assigned_struct()
        {
            LanguageParser parser;
            var code = TypeLookup("struct hello { test : number } let j = hello", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j = hello", code);
        }


        [TestMethod]
        public void Invoke_simple_mathematical_expression()
        {
            LanguageParser parser;
            var code = TypeLookup("let result = 5 + (1 * 1) - 992", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var result = 5 + 1 * 1 - 992", code);
        }


        [TestMethod]
        public void variable_assigned_struct_2()
        {
            LanguageParser parser;
            var code = TypeLookup("struct hello { test : number } let j j = hello", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j
j = hello", code);
        }

        [TestMethod]
        public void variable_assigned_array_empty()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = []", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = ['hello', 'wha']", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = [\"hello\", \"wha\"]", code);
        }


        [TestMethod]
        public void variable_assigned_array_empty_2()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = []", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j
j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_2()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = ['hello', 'wha']", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"]", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_3()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = ['hello', 'wha'].Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"].Length", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_4()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = ['hello', 'wha'][0]", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0]", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_5()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = ['hello', 'wha'][0].Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0].Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_1()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = 'test'[0]", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_2()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = 'test'[0]", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_3()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = 1[0]", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_4()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = 1[0]", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_1()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = 'test'.Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\".Length", code);
        }


        [TestMethod]
        public void variable_assigned_constant_property_2()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = 'test'.Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = \"test\".Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_3()
        {
            LanguageParser parser;
            var code = TypeLookup("let j j = 0.Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 0.Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_4()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = 0.Length", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 0.Length", code);
        }

        [TestMethod]
        public void for_loop()
        {
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { break } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup(@"let console = $console
struct test_struct {
  number my_number
}

let a = test_struct { my_number = 1000 }
console.log(a.my_number)", out parser);

            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup(@"let console = $console
struct test_struct {
  my_number : number;
  my_string : string;
}

let a = test_struct { my_number = 1000, my_string = ""hehe"" }
console.log(a.my_number)
console.log(a.my_string)", out parser);

            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i } } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i break } } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i].apa = i } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25) { console.log(s.hello[i+2]) } } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25 * 2 / 4) { console.log(s.hello[i+2] + \" hello world\") } } return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out parser);
            Assert.AreEqual(false, parser.HasErrors);
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
            LanguageParser parser;
            var code = TypeLookup(@"let console = $console

let y = 0
let x = y++
let z = ++y

console.log(x + ' ' + y + ' ' + z)", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var console = $console
var y = 0
var x = y++
var z = ++y
console.log(x + "" "" + y + "" "" + z)", code);
        }

        [TestMethod]
        public void simple_variable_number()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = 0", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 0", code);
        }

        [TestMethod]
        public void simple_variable_string()
        {
            LanguageParser parser;
            var code = TypeLookup("let j = \"0\"", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j = ""0""", code);
        }

        [TestMethod]
        public void simple_fn_string()
        {
            LanguageParser parser;
            var code = TypeLookup("fn test() { return \"0\" }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test() -> string {
  return ""0""
}", code);
        }

        [TestMethod]
        public void simple_fn_number()
        {
            LanguageParser parser;
            var code = TypeLookup("fn test() { return 0 }", out parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test() -> number {
  return 0
}", code);
        }

        private LanguageParser Parser(string code)
        {
            return new LanguageParser(new Lexer(code, false).Tokenize());
        }

        private AstNode Reduce(string code, out LanguageParser parser)
        {
            parser = Parser(code);
            return new NodeReducer().Process(parser.Parse());
        }

        private string TypeLookup(string code, out LanguageParser parser)
        {
            var ast = new NodeTypeBinder().Process(Reduce(code, out parser));
            var analyzer = new ExpressionAnalyzer();
            IList<string> errors;
            var analyzed = analyzer.AnalyzeExpression(ast as EntryNode, out errors);
            if (errors.Count > 0) throw new Exception(string.Join(" ", errors));
            var codeGenerator = new CodeGeneratorVisitor();
            return codeGenerator.Visit(analyzed.GetExpression()).TrimEnd('\r', '\n');
        }
    }
}