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
using Shinobytes.XzaarScript.Scripting;
using Shinobytes.XzaarScript.Scripting.Compilers;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarExpressionAnalyzerTests
    {
        [TestMethod]
        public void Transform_Main_with_loop()
        {
            var code = FormatCode("fn main(int a) { loop { a++ } }");

            Assert.AreEqual(
@"fn main(number a) {
  loop {
    a++
  }
}
", code);
        }


        [TestMethod]
        public void Expression_return_type()
        {
            var code = FormatCode("fn a() { return \"test\" + 5; }");

            Assert.AreEqual(
@"fn a() -> string {
  return ""test"" + 5
}
", code);
        }

        [TestMethod]
        public void Expression_j_mul_assign_and_divide_assign()
        {
            var code = FormatCode("let j = 0 j *= 2 j /= 1");

            Assert.AreEqual(
@"var j = 0
j = j * 2
j = j / 1
", code);
        }




        [TestMethod]
        public void Expression_if_else_if()
        {
            var code = FormatCode("fn a() { if (0 == 0) { } else if(1 == 1) { } else { } }");

            Assert.AreEqual(
@"fn a() {
  if (0 == 0) { 
  }
  else {
    if (1 == 1) { 
    }
  }
}
", code);
        }




        [TestMethod]
        public void Transform_func_with_return_Type()
        {
            var code = FormatCode("fn test() -> number { }");

            Assert.AreEqual(
@"fn test() -> number {
}
", code);
        }

        [TestMethod]
        public void Transform_basic_struct()
        {
            var code = FormatCode("struct MyAwesomeStruct { number myNumberA; string myStringB; any myObjectC; }");
            Assert.AreEqual(
@"struct MyAwesomeStruct {
  number myNumberA
  string myStringB
  any myObjectC
}
", code);
        }

        [TestMethod]
        public void Transform_basic_struct_with_array_field_access_first_item_in_array()
        {
            var code = FormatCode("struct MyAwesomeStruct { number[] numbers } let s = MyAwesomeStruct s.numbers[0] = 0");
            Assert.AreEqual(
@"struct MyAwesomeStruct {
  number[] numbers
}
var s = MyAwesomeStruct
s.numbers[0] = 0
", code);
        }

        [TestMethod]
        public void Transform_basic_struct_and_use_a_field()
        {
            var code = FormatCode("struct MyAwesomeStruct { number a } let j = MyAwesomeStruct j.a = 0");
            Assert.AreEqual(
@"struct MyAwesomeStruct {
  number a
}
var j = MyAwesomeStruct
j.a = 0
", code);
        }

        [TestMethod]
        public void Transform_complex_struct_and_accessor()
        {
            var code = FormatCode("struct BaseStruct { number a } struct MyStructA { BaseStruct a } let b = MyStructA b.a.a = 0");
            Assert.AreEqual(
@"struct BaseStruct {
  number a
}
struct MyStructA {
  BaseStruct a
}
var b = MyStructA
b.a.a = 0
", code);
        }

        [TestMethod]
        public void Transform_Main_with_loop_decrementor()
        {
            var code = FormatCode("fn main(int a) { loop { --a } }");

            Assert.AreEqual(
@"fn main(number a) {
  loop {
    --a
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_foreach_loop()
        {
            var code = FormatCode("fn main(int a) { let collection = [] foreach(let i in collection) { } }");

            Assert.AreEqual(
@"fn main(number a) {
  var collection = []
  foreach (var i in collection) {
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_foreach_loop_2()
        {
            var code = FormatCode("fn test(any j) { } fn main(int a) { let collection = [] foreach(let i in collection) { test(i) } }");

            Assert.AreEqual(
@"fn test(any j) {
}
fn main(number a) {
  var collection = []
  foreach (var i in collection) {
    test(i)
  }
}
", code);
        }

        [TestMethod, ExpectedException(typeof(XzaarExpressionTransformerException))]
        public void Transform_Main_with_foreach_loop_throws_on_parameter_mismatch()
        {
            var code = FormatCode("fn main(int a) { let collection = [] foreach(let i in collection) { main(i) } }");

            Assert.AreEqual(
@"fn main(number a) {
  var collection = []
  foreach (var i in collection) {
    main(i)
  }
}
", code);
        }


        [TestMethod]
        public void Transform_Main_with_function_inside_condition()
        {
            var code = FormatCode("fn always_true() { return true } fn main() { while(always_true()) { } }");

            Assert.AreEqual(
@"fn always_true() -> boolean {
  return True
}
fn main() {
  while (always_true()) {
  }
}
", code);
        }


        [TestMethod]
        public void Transform_Main_with_function_in_expression_inside_condition()
        {
            var code = FormatCode("fn always_true() { return true } fn main() { while(always_true() == true) { } }");

            Assert.AreEqual(
@"fn always_true() -> boolean {
  return True
}
fn main() {
  while (always_true() == True) {
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case_bodies_1()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: { } break case 1: { } return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x) {
    case 0:
      break
    case 1:
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case_bodies_2()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: { main(x) } break case 1: { main(x) } return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x) {
    case 0:
      main(x)
      break
    case 1:
      main(x)
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case_bodies_3()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: main(x) break case 1: main(x) return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x) {
    case 0:
      main(x)
      break
    case 1:
      main(x)
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case_bodies_4()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: main(x) main(x) main(x) break case 1: main(x) main(x) return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x) {
    case 0:
      main(x)
      main(x)
      main(x)
      break
    case 1:
      main(x)
      main(x)
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: break case 1: return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x) {
    case 0:
      break
    case 1:
      return
  }
}
", code);
        }


        [TestMethod]
        public void Transform_Main_with_switch_case_with_expressions()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x + 1) { case 0 - 2: break case 1 + 1: return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x + 1) {
    case 0 - 2:
      break
    case 1 + 1:
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_switch_case_with_expressions_2()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x + 1) { case 0-2: break case 1+1: return } }");

            Assert.AreEqual(
@"fn main(number a) {
  var x = 0
  switch (x + 1) {
    case 0 - 2:
      break
    case 1 + 1:
      return
  }
}
", code);
        }

        [TestMethod]
        public void Transform_global_goto_label()
        {
            var code = FormatCode("infinite_loop: goto infinite_loop");

            Assert.AreEqual(
@"infinite_loop:
goto infinite_loop
", code);
        }


        [TestMethod]
        public void Transform_Main_with_goto_label()
        {
            var code = FormatCode("fn main(int a) { infinite_loop: goto infinite_loop }");

            Assert.AreEqual(
@"fn main(number a) {
  infinite_loop:
  goto infinite_loop
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_goto_label_outside_function()
        {
            var code = FormatCode("outside: fn main(int a) { infinite_loop: goto outside }");

            Assert.AreEqual(
@"outside:
fn main(number a) {
  infinite_loop:
  goto outside
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_goto_label_deep()
        {
            var code = FormatCode("fn main(int a) { infinite_loop: if(true) { if(true) { goto infinite_loop } } }");

            Assert.AreEqual(
@"fn main(number a) {
  infinite_loop:
  if (True) { 
    if (True) { 
      goto infinite_loop
    }
  }
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_loop_continue_break()
        {
            var code = FormatCode("fn main(int a) { loop { a++ if(a == 0) { continue } if (a == 10) { break } } return }");
            Assert.AreEqual(
@"fn main(number a) {
  loop {
    a++
    if (a == 0) { 
      continue
    }
    if (a == 10) { 
      break
    }
  }
  return 
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_while_and_dowhile_loop()
        {
            var code = FormatCode("fn main(int a) { while(true) { a++ do { a-- } while(False) } }");

            Assert.AreEqual(
@"fn main(number a) {
  while (True) {
    a++
    do {
      a--
    } while(False)
  }
}
", code);
        }


        [TestMethod]
        public void Transform_return_expression_mathematical_recursion()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } return i * factorial(i - 1) }");

            Assert.AreEqual(
@"fn factorial(number i) -> number {
  if (i == 1) { 
    return 1
  }
  return i * factorial(i - 1)
}
", code);
        }

        [TestMethod]
        public void Transform_return_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(int i) { return recursive(i + 1) }");

            Assert.AreEqual(
@"fn recursive(number i) -> any {
  return recursive(i + 1)
}
", code);
        }

        [TestMethod]
        public void Transform_return_variable_set_directly_to_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(int i) { let j = recursive(i + 1) return j }");

            Assert.AreEqual(
@"fn recursive(number i) -> number {
  var j = recursive(i + 1)
  return j
}
", code);
        }


        [TestMethod]
        public void Transform_return_variable_set_directly_to_late_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(int i) { let j = late(i + 1) return j } fn late(int k) { return k }");

            Assert.AreEqual(
@"fn recursive(number i) -> number {
  var j = late(i + 1)
  return j
}
fn late(number k) -> number {
  return k
}
", code);
        }

        [TestMethod]
        public void Transform_return_variable_as_expression()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } let result = (i * factorial(i - 1)) return result }");

            Assert.AreEqual(
@"fn factorial(number i) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - 1)
  return result
}
", code);
        }

        [TestMethod]
        public void Transform_return_variable()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } let result = i * factorial(i - 1) return result }");

            Assert.AreEqual(
@"fn factorial(number i) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - 1)
  return result
}
", code);
        }

        [TestMethod]
        public void Transform_return_variable_with_function_as_argument_to_function()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } let result = (i * factorial(i - factorial(i - 1))) return result }");

            Assert.AreEqual(
@"fn factorial(number i) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - factorial(i - 1))
  return result
}
", code);
        }

        [TestMethod]
        public void Transform_Main_with_for_loop()
        {
            var code = FormatCode("fn main(int a) { for(let i = 0; i < 100; i++) { a++ } }");

            Assert.AreEqual(
@"fn main(number a) {
  for (var i = 0; i < 100; i++) {
    a++
  }
}
", code);
        }

        [TestMethod]
        public void Iterate_array_of_numbers_using_for_loop()
        {
            var code = FormatCode("fn main(int a) { let j = [] for(let i = 0; i < 100; i++) { j[i] = i } }");

            Assert.AreEqual(
@"fn main(number a) {
  var j = []
  for (var i = 0; i < 100; i++) {
    j[i] = i
  }
}
", code);
        }

        [TestMethod]
        public void Indexer_inside_indexer()
        {
            var code = FormatCode("fn main(int a) { let j = [] let c = [] j[c[0]] = 10 }");

            Assert.AreEqual(
@"fn main(number a) {
  var j = []
  var c = []
  j[c[0]] = 10
}
", code);
        }



        [TestMethod]
        public void Transform_Main()
        {
            var code = FormatCode("fn main(int a) { if (a == 0) { a = 3 } else { a = 4 } main(a) }");

            Assert.AreEqual(
@"fn main(number a) {
  if (a == 0) { 
    a = 3
  }
  else {
    a = 4
  }
  main(a)
}
", code);
        }

        [TestMethod]
        public void Global_instructions()
        {
            var code = FormatCode("fn a() { } fn b() { a() } b()");

            Assert.AreEqual(
@"fn a() {
}
fn b() {
  a()
}
b()
", code);
        }

        [TestMethod]
        public void Declare_Global_Variable_and_use_in_main()
        {
            var code = FormatCode("var myglobal = 0 fn main() { if (myglobal == 0) { myglobal = 3 } else { myglobal = 4 } main() }");

            Assert.AreEqual(
@"var myglobal = 0
fn main() {
  if (myglobal == 0) { 
    myglobal = 3
  }
  else {
    myglobal = 4
  }
  main()
}
", code);
        }

        [TestMethod]
        public void Declare_local_variable_and_use_in_main()
        {
            var code = FormatCode("fn main() { var myglobal = 0 if (myglobal == 0) { myglobal = 3 } else { myglobal = 4 } main() }");

            Assert.AreEqual(
@"fn main() {
  var myglobal = 0
  if (myglobal == 0) { 
    myglobal = 3
  }
  else {
    myglobal = 4
  }
  main()
}
", code);
        }

        [TestMethod]
        public void Declare_local_variable_inside_deep_scope_in_main()
        {
            var code = FormatCode("fn println(string text) { } fn main() { if (true) { if (false) { let localwo = \"test\" if (true) { let myglobal = 0 println(localwo) println(localwo) } } } }");
            Assert.AreEqual(
@"fn println(string text) {
}
fn main() {
  if (True) { 
    if (False) { 
      var localwo = ""test""
      if (True) { 
        var myglobal = 0
        println(localwo)
        println(localwo)
      }
    }
  }
}
", code);
        }

        [TestMethod, ExpectedException(typeof(XzaarExpressionTransformerException))]
        public void Declare_local_variable_after_use_in_main_throws()
        {
            FormatCode("fn main() { if (myglobal == 0) { myglobal = 3 } else { myglobal = 4 } var myglobal = 0 main() }");
        }

        [TestMethod, ExpectedException(typeof(XzaarExpressionTransformerException))]
        public void Use_of_undeclared_function_throws()
        {
            FormatCode("fn main() { test() }");
        }

        [TestMethod, ExpectedException(typeof(XzaarExpressionTransformerException))]
        public void Use_of_missing_condition_inside_if_statement_throws()
        {
            FormatCode("fn main() { if() {} }");
        }

        [TestMethod, ExpectedException(typeof(XzaarExpressionTransformerException))]
        public void Nameless_function_throws()
        {
            FormatCode("fn () { }");
        }

        [TestMethod, ExpectedException(typeof(XzaarScriptParserException))]
        public void Missing_end_paranthesis_in_function_throws()
        {
            FormatCode("fn a_girl_needs_no_name( { }");
        }

        [TestMethod, ExpectedException(typeof(XzaarScriptParserException))]
        public void Missing_start_paranthesis_in_function_throws()
        {
            FormatCode("fn a) { }");
        }

        [TestMethod, ExpectedException(typeof(XzaarScriptParserException))]
        public void Missing_start_bracket_in_function_throws()
        {
            FormatCode("fn a() }");
        }

        [TestMethod, ExpectedException(typeof(XzaarScriptParserException))]
        public void Missing_end_bracket_in_function_throws()
        {
            FormatCode("fn a() { ");
        }

        private string FormatCode(string inputCode)
        {
            var tokens = new XzaarScriptLexer().Tokenize(inputCode);
            var ast = new XzaarScriptParser().Parse(tokens);
            ast = new XzaarScriptTransformer().Transform(ast);
            var analyzer = new XzaarExpressionAnalyzer();
            var analyzed = analyzer.AnalyzeExpression(ast);
            var codeGenerator = new CodeGeneratorVisitor();
            return codeGenerator.Visit(analyzed.GetExpression());
        }
    }
}