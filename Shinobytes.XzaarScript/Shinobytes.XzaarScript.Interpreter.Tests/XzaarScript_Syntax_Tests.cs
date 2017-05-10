using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Compiler.Compilers;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.UtilityVisitors;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Syntax_Tests
    {

        [TestMethod]
        public void assign_conditional()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let a = 1 > 0 ? true : false;");

            Assert.AreEqual(@"var a = 1 > 0 ? True : False", code);
        }

        [TestMethod]
        public void Bang_not_equal()
        {
            var code = FormatCode(@"let a = true let b = false let c = a != b");

            Assert.AreEqual(@"var a = True
var b = False
var c = a != b", code);
        }


        [TestMethod]
        public void Bang_not_variable()
        {
            var code = FormatCode(@"let a = true let b = false a = !b");

            Assert.AreEqual(@"var a = True
var b = False
a = !b", code);
        }

        [TestMethod]
        public void Bang_not_variable_1()
        {
            var code = FormatCode(@"let a = true let b = false a = !!b");

            Assert.AreEqual(@"var a = True
var b = False
a = !!b", code);
        }

        [TestMethod]
        public void Bang_not_variable_2()
        {
            var code = FormatCode(@"let a = true let b = false a = a && !b");

            Assert.AreEqual(@"var a = True
var b = False
a = a && !b", code);
        }

        [TestMethod]
        public void Bang_not_expression()
        {
            var code = FormatCode(@"let b = true let c = false let a = !(b && c)");

            Assert.AreEqual(@"var b = True
var c = False
var a = !b && c", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_1()
        {
            var code = FormatCode(@"fn test(val:bool) { } test(!true)");

            Assert.AreEqual(@"fn test(val:bool) {
}
test(!True)", code);
        }


        [TestMethod]
        public void Bang_not_expression_invoke_method__()
        {
            var code = FormatCode(@"fn test(val:bool) { } test(!true)");

            Assert.AreEqual(@"fn test(val:bool) {
}
test(!True)", code);
        }


        [TestMethod]
        public void Bang_not_expression_invoke_method_2()
        {
            var code = FormatCode(@"fn test(bool val) { } test(!false)");

            Assert.AreEqual(@"fn test(val:bool) {
}
test(!False)", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_3()
        {
            var code = FormatCode(@"fn test(bool val) { } let a = !test(!false)");

            Assert.AreEqual(@"fn test(val:bool) {
}
var a = !test(!False)", code);
        }

        [TestMethod]
        public void Switch_Case_Invoke_function_dynamically_with_args_4()
        {
            var code = FormatCode("fn greet(string name) { let j = 0 switch(name) { case \"zerratar\": return true } return false }");
            Assert.AreEqual(@"fn greet(name:string) -> bool {
  var j = 0
  switch (name) {
    case ""zerratar"":
      return True
  }
  return False
}", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_4()
        {
            var code = FormatCode(@"fn test(bool val) { } let a = !test(!false)");

            Assert.AreEqual(@"fn test(val:bool) {
}
var a = !test(!False)", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_5()
        {
            var code = FormatCode(@"fn test(bool val) -> bool { return val } let a = !test(!false) && test(true)");

            Assert.AreEqual(@"fn test(val:bool) -> bool {
  return val
}
var a = !test(!False) && test(True)", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_6()
        {
            var code = FormatCode(@"fn test(bool val) { } let a = !(test(!false) && test(true))");

            Assert.AreEqual(@"fn test(val:bool) {
}
var a = !test(!False) && test(True)", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_7()
        {
            var code = FormatCode(@"fn test(bool val) { } let a = test(!false) && test(true)");

            Assert.AreEqual(@"fn test(val:bool) {
}
var a = test(!False) && test(True)", code);
        }

        [TestMethod]
        public void Bang_not_expression_invoke_method_8()
        {
            var code = FormatCode(@"fn test(bool val) { } let a = True a = test(!false) && test(true)");

            Assert.AreEqual(@"fn test(val:bool) {
}
var a = True
a = test(!False) && test(True)", code);
        }

        [TestMethod]
        public void Conditional_Statement_1()
        {
            var code = FormatCode("let apa = 0 let c = apa == 8 || apa < 1 * 2");
            Assert.AreEqual(
@"var apa = 0
var c = apa == 8 || apa < 1 * 2", code);
        }
        [TestMethod]
        public void Conditional_Statement_2()
        {
            var code = FormatCode("let apa = 0 let c = ((apa == 8) || (apa < (1 * 2))) || (apa >= 100)");
            Assert.AreEqual(
@"var apa = 0
var c = apa == 8 || apa < 1 * 2 || apa >= 100", code);
        }
        [TestMethod]
        public void Transform_Main_with_loop()
        {
            var code = FormatCode("fn main(int a) { loop { a++ } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  loop {
    a++
  }
}", code);

        }
        [TestMethod]
        public void Transform_complex_strcat()
        {
            var code = FormatCode(@"fn main(any console) -> void {
   let anders = ""Anders""
   console.log(""Hey there "" + anders + ""!"")
}");

            Assert.AreEqual(
@"fn main(console:any) {
  var anders = ""Anders""
  console.log(""Hey there "" + anders + ""!"")
}", code);


        }

        [TestMethod]
        public void Transform_complex_strcat_1()
        {
            var code = FormatCode(@"struct s {
    number num,
    string str
}
fn main(console:any) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num + ""asd"" + 0 + 16
    console.log(j.str + j.num)
}");

            Assert.AreEqual(
@"struct s {
  number num
  string str
}
fn main(console:any) {
  var j = s
  j.num = 9000
  j.str = ""over "" + j.num + ""asd"" + 0 + 16
  console.log(j.str + j.num)
}", code);


        }

        [TestMethod]
        public void Transform_complex_strcat_2()
        {
            var code = FormatCode(@"struct s {
    number num,
    string str
}
fn main(console:any) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num + ""asd"" + 0
    console.log(j.str + j.num)
}");

            Assert.AreEqual(
@"struct s {
  number num
  string str
}
fn main(console:any) {
  var j = s
  j.num = 9000
  j.str = ""over "" + j.num + ""asd"" + 0
  console.log(j.str + j.num)
}", code);
        }

        [TestMethod]
        public void Transform_complex_strcat_3()
        {
            var code = FormatCode(@"struct s {
    number num,
    string str
}
fn main(console:any) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num
    console.log(j.str + j.num)
}");

            Assert.AreEqual(
@"struct s {
  number num
  string str
}
fn main(console:any) {
  var j = s
  j.num = 9000
  j.str = ""over "" + j.num
  console.log(j.str + j.num)
}", code);
        }

        [TestMethod]
        public void Push_array_return_item_value_8()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var code = FormatCode(@"let console = $console

let names = [""Daisy"",""Friday""]

console.log(names[1][0])");
            Assert.AreEqual(
  @"var console = $console
var names = [""Daisy"", ""Friday""]
console.log(names[1][0])", code);
        }

        [TestMethod]
        public void Transform_complex_strcat_4()
        {
            var code = FormatCode(@"struct s {
    number num,
    string str
}
fn main(console:any) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + 5
    console.log(j.str + j.num)
}");

            Assert.AreEqual(
@"struct s {
  number num
  string str
}
fn main(console:any) {
  var j = s
  j.num = 9000
  j.str = ""over "" + 5
  console.log(j.str + j.num)
}", code);
        }

        [TestMethod]
        public void Transform_complex_strcat_5()
        {
            var code = FormatCode(@"struct s {
    number num,
    string str
}
fn main(console:any) -> void {
    let j = s
    j.num = 9000
    j.str = ""over ""
    console.log(j.str + j.num)
}");

            Assert.AreEqual(
@"struct s {
  number num
  string str
}
fn main(console:any) {
  var j = s
  j.num = 9000
  j.str = ""over ""
  console.log(j.str + j.num)
}", code);
        }

        [TestMethod]
        public void Expression_return_type()
        {
            var code = FormatCode("fn a() { return \"test\" + 5; }");

            Assert.AreEqual(
@"fn a() -> string {
  return ""test"" + 5
}", code);
        }

        [TestMethod]
        public void Expression_j_mul_assign_and_divide_assign()
        {
            var code = FormatCode("let j = 0 j *= 2 j /= 1");

            Assert.AreEqual(
@"var j = 0
j = j * 2
j = j / 1", code);
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
}", code);
        }

        [TestMethod]
        public void Mathematical_Expression()
        {
            var code = FormatCode("let j = 5 + 1 * 1 - 992");
            Assert.AreEqual("var j = 5 + 1 * 1 - 992", code);
        }

        [TestMethod]
        public void Mathematical_Expression_1()
        {
            var code = FormatCode("let j = 5 + (1 * 1) - 992");
            Assert.AreEqual("var j = 5 + 1 * 1 - 992", code);
        }

        [TestMethod]
        public void Mathematical_Expression_2()
        {
            var code = FormatCode("let j = (5 + (1 * 1) - 992)");
            Assert.AreEqual("var j = 5 + 1 * 1 - 992", code);
        }

        [TestMethod]
        public void Mathematical_Expression_as_function_argument_1()
        {
            var code = FormatCode("fn a(number c) {} let j = 5 + 1 * 1 - 992 a(j)");
            Assert.AreEqual(@"fn a(c:f64) {
}
var j = 5 + 1 * 1 - 992
a(j)", code);
        }


        [TestMethod]
        public void Mathematical_Expression_as_function_argument_2()
        {
            var code = FormatCode("fn a(number c) {} a(5 + 1 * 1 - 992)");
            Assert.AreEqual(@"fn a(c:f64) {
}
a(5 + 1 * 1 - 992)", code);
        }

        [TestMethod]
        public void Transform_func_with_return_Type()
        {
            var code = FormatCode("fn test() -> number { }");

            Assert.AreEqual(
@"fn test() -> number {
}", code);
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
}", code);
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
s.numbers[0] = 0", code);
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
j.a = 0", code);
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
b.a.a = 0", code);
        }

        [TestMethod]
        public void Transform_Main_with_loop_decrementor()
        {
            var code = FormatCode("fn main(int a) { loop { --a } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  loop {
    --a
  }
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_foreach_loop()
        {
            var code = FormatCode("fn main(int a) { let collection = [] foreach(let i in collection) { } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var collection = []
  foreach (var i in collection) {
  }
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_foreach_loop_2()
        {
            var code = FormatCode("fn test(any j) { } fn main(int a) { let collection = [] foreach(let i in collection) { test(i) } }");

            Assert.AreEqual(
@"fn test(j:any) {
}
fn main(a:f64) {
  var collection = []
  foreach (var i in collection) {
    test(i)
  }
}", code);
        }

        [TestMethod, ExpectedException(typeof(ExpressionException))]
        public void Transform_Main_with_foreach_loop_throws_on_parameter_mismatch()
        {
            var code = FormatCode("fn main(int a) { let collection = [] foreach(let i in collection) { main(i) } }");

            Assert.AreEqual(
@"fn main(number a) {
  var collection = []
  foreach (var i in collection) {
    main(i)
  }
}", code);
        }


        [TestMethod]
        public void Transform_Main_with_function_inside_condition()
        {
            var code = FormatCode("fn always_true() { return true } fn main() { while(always_true()) { } }");

            Assert.AreEqual(
@"fn always_true() -> bool {
  return True
}
fn main() {
  while (always_true()) {
  }
}", code);
        }


        [TestMethod]
        public void Transform_Main_with_function_in_expression_inside_condition()
        {
            var code = FormatCode("fn always_true() { return true } fn main() { while(always_true() == true) { } }");

            Assert.AreEqual(
@"fn always_true() -> bool {
  return True
}
fn main() {
  while (always_true() == True) {
  }
}", code);
        }

        [TestMethod]
        public void Switch_case_bodies_1()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: { } break case 1: { } return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x) {
    case 0:
      break
    case 1:
      return
  }
}", code);
        }

        [TestMethod]
        public void Switch_case_bodies_2()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: { main(x) } break case 1: { main(x) } return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x) {
    case 0:
      main(x)
      break
    case 1:
      main(x)
      return
  }
}", code);
        }

        [TestMethod]
        public void Switch_case_bodies_3()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: main(x) break case 1: main(x) return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x) {
    case 0:
      main(x)
      break
    case 1:
      main(x)
      return
  }
}", code);
        }

        [TestMethod]
        public void Switch_case_bodies_4()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: main(x) main(x) main(x) break case 1: main(x) main(x) return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
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
}", code);
        }

        [TestMethod]
        public void Switch_case()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x) { case 0: break case 1: return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x) {
    case 0:
      break
    case 1:
      return
  }
}", code);
        }


        [TestMethod]
        public void Switch_case_with_expressions()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x + 1) { case 0 - 2: break case 1 + 1: return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x + 1) {
    case 0 - 2:
      break
    case 1 + 1:
      return
  }
}", code);
        }

        [TestMethod]
        public void Switch_case_with_expressions_2()
        {
            var code = FormatCode("fn main(int a) { let x = 0 switch(x + 1) { case 0-2: break case 1+1: return } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var x = 0
  switch (x + 1) {
    case 0 - 2:
      break
    case 1 + 1:
      return
  }
}", code);
        }

        [TestMethod]
        public void Transform_global_goto_label()
        {
            var code = FormatCode("infinite_loop: goto infinite_loop");

            Assert.AreEqual(
@"infinite_loop:
goto infinite_loop", code);
        }


        [TestMethod]
        public void Transform_Main_with_goto_label()
        {
            var code = FormatCode("fn main(int a) { infinite_loop: goto infinite_loop }");

            Assert.AreEqual(
@"fn main(a:f64) {
  infinite_loop:
  goto infinite_loop
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_goto_label_outside_function()
        {
            var code = FormatCode("outside: fn main(int a) { infinite_loop: goto outside }");

            Assert.AreEqual(
@"outside:
fn main(a:f64) {
  infinite_loop:
  goto outside
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_goto_label_deep()
        {
            var code = FormatCode("fn main(int a) { infinite_loop: if(true) { if(true) { goto infinite_loop } } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  infinite_loop:
  if (True) { 
    if (True) { 
      goto infinite_loop
    }
  }
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_loop_continue_break()
        {
            var code = FormatCode("fn main(int a) { loop { a++ if(a == 0) { continue } if (a == 10) { break } } return }");
            Assert.AreEqual(
@"fn main(a:f64) {
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
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_while_and_dowhile_loop()
        {
            var code = FormatCode("fn main(int a) { while(true) { a++ do { a-- } while(False) } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  while (True) {
    a++
    do {
      a--
    } while(False)
  }
}", code);
        }


        [TestMethod]
        public void Transform_return_expression_mathematical_recursion()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } return i * factorial(i - 1) }");

            Assert.AreEqual(
@"fn factorial(i:f64) -> number {
  if (i == 1) { 
    return 1
  }
  return i * factorial(i - 1)
}", code);
        }

        [TestMethod]
        public void Transform_return_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(int i) { return recursive(i + 1) }");

            Assert.AreEqual(
@"fn recursive(i:f64) -> any {
  return recursive(i + 1)
}", code);
        }

        [TestMethod]
        public void Transform_return_variable_set_directly_to_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(i:i32) { let j = recursive(i + 1) return j }");

            Assert.AreEqual(
@"fn recursive(i:i32) -> any {
  var j = recursive(i + 1)
  return j
}", code);
        }


        [TestMethod]
        public void Transform_return_variable_set_directly_to_late_functioncall_recursion()
        {
            var code = FormatCode("fn recursive(i32 i) -> number { let j = late(i + 1) return j } fn late(int k) { return k }");

            Assert.AreEqual(
@"fn recursive(i:i32) -> number {
  var j = late(i + 1)
  return j
}
fn late(k:f64) -> number {
  return k
}", code);
        }

        [TestMethod]
        public void Transform_return_variable_as_expression()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } let result = (i * factorial(i - 1)) return result }");

            Assert.AreEqual(
@"fn factorial(i:f64) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - 1)
  return result
}", code);
        }

        [TestMethod]
        public void Transform_return_variable()
        {
            var code = FormatCode("fn factorial(int i) { if (i == 1) { return 1 } let result = i * factorial(i - 1) return result }");

            Assert.AreEqual(
@"fn factorial(i:f64) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - 1)
  return result
}", code);
        }

        [TestMethod]
        public void Transform_return_variable_with_function_as_argument_to_function()
        {
            var code = FormatCode("fn factorial(number i) { if (i == 1) { return 1 } let result = (i * factorial(i - factorial(i - 1))) return result }");

            Assert.AreEqual(
@"fn factorial(i:f64) -> number {
  if (i == 1) { 
    return 1
  }
  var result = i * factorial(i - factorial(i - 1))
  return result
}", code);
        }

        [TestMethod]
        public void Transform_Main_with_for_loop()
        {
            var code = FormatCode("fn main(int a) { for(let i = 0; i < 100; i++) { a++ } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  for (var i = 0; i < 100; i++) {
    a++
  }
}", code);
        }

        [TestMethod]
        public void Iterate_array_of_numbers_using_for_loop()
        {
            var code = FormatCode("fn main(int a) { let j = [] for(let i = 0; i < 100; i++) { j[i] = i } }");

            Assert.AreEqual(
@"fn main(a:f64) {
  var j = []
  for (var i = 0; i < 100; i++) {
    j[i] = i
  }
}", code);
        }

        [TestMethod]
        public void Indexer_inside_indexer()
        {
            var code = FormatCode("fn main(i32 a) { let j = [] let c = [] j[c[0]] = 10 }");

            Assert.AreEqual(
@"fn main(a:i32) {
  var j = []
  var c = []
  j[c[0]] = 10
}", code);
        }



        [TestMethod]
        public void Transform_Main()
        {
            var code = FormatCode("fn main(int a) { if (a == 0) { a = 3 } else { a = 4 } main(a) }");

            Assert.AreEqual(
@"fn main(a:f64) {
  if (a == 0) { 
    a = 3
  }
  else {
    a = 4
  }
  main(a)
}", code);
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
b()", code);
        }

        [TestMethod]
        public void Transform_call_function_on_any_object()
        {
            var code = FormatCode("fn test(any s) -> string { s.TestMethod() return s.Test } ");
            Assert.AreEqual(
@"fn test(s:any) -> string {
  s.TestMethod()
  return s.Test
}", code);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_execute_function_with_xzaar_object_and_return_result()
        {
            var code = FormatCode("fn test(any s) -> any { return s.Result1(\"hello there\") } ");
            Assert.AreEqual(@"fn test(s:any) -> any {
  return s.Result1(""hello there"")
}", code);
        }

        [TestMethod]
        public void Switch_case_test()
        {
            var code =
                FormatCode(
                    "fn greet(string name) { let j = 0 switch(name) { case \"zerratar\": j = 2 return true } return false }");

            Assert.AreEqual(
            @"fn greet(name:string) -> bool {
  var j = 0
  switch (name) {
    case ""zerratar"":
      j = 2
      return True
  }
  return False
}", code);
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
}", code);
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
}", code);
        }

        [TestMethod]
        public void Transform_let_variable_bool_expression_return_variable()
        {
            var code = FormatCode("fn greet(name:string) { let isZerratar = name == \"zerratar\" return isZerratar }");
            Assert.AreEqual(
@"fn greet(name:string) -> bool {
  var isZerratar = name == ""zerratar""
  return isZerratar
}", code);
        }

        [TestMethod]
        public void Transform_return_bool_expression()
        {
            var code = FormatCode("fn greet(name:string) { return name == \"zerratar\" }");
            Assert.AreEqual(
@"fn greet(name:string) -> bool {
  return name == ""zerratar""
}", code);
        }

        [TestMethod]
        public void Declare_local_variable_inside_deep_scope_in_main()
        {
            var code = FormatCode("fn println(text:string) { } fn main() { if (true) { if (false) { let localwo = \"hello world\" if (true) { let myglobal = 0 println(localwo) println(localwo) } } } }");
            Assert.AreEqual(
@"fn println(text:string) {
}
fn main() {
  if (True) { 
    if (False) { 
      var localwo = ""hello world""
      if (True) { 
        var myglobal = 0
        println(localwo)
        println(localwo)
      }
    }
  }
}", code);
        }

        [TestMethod]
        public void Format_struct_usage_in_functions()
        {
            var code =
                FormatCode(
                    "struct testStruct { number hello } fn fun_with_struct(testStruct s) { s.hello = 9999 } fn do_stuff() { let s1 = testStruct fun_with_struct(s1) return s1.hello }");

            Assert.AreEqual(
@"struct testStruct {
  number hello
}
fn fun_with_struct(s:testStruct) {
  s.hello = 9999
}
fn do_stuff() -> any {
  var s1 = testStruct
  fun_with_struct(s1)
  return s1.hello
}", code);
        }
        [TestMethod]
        public void Invoke_function_accessing_a_struct()
        {
            var code = FormatCode("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ");
            Assert.AreEqual(
@"struct structA {
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
        public void Invoke_function_array_parameter()
        {
            var code = FormatCode("fn do_stuff(number[] numbers) { numbers[0] = 0 }");
            Assert.AreEqual(
@"fn do_stuff(numbers:number[]) {
  numbers[0] = 0
}", code);
        }

        [TestMethod]
        public void Invoke_function_accessing_a_class_array_field()
        {
            var code = FormatCode("fn do_stuff(any obj) { obj[0] = 1 return obj[0] }");
            Assert.AreEqual(
@"fn do_stuff(obj:any) -> any {
  obj[0] = 1
  return obj[0]
}", code);
        }

        [TestMethod]
        public void Invoke_function_declare_variable_with_incrementor_expression()
        {
            var code = FormatCode("fn do_stuff(number val) { let result = val++ return result }");
            Assert.AreEqual(
@"fn do_stuff(val:f64) -> number {
  var result = val++
  return result
}", code);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression()
        {
            var code = FormatCode("fn do_stuff(number val) { return val++ }");
            Assert.AreEqual(
@"fn do_stuff(val:f64) -> number {
  return val++
}", code);
        }

        [TestMethod]
        public void Invoke_get_array_item()
        {
            var code = FormatCode("let array = [] let result = array[0]");
            Assert.AreEqual(
@"var array = []
var result = array[0]", code);
        }

        [TestMethod]
        public void Invoke_push_to_array_1()
        {
            var code = FormatCode("let array = [] array.push(25) let result = array[0]");
            Assert.AreEqual(
@"var array = []
array.push(25)
var result = array[0]", code);
        }

        [TestMethod]
        public void Invoke_push_to_array_2()
        {
            var code = FormatCode("let array = [] array.push(25)");
            Assert.AreEqual(
@"var array = []
array.push(25)", code);
        }

        [TestMethod]
        public void Invoke_remove_from_array()
        {
            var code = FormatCode("let array = [] array.remove(0)");
            Assert.AreEqual(
@"var array = []
array.remove(0)", code);
        }

        [TestMethod]
        public void Invoke_push_and_remove_from_array_1()
        {
            var code = FormatCode("let array = [] array.push(25) array.remove(0)");
            Assert.AreEqual(
@"var array = []
array.push(25)
array.remove(0)", code);
        }

        [TestMethod]
        public void Invoke_push_and_remove_from_array_2()
        {
            var code = FormatCode("let array = [] array.push(10) array.push(100) array.remove(0) let result = array[0]");
            Assert.AreEqual(
   @"var array = []
array.push(10)
array.push(100)
array.remove(0)
var result = array[0]", code);
        }

        [TestMethod]
        public void Invoke_push_and_remove_from_array_3()
        {
            var code = FormatCode("let array = [] array.push(10) array.push(100) array.remove(0) let result = array.length");
            Assert.AreEqual(
   @"var array = []
array.push(10)
array.push(100)
array.remove(0)
var result = array.length", code);
        }

        [TestMethod]
        public void Invoke_push_to_array_and_get_length()
        {
            var code = FormatCode("let array = [] array.push(10) let result = array.length");
            Assert.AreEqual(
@"var array = []
array.push(10)
var result = array.length", code);
        }

        [TestMethod]
        public void Invoke_get_array_length()
        {
            var code = FormatCode("let array = [] let result = array.length");
            Assert.AreEqual(
@"var array = []
var result = array.length", code);
        }


        [TestMethod]
        public void Combine_unary_and_conditional_expression_assignment()
        {
            var code = FormatCode("fn test() -> void { let a = 0 let b = a++ > 0 }");
            Assert.AreEqual(
@"fn test() {
  var a = 0
  var b = a++ > 0
}", code);
        }


        [TestMethod]
        public void Combine_unary_and_conditional_expression_assignment_2()
        {
            var code = FormatCode("fn test() { let a = 0 let b = a++ > 0 return b }");
            Assert.AreEqual(
@"fn test() -> bool {
  var a = 0
  var b = a++ > 0
  return b
}", code);
        }

        [TestMethod]
        public void Combine_unary_and_math_expression_assignment()
        {
            var code = FormatCode("fn test() -> void { let a = 0 let b = a++ + 10 }");
            Assert.AreEqual(
@"fn test() {
  var a = 0
  var b = a++ + 10
}", code);
        }

        [TestMethod]
        public void Push_array_return_item_value_1()
        {
            var code = FormatCode(@"let console = $console
let names = []
names.push(""Daisy"")
console.log(names[0])");

            Assert.AreEqual(
@"var console = $console
var names = []
names.push(""Daisy"")
console.log(names[0])", code);
        }

        [TestMethod]
        public void Push_array_return_item_value_3()
        {
            var code = FormatCode(@"let console = $console
let names = []

names.push(""Daisy"")

for(let j = 0; j < names.length; j++) { 
   console.log(names[j]) 
}");
            Assert.AreEqual(
@"var console = $console
var names = []
names.push(""Daisy"")
for (var j = 0; j < names.length; j++) {
  console.log(names[j])
}", code);

        }


        [TestMethod]
        public void Use_bang_not_bool_invoked_clr_method()
        {
            var code = FormatCode(@"
let woot = $woot
let console = $console
if (1 == 1) console.log('1')
if (!woot.DidItWork('hello')) {
console.log('failed')
} else {
console.log('success')
}
");
            Assert.AreEqual(
@"var woot = $woot
var console = $console
if (1 == 1) { 
  console.log(""1"")
}
if (!woot.DidItWork(""hello"")) { 
  console.log(""failed"")
}
else {
  console.log(""success"")
}", code);
        }



        [TestMethod]
        public void Push_array_return_item_value_4()
        {
            var code = FormatCode(@"let console = $console

let names = []

names.push(""Daisy"")

console.log(names[0][0])");
            Assert.AreEqual(
@"var console = $console
var names = []
names.push(""Daisy"")
console.log(names[0][0])", code);
        }

        [TestMethod]
        public void Push_array_return_item_value_5()
        {
            var code = FormatCode(@"let console = $console
let names = []

names.push(""Daisy"")
let len = names.length
for(let j = 0; j < len; j++) { 
   console.log(names[j]) 
}");


            Assert.AreEqual(
@"var console = $console
var names = []
names.push(""Daisy"")
var len = names.length
for (var j = 0; j < len; j++) {
  console.log(names[j])
}", code);

        }

        [TestMethod]
        public void Push_array_return_item_value_2()
        {
            var code = FormatCode(@"let console = $console

let names = []

names.push(""Daisy"")

let j = names[0]

console.log(j)");
            Assert.AreEqual(
    @"var console = $console
var names = []
names.push(""Daisy"")
var j = names[0]
console.log(j)", code);

        }
        [TestMethod]
        public void Console_log_constant_expr()
        {
            var code = FormatCode("fn test(any console) { console.log(\"Hello World\".length) }");
            Assert.AreEqual(
    @"fn test(console:any) {
  console.log(""Hello World"".Length)
}", code);
        }

        [TestMethod]
        public void IF_else_then()
        {
            var code = FormatCode(@"let console = $console
let account = 500
fn bet(number betsum) -> void {
  if(betsum > account) {
    console.log(""sluta tro på lyyx"")
    console.log(""försök igen"")
  }
  else {
    console.log(""du har bettat "" + betsum)
  }
}
bet(498)");
            Assert.AreEqual(
    @"var console = $console
var account = 500
fn bet(betsum:f64) {
  if (betsum > account) { 
    console.log(""sluta tro på lyyx"")
    console.log(""försök igen"")
  }
  else {
    console.log(""du har bettat "" + betsum)
  }
}
bet(498)", code);

        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_1()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console

let names = [""hello""]
console.log(names[0])");
            Assert.AreEqual(
    @"var console = $console
var names = [""hello""]
console.log(names[0])", code);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console

let names = [""hello""]
let j = []
j = [""hello""]
console.log(names[0])");
            Assert.AreEqual(
    @"var console = $console
var names = [""hello""]
var j = []
j = [""hello""]
console.log(names[0])", code);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_3()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console

let names = [""hello""]
let j = []
j = []
console.log(names[0])");
            Assert.AreEqual(
    @"var console = $console
var names = [""hello""]
var j = []
j = []
console.log(names[0])", code);

        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_7()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn test(arr:any) -> void {
  console.log(arr[0])
}
let apa = ""hello""
let names = []
names = [123,apa, 521]
test(names)");
            Assert.AreEqual(
    @"var console = $console
fn test(arr:any) {
  console.log(arr[0])
}
var apa = ""hello""
var names = []
names = [123, apa, 521]
test(names)", code);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_8()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn test(arr:any) -> void {
  console.log(arr[0])
}
let apa = ""hello""
test([123,apa, 521])");


            Assert.AreEqual(
    @"var console = $console
fn test(arr:any) {
  console.log(arr[0])
}
var apa = ""hello""
test([123, apa, 521])", code);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_9()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[0])
}
let apa = ""hello""
let names = [123,apa, 521]
test(names)");


            Assert.AreEqual(
    @"var console = $console
fn test(arr:any) {
  console.log(arr[0])
}
var apa = ""hello""
var names = [123, apa, 521]
test(names)", code);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_12()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
console.log([123, 55, 521].length)");
            Assert.AreEqual(
    @"var console = $console
console.log([123, 55, 521].length)", code);
        }



        [TestMethod]
        public void Declare_variable_with_array_initializer_10()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn test(arr:any) -> void {
  console.log(arr[2])
}
let apa = ""hello""
let names = [123+2,apa, 521-500]
test(names)");
            Assert.AreEqual(
    @"var console = $console
fn test(arr:any) {
  console.log(arr[2])
}
var apa = ""hello""
var names = [123 + 2, apa, 521 - 500]
test(names)", code);
        }



        [TestMethod]
        public void Declare_variable_with_array_initializer_13()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
let names = [123, 55, 521]
names.clear()
console.log(names.length)");
            Assert.AreEqual(
    @"var console = $console
var names = [123, 55, 521]
names.clear()
console.log(names.length)", code);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_14()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"".length * 3 + 5 / 12 - 10.4)
   let cc = [""asd"",""pasd""]
   let i = 0
   foreach(var j in cc) {
       console.log(j + (i++))
       
   }
}

print_hello_world()");
            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"".Length * 3 + 5 / 12 - 10,4)
  var cc = [""asd"", ""pasd""]
  var i = 0
  foreach (var j in cc) {
    console.log(j + i++)
  }
}
print_hello_world()", code);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_15()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"".length * 3 + 5 / 12 - 10.4)
   let cc = [""asd"",""pasd""]
   let i = 0
   foreach(var j in cc) {
       console.log(j + i)
       i++
   }
}

print_hello_world()");

            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"".Length * 3 + 5 / 12 - 10,4)
  var cc = [""asd"", ""pasd""]
  var i = 0
  foreach (var j in cc) {
    console.log(j + i)
    i++
  }
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_16()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"".length * 3 + 5 / 12 - 10.4)
   let cc = [""asd"",""pasd""]
   let i = 0
   foreach(var j in cc) {
       console.log(j + (i++))
   }
}

print_hello_world()");

            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"".Length * 3 + 5 / 12 - 10,4)
  var cc = [""asd"", ""pasd""]
  var i = 0
  foreach (var j in cc) {
    console.log(j + i++)
  }
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_1()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   let b = a++
   console.log(b)
}
print_hello_world()");


            Assert.AreEqual(@"var console = $console
fn print_hello_world() {
  var a = 0
  var b = a++
  console.log(b)
}
print_hello_world()", code);
        }


        [TestMethod]
        public void Declare_variable_with_increment_assignment_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   let b = a+=2
   console.log(b)
}
print_hello_world()");


            Assert.AreEqual(@"var console = $console
fn print_hello_world() {
  var a = 0
  var b = a = a + 2
  console.log(b)
}
print_hello_world()", code);
        }


        [TestMethod]
        public void Declare_variable_with_increment_assignment_5()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"var console = $console
fn print_hello_world() {
  var a = 0
  var b = a = a + 2
  console.log(b)
}
print_hello_world()
");

            Assert.AreEqual(@"var console = $console
fn print_hello_world() {
  var a = 0
  var b = a = a + 2
  console.log(b)
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_3()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   a += 2
   let b = a
   console.log(b)
}
print_hello_world()");

            Assert.AreEqual(@"var console = $console
fn print_hello_world() {
  var a = 0
  a = a + 2
  var b = a
  console.log(b)
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_4()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = FormatCode(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   a += 2
   let b = a+=1
   console.log(b)
}
print_hello_world()");

            Assert.AreEqual(@"var console = $console
fn print_hello_world() {
  var a = 0
  a = a + 2
  var b = a = a + 1
  console.log(b)
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Assign_hexadecimal_constant_to_variable()
        {
            var code = FormatCode("let j = 0xff");
            Assert.AreEqual("var j = 255", code);
        }

        [TestMethod]
        public void Assign_return_value_of_hidden_function_to_variable_and_print_value()
        {
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
   let secret = """" + console.SecretMethod()
   console.log(secret)
}

print_hello_world()");
            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"")
  var secret = """" + console.SecretMethod()
  console.log(secret)
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Assign_return_value_of_hidden_function_to_variable_and_print_value_1()
        {
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
   let secret = console.SecretMethod() + """"
   console.log(secret)
}

print_hello_world()");

            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"")
  var secret = console.SecretMethod() + """"
  console.log(secret)
}
print_hello_world()", code);
        }

        [TestMethod]
        public void Assign_return_value_of_hidden_function_to_variable_and_print_value_4()
        {
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")   
   console.log(console.SecretMethod() + """")
}

print_hello_world()");

            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"")
  console.log(console.SecretMethod() + """")
}
print_hello_world()", code);
        }


        [TestMethod]
        public void Assign_return_value_of_hidden_function_to_variable_and_print_value_3()
        {
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
   let secret = """"
   secret = secret + console.SecretMethod()
   console.log(secret)
}

print_hello_world()");

            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"")
  var secret = """"
  secret = secret + console.SecretMethod()
  console.log(secret)
}
print_hello_world()", code);
        }


        [TestMethod]
        public void Assign_return_value_of_hidden_function_to_variable_and_print_value_2()
        {
            var code = FormatCode(@"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
   let secret = console.SecretMethod()
   console.log(secret)
}

print_hello_world()");
            Assert.AreEqual(
    @"var console = $console
fn print_hello_world() {
  console.log(""Hello World"")
  var secret = console.SecretMethod()
  console.log(secret)
}
print_hello_world()", code);
        }

        /*// variable names starting with $ 
    // are variables grabbed from an external source
    let console = $console

    fn print_hello_world() -> void {
    console.log(""Hello World"")
    let secret = """" + console.SecretMethod()
    console.log(secret)
    }

    print_hello_world()*/
        [TestMethod]
        public void Get_string_length()
        {
            var code = FormatCode("fn test() { let t =\"hello world\" let len = t.length } ");
            Assert.AreEqual(
    @"fn test() {
  var t = ""hello world""
  var len = t.length
}", code);
        }

        [TestMethod]
        public void Assign_value_to_managed_field()
        {
            var code = FormatCode("fn test(s:any) -> string { s.Test = \"HEHEHE\" return s.Test } ");
            Assert.AreEqual(
    @"fn test(s:any) -> string {
  s.Test = ""HEHEHE""
  return s.Test
}", code);
        }

        [TestMethod]
        public void Invoke_combination_of_strcat_usages()
        {
            var code = FormatCode(
        @"let console = $0 
let zerratar = ""Karl""
fn favoColor() -> string { 
    return ""green""
}

fn main() -> void {
   let anders = ""Anders""
   console.log(""Hey there "" + anders + ""!"")
   console.log(""My name is "" + zerratar + ""."")
   console.log(""My favorite color is "" + favoColor())
}

main()");
            Assert.AreEqual(
    @"var console = $0
var zerratar = ""Karl""
fn favoColor() -> string {
  return ""green""
}
fn main() {
  var anders = ""Anders""
  console.log(""Hey there "" + anders + ""!"")
  console.log(""My name is "" + zerratar + ""."")
  console.log(""My favorite color is "" + favoColor())
}
main()", code);

        }

        [TestMethod]
        public void Return_early_from_function()
        {
            var code = FormatCode("fn looper() { return -1 let j = 0 for(let i = 0; i < 10; i++) { j++ break } return j } let result = looper()");
            Assert.AreEqual(
    @"fn looper() -> number {
  return -1
  var j = 0
  for (var i = 0; i < 10; i++) {
    j++
    break
  }
  return j
}
var result = looper()", code);
        }


        [TestMethod]
        public void Break_from_do_while_loop_loop()
        {
            var code = FormatCode("fn main() { let j = 0 do { j++ loop { j-- break } } while(true) }");
            Assert.AreEqual(
    @"fn main() {
  var j = 0
  do {
    j++
    loop {
      j--
      break
    }
  } while(True)
}", code);
        }

        [TestMethod]
        public void Assign_value_to_managed_field_deep()
        {
            var code = FormatCode("fn test(any s) -> string { s.Val.Test = \"HEHEHE\" return s.Val.Test } ");
            Assert.AreEqual(
    @"fn test(s:any) -> string {
  s.Val.Test = ""HEHEHE""
  return s.Val.Test
}", code);
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

        private string FormatCode(string code)
        {
            LanguageParser parser;
            var ast = new NodeTypeBinder().Process(Reduce(code, out parser));
            var analyzer = new NodeAnalyzer();
            var analyzed = analyzer.Analyze(ast as EntryNode);
            var codeGenerator = new CodeGeneratorVisitor();
            return codeGenerator.Visit(analyzed.GetExpression()).TrimEnd('\r', '\n');
        }
    }
}