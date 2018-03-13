/* 
 *	 This file is part of XzaarScript.
 *	 Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 *	 Permission is hereby granted, free of charge, to any person obtaining a copy
 *	 of this software and associated documentation files (the "Software"), to deal
 *	 in the Software without restriction, including without limitation the rights
 *	 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	 copies of the Software, and to permit persons to whom the Software is
 *	 furnished to do so, subject to the following conditions:
 *	 
 *	 The above copyright notice and this permission notice shall be included in
 *	 all copies or substantial portions of the Software.
 *	 
 *	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *	 THE SOFTWARE.  
 **/
 
using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Assembly.Models;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_VM_Tests
    {
        [TestMethod]
        public void Invoke_function_with_any_parameter()
        {
            var rt = Run("fn test(any a) { return a } let result = test(10) ");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(10, j);
        }

        [TestMethod]
        public void Invoke_simple_addition()
        {
            var rt = Run("let result = 1 + 1");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(2, j);
        }

        [TestMethod]
        public void Invoke_push_to_array_1()
        {
            var rt = Run("let array = [] array.push(25)");
            var j = rt.GetVariableValue<object[]>("array");
            Assert.AreEqual(j[0] + "", "25");
        }

        [TestMethod]
        public void Invoke_push_to_array_2()
        {
            var rt = Run("let array = [] array.push(25) let result = array[0]");
            var j = rt.GetVariableValue<object>("result");
            Assert.AreEqual(25, j);
        }

        [TestMethod]
        public void Invoke_push_and_remove_from_array()
        {
            var rt = Run("let array = [] array.push(10) array.push(100) array.remove(0) let result = array[0]");
            var j = rt.GetVariableValue<object>("result");
            Assert.AreEqual(100, j);
        }

        [TestMethod]
        public void Invoke_push_to_array_and_get_length()
        {
            var rt = Run("let array = [] array.push(10) let result = array.length");
            var j = rt.GetVariableValue<object>("result");
            Assert.AreEqual(1, j);
        }

        [TestMethod]
        public void Invoke_simple_mathematical_expression()
        {
            var rt = Run("let result = 5 + (1 * 1) - 992");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(5 + (1 * 1) - 992, j);
        }

        [TestMethod]
        public void Invoke_string_variable()
        {
            var rt = Run("let result = \"hello world\"");
            var j = rt.GetVariableValue<string>("result");
            Assert.AreEqual("hello world", j);
        }

        [TestMethod]
        public void Invoke_string_variable_strcat()
        {
            var rt = Run("let result = \"I'm \" + 27 + \" years old\"");
            var j = rt.GetVariableValue<string>("result");
            Assert.AreEqual("I'm 27 years old", j);
        }

        [TestMethod]
        public void Invoke_string_variable_strcat_using_variables_1()
        {
            var rt = Run("let a = \"I'm \" let result = a + 27 + \" years old\"");
            var j = rt.GetVariableValue<string>("result");
            Assert.AreEqual("I'm 27 years old", j);
        }

        [TestMethod]
        public void Invoke_string_variable_strcat_using_variables_2()
        {
            var rt = Run("let a = \"I'm \" let b = \" years old\" let result = a + 27 + b");
            var j = rt.GetVariableValue<string>("result");
            Assert.AreEqual("I'm 27 years old", j);
        }

        [TestMethod]
        public void Invoke_string_variable_strcat_using_variables_and_function_call()
        {
            var rt = Run("fn yearsOld(number years) { return years + \" years old\" } let a = \"I'm \" let b = \" years old\" let result = a + yearsOld(27)");
            var j = rt.GetVariableValue<string>("result");
            Assert.AreEqual("I'm 27 years old", j);
        }


        [TestMethod]
        public void Invoke_function_with_loop_1()
        {
            var rt = Run("fn looper() { let j = 0 loop { ++j if (j > 10) { break } } return j } let result = looper()");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(11, j);
        }

        [TestMethod]
        public void Invoke_function_with_loop_2()
        {
            var rt = Run("fn looper() { let j = 0 loop { j++ if (j > 10) { break } } return j } let result = looper()");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(11, j);
        }


        [TestMethod]
        public void Invoke_function_with_for_loop()
        {
            var rt = Run("fn looper() { let j = 0 for(let i = 0; i < 10; i++) { j++ } return j } let result = looper()");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(10, j);
        }

        [TestMethod]
        public void Invoke_function_with_controlled_for_loop()
        {
            var rt = Run("fn looper() { let j = 0 for(let i = 0; i < 10; i++) { j++ break } return j } let result = looper()");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(1, j);
        }

        [TestMethod]
        public void Invoke_function_with_return_early()
        {
            var rt = Run("fn looper() { return -1 let j = 0 for(let i = 0; i < 10; i++) { j++ break } return j } let result = looper()");
            var j = rt.GetVariableValue<int>("result");
            Assert.AreEqual(-1, j);
        }

        [TestMethod]
        public void Invoke_function_dynamically()
        {
            var rt = Run("fn dynamic() { return \"hello world\" }");
            var result = rt.Invoke<string>("dynamic");
            Assert.AreEqual("hello world", result);
        }

        [TestMethod]
        public void Invoke_function_dynamically_with_args_1()
        {
            var rt = Run("fn greet(string name) { return \"hello \" + name }");
            var result = rt.Invoke<string>("greet", "zerratar");
            Assert.AreEqual("hello zerratar", result);
        }

        [TestMethod]
        public void Invoke_function_dynamically_with_args_2()
        {
            var rt = Run("fn greet(string name) { let isZerratar = name == \"zerratar\" return isZerratar }");
            var result = rt.Invoke<bool>("greet", "zerratar");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Invoke_function_dynamically_with_args_3()
        {
            var rt = Run("fn greet(string name) { let j = 0 switch(name) { case \"zerratar\": j = 2 return true } return false }");
            var result = rt.Invoke<bool>("greet", "zerratar");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Invoke_function_dynamically_with_args_4()
        {
            var rt = Run("fn greet(string name) { let j = 0 switch(name) { case \"zerratar\": return true } return false }");
            var result = rt.Invoke<bool>("greet", "zerratar");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Invoke_function_accessing_a_struct_1()
        {
            var rt = Run("struct testStruct { number hello } fn fun_with_struct(testStruct s) { s.hello = 9999 } fn do_stuff() { let s1 = testStruct fun_with_struct(s1) return s1.hello }");
            var result = rt.Invoke<int>("do_stuff");
            Assert.AreEqual(9999, result);
        }

        [TestMethod]
        public void Invoke_function_accessing_a_struct_2()
        {
            var rt = Run("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ");
            var result = rt.Invoke<object>("do_stuff");
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(typeof(object[]), result.GetType());

            for (int index = 0; index < ((object[])result).Length; index++)
            {
                var item = (int)Convert.ChangeType(((object[])result)[index], typeof(int));
                if (!item.Equals(index)) Assert.Fail(item + " != " + index);
            }
        }

        [TestMethod]
        public void Invoke_function_accessing_a_class_array_field()
        {
            var rt = Run("fn do_stuff(any obj) { obj[0] = 1 return obj[0] }");
            var result = rt.Invoke<int>("do_stuff", new ClassWithArray());
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_conditional_expression_assignment()
        {
            var rt = Run("fn test() -> void { let a = 0 let b = a++ > 0 }");
            var result = rt.Invoke<object>("test");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_conditional_expression_assignment_2()
        {
#warning Known bug: using a post increment/decrement inside an expression will assign the value before use
            // TODO: fix post increment inside expressions to only be invoked after the actual expression
            var rt = Run("fn test() { let a = 0 let b = a++ > 0 return b }");
            var result = rt.Invoke<object>("test");
            if (false != (bool)result)
            {
                Assert.Inconclusive("TODO: fix post increment inside expressions to only be invoked after the actual expression, this test will keep failing until its been fixed.");
            }
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_conditional_expression_assignment_3()
        {
            var rt = Run("fn test() { let a = 0 let b = ++a > 0 return b }");
            var result = rt.Invoke<object>("test");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_conditional_expression_assignment_4()
        {
            var rt = Run("fn test() { let a = 1 let b = a++ > 0 return b }");
            var result = rt.Invoke<object>("test");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_math_expression_assignment()
        {
            var rt = Run("fn test() { let a = 0 let b = a++ + 10 }");
            var result = rt.Invoke<object>("test");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Invoke_Combine_unary_and_math_expression_assignment_return_result()
        {
            var rt = Run("fn test() { let a = 0 let b = a++ + 10 return b }");
            var result = rt.Invoke<int>("test");
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Invoke_function_declare_variable_with_incrementor_expression()
        {
            var rt = Run("fn do_stuff(number val) { let result = val++ return result }");
            var result = rt.Invoke<int>("do_stuff", 1);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression()
        {
            var rt = Run("fn do_stuff(number val) { return val++ }");
            var result = rt.Invoke<int>("do_stuff", 1);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression_2()
        {
            var rt = Run("fn do_stuff(number val) { return ++val }");
            var result = rt.Invoke<int>("do_stuff", 1);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression_3()
        {
            var rt = Run("fn do_stuff_2(number v2) -> number { return v2 } fn do_stuff(number val) { let a = do_stuff_2(val++) return a }");
            var result = rt.Invoke<int>("do_stuff", 1);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression_4()
        {
            var rt = Run("fn do_stuff_2(number v2) -> number { return v2 } fn do_stuff(number val) { let a = do_stuff_2(val++) return val }");
            var result = rt.Invoke<int>("do_stuff", 1);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_struct_as_argument()
        {
            var rt = Run("fn test(any s) -> string { return s.Test } ");
            var result = rt.Invoke<string>("test", new TestStruct { Test = "Hello World" });
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_execute_function()
        {
            var rt = Run("fn test(any s) -> string { s.TestMethod() return s.Test } ");
            var result = rt.Invoke<string>("test", new TestClass());
            Assert.AreEqual("Test Method Executed!", result);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_execute_function_with_xzaar_object()
        {
            var rt = Run("fn test(any s) -> string { s.Param1(\"hello there\") return s.Test } ");
            var result = rt.Invoke<string>("test", new TestClass());
            Assert.AreEqual("hello there", result);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_execute_function_with_xzaar_object_and_return_result()
        {
            var rt = Run("fn test(any s) -> any { return s.Result1(\"hello there\") } ");
            var result = rt.Invoke<string>("test", new TestClass());
            Assert.AreEqual("hello there", result);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_update_managed_field()
        {
            var rt = Run("fn test(any s) -> string { s.Test = \"HEHEHE\" return s.Test } ");
            var result = rt.Invoke<string>("test", new TestClass());
            Assert.AreEqual("HEHEHE", result);
        }

        [TestMethod]
        public void Invoke_function_using_a_complex_clr_struct_change_is_not_kept()
        {
            // TODO: Fix! Obviously, we want to keep accessing the same struct and not have multiple instances. goshijollimosh!
            // the following test will fail to return the value "HEHEHE" because its not returning the value
            // from the same struct anymore.
            var rt = Run("fn test(any s) -> string { s.Val.Test = \"HEHEHE\" return s.Val.Test } ");
            var result = rt.Invoke<string>("test", new DeepStruct());
            Assert.AreEqual(null, result);
        }


        [TestMethod]
        public void Push_array_return_item_value_1()
        {
            var rt = Load(@"let console = $console

let names = []

names.push(""Daisy"")

console.log(names[0])");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("$console", cw);
            rt.Run();
            Assert.AreEqual("Daisy\r\n", cw.Output);
        }

        [TestMethod]
        public void Push_array_return_item_value_2()
        {
            var rt = Load(@"let console = $console

let names = []

names.push(""Daisy"")

let j = names[0]

console.log(j)");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("Daisy\r\n", cw.Output);
        }


        [TestMethod]
        public void Push_array_return_item_value_3()
        {
            var rt = Load(@"let console = $console
let names = []

names.push(""Daisy"")

for(let j = 0; j < names.length; j++) { 
   console.log(names[j]) 
}");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("Daisy\r\n", cw.Output);

        }

        [TestMethod]
        public void Push_array_return_item_value_4()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var rt = Load(@"let console = $console

let names = []

names.push(""Daisy"")

console.log(names[0][0])");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("$console", cw);
            rt.Run();
            Assert.AreEqual("D\r\n", cw.Output);
        }

        [TestMethod]
        public void Push_array_return_item_value_5()
        {
            var rt = Load(@"let console = $console
let names = []

names.push(""Daisy"")
let len = names.length
for(let j = 0; j < len; j++) { 
   console.log(names[j]) 
}");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("Daisy\r\n", cw.Output);

        }
        [TestMethod]
        public void Push_array_return_item_value_6()
        {
            var rt = Load(@"let console = $console

let names = []

names.push(""Daisy""[0])

for(let j = 0; j < names.length; j++) { 
   console.log(names[j]) 
}");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("D\r\n", cw.Output);

        }
        [TestMethod]
        public void Push_array_return_item_value_7()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var rt = Load(@"let console = $console

let names = []

names.push(""Daisy"")

console.log(names[0][1])");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("$console", cw);
            rt.Run();
            Assert.AreEqual("a\r\n", cw.Output);
        }
        [TestMethod]
        public void Push_array_return_item_value_8()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var rt = Load(@"let console = $console

let names = [""Daisy"",""Friday""]

console.log(names[1][0])");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("$console", cw);
            rt.Run();
            Assert.AreEqual("F\r\n", cw.Output);
        }
        [TestMethod]
        public void Declare_variable_with_array_initializer_1()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console

let names = [""hello""]
console.log(names[0]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console

let names = [123,521]
console.log(names[1]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("521\r\n", cw.Output);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_3()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console

let names = [""hello""]
let j = []
j = [""hello""]
console.log(names[0]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_4()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console

let names = [123,""hello"", 521]
console.log(names[1]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_5()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let apa = ""hello""
let names = [123,apa, 521]
console.log(names[1]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);

        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_6()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let apa = ""hello""
let names = []
names = [123,apa, 521]
console.log(names[1]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_7()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[1])
}
let apa = ""hello""
let names = []
names = [123,apa, 521]
test(names) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("hello\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_8()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[0])
}
let apa = ""hello""
test([123,apa, 521]) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("123\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_9()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[2])
}
let apa = ""hello""
let names = [123,apa, 521]
test(names) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("521\r\n", cw.Output);
        }



        [TestMethod]
        public void Declare_variable_with_array_initializer_10()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[2])
}
let apa = ""hello""
let names = [123+2,apa, 521-500]
test(names) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("21\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_11()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let names = [123, 55, 521]
console.log(names.length) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("3\r\n", cw.Output);
        }



        [TestMethod]
        public void Declare_variable_with_array_initializer_13()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let names = [123, 55, 521]
names.clear()
console.log(names.length) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("0\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_14()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let names = [123, 55, 521]
let res = names.indexof(55)
console.log(res) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("1\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_15()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
let names = [123, 55, ""asd""]
let res = names.indexof(""asd"")
console.log(res) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("2\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_12()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
console.log([123, 55, 521].length) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("3\r\n", cw.Output);
        }

        [TestMethod]
        public void IF_else_then_1()
        {
            var rt = Load(@"let console = $console
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
bet(498)
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("du har bettat 498\r\n", cw.Output);

        }


        [TestMethod]
        public void IF_else_then_2()
        {
            var rt = Load(@"let console = $console
let account = 500
fn bet(number betsum) -> void {
  if(betsum > account) {
    console.log(""sluta tro på lyyx"")
    console.log(""försök igen"")
  }
  else if(betsum == account) {
    console.log(""du har bettat allt du har.. "" + betsum)
  }
  else {
    console.log(""du har bettat "" + betsum)  
  }
}
bet(498)
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("du har bettat 498\r\n", cw.Output);

        }

        [TestMethod]
        public void IF_else_then_3()
        {
            var rt = Load(@"let console = $console
let account = 500
fn bet(number betsum) -> void {
  if(betsum > account) {
    console.log(""sluta tro på lyyx"")
    console.log(""försök igen"")
  }
  else if(betsum == account) {
    console.log(""du har bettat allt du har.. "" + betsum)
  }
  else {
    console.log(""du har bettat "" + betsum)  
  }
}
bet(500)
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("du har bettat allt du har.. 500\r\n", cw.Output);

        }


        [TestMethod]
        public void IF_else_then_4()
        {
            var rt = Load(@"let console = $console
let account = 500
fn bet(number betsum) -> void {
  if(betsum > account) {
    console.log(""sluta tro på lyyx"")
    console.log(""försök igen"")
  }
  else if(betsum == account) {
    console.log(""du har bettat allt du har.. "" + betsum)
  }
  else {
    console.log(""du har bettat "" + betsum)  
  }
}
bet(501)
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("sluta tro på lyyx\r\nförsök igen\r\n", cw.Output);

        }



        [TestMethod]
        public void Declare_variable_with_array_initializer_16()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"// variable names starting with $ 
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

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("23,0166666666667\r\nasd1\r\npasd2\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_array_initializer_17()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"// variable names starting with $ 
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

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("23,0166666666667\r\nasd0\r\npasd1\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_array_initializer_18()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"// variable names starting with $ 
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

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("23,0166666666667\r\nasd1\r\npasd2\r\n", cw.Output);
        }



        [TestMethod]
        public void Invoke_managed_speed_test_3()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn test(any arr) -> void {
  console.log(arr[1])
}
let apa = ""hello""
let names = []
names = [123,apa, 521]
test(names) 
");
            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                cw.Output = "";
                rt.Run();
                Assert.AreEqual("hello\r\n", cw.Output);
            }
            sw.Stop();
            var elapsedTime = sw.ElapsedMilliseconds;
            Assert.IsTrue(elapsedTime < 1000, "We were unable to run this code 10000 times under a second :(.. Elasped: " + elapsedTime);

            System.Diagnostics.Trace.WriteLine(elapsedTime + "ms for 10k runs.");
        }

        [TestMethod]
        public void Invoke_managed_speed_test()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"$console.log(""hello"")");

            Stopwatch sw = new Stopwatch();
            var cw2 = new ConsoleWrapperTest(rt);
            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                cw2.Output = "";
                cw2.log("hello");
                Assert.AreEqual("hello\r\n", cw2.Output);
            }
            sw.Stop();
            var managedElapsed = (double)sw.ElapsedMilliseconds;

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);

            sw.Restart();
            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                cw.Output = "";
                rt.Run();
                Assert.AreEqual("hello\r\n", cw.Output);
            }
            sw.Stop();
            var elapsedTime = (double)sw.ElapsedMilliseconds;
            // Assert.IsTrue(elapsedTime < 1000, "We were unable to run this code 10000 times under a second :(.. Elasped: " + elapsedTime);
            var proc = elapsedTime / managedElapsed;
            // Assert.IsTrue(proc < 100, "Seem like we are running " + proc + "x slower than managed code.");


            System.Diagnostics.Trace.WriteLine(proc + "x slower than managed code.");
        }

        [TestMethod]
        public void Invoke_managed_speed_test_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"for(let j = 0; j < 10000; j++) { $console.Output = """" $console.log(""hello"") }");

            Stopwatch sw = new Stopwatch();
            var cw2 = new ConsoleWrapperTest(rt);
            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                cw2.Output = "";
                cw2.log("hello");
                Assert.AreEqual("hello\r\n", cw2.Output);
            }
            sw.Stop();
            var managedElapsed = (double)sw.ElapsedMilliseconds;

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);

            sw.Restart();
            sw.Start();


            rt.Run();

            sw.Stop();
            var elapsedTime = (double)sw.ElapsedMilliseconds;
           // Assert.IsTrue(elapsedTime < 1000, "We were unable to run this code 10000 times under a second :(.. Elasped: " + elapsedTime);
            var proc = elapsedTime / managedElapsed;
           // Assert.IsTrue(proc < 100, "Seem like we are running " + proc + "x slower than managed code.");

            System.Diagnostics.Trace.WriteLine(proc + "x slower than managed code. (Elapsed: " + elapsedTime + "ms)");
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_1()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   let b = a++
   console.log(b)
}
print_hello_world()");

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("0\r\n", cw.Output);
        }


        [TestMethod]
        public void Declare_variable_with_increment_assignment_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   let b = a+=2
   console.log(b)
}
print_hello_world()");

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("2\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_3()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   a += 2
   let b = a+=1
   console.log(b)
}
print_hello_world()");

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("3\r\n", cw.Output);
        }

        [TestMethod]
        public void Declare_variable_with_increment_assignment_4()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var rt = Load(@"let console = $console
fn print_hello_world() -> void {
   let a = 0
   a += 2   
   console.log(a)
}
print_hello_world()");

            var cw = new ConsoleWrapperTest(rt);
            rt.RegisterGlobalVariable("console", cw);
            rt.Run();
            Assert.AreEqual("2\r\n", cw.Output);
        }


        [TestMethod]
        public void Invoke_function_using_a_complex_clr_class_value_change_is_kept()
        {
            var rt = Run("fn test(any s) -> string { s.Val.Test = \"HEHEHE\" return s.Val.Test } ");
            var result = rt.Invoke<string>("test", new DeepClass());
            Assert.AreEqual("HEHEHE", result);
        }

        [TestMethod]
        public void Console_log_constant_expr()
        {
            var rt = Load("fn test(any console) { console.log(\"Hello World\".length) }");
            var console = new ConsoleWrapperTest(rt);
            var result = rt.Invoke<string>("test", console);
            Assert.AreEqual("11\r\n", console.Output);
        }

        [TestMethod]
        public void Invoke_combination_of_strcat_usages()
        {
            var rt = Load(@"let console = $0 

fn favoColor() -> string { 
    return ""green""
}

fn main() -> void {
   let zerratar = ""Karl""
   let anders = ""Anders""
   console.log(""Hey there "" + anders + ""!"")
   console.log(""My name is "" + zerratar + ""."")
   console.log(""My favorite color is "" + favoColor())
}

main()");
            var console = new ConsoleWrapperTest(rt);

            rt.Invoke(console);
            Assert.AreEqual(@"Hey there Anders!
My name is Karl.
My favorite color is green
", console.Output);
        }


        [TestMethod]
        public void Invoke_combination_of_strcat_usages_1()
        {
            var rt = Load(@"let console = $0 
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
            var console = new ConsoleWrapperTest(rt);

            rt.Invoke(console);
            Assert.AreEqual(@"Hey there Anders!
My name is Karl.
My favorite color is green
", console.Output);
        }



        [TestMethod]
        public void Invoke_combination_of_strcat_usages_2()
        {
            var rt = Load(@"
fn favoColor() -> string { 
    return ""green""
}

fn main() -> void {
   let zerratar = ""Karl""
   let anders = ""Anders""
   $0.log(""Hey there "" + anders + ""!"")
   $0.log(""My name is "" + zerratar + ""."")
   $0.log(""My favorite color is "" + favoColor())
}

main()");
            var console = new ConsoleWrapperTest(rt);

            rt.Invoke(console);
            Assert.AreEqual(@"Hey there Anders!
My name is Karl.
My favorite color is green
", console.Output);
        }


        private XzaarRuntime Run(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile()
                .Run();
        }


        private XzaarRuntime Load(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile()
                .Runtime();
        }
    }

    public class ConsoleWrapperTest
    {
        private readonly XzaarRuntime rt;
        public string Output;

        public ConsoleWrapperTest(XzaarRuntime rt)
        {
            this.rt = rt;
        }
        public void log(object value)
        {
            Output += value + Environment.NewLine;
        }
    }

    public class TestClass
    {
        public string Test;

        public void TestMethod()
        {
            this.Test = "Test Method Executed!";
        }

        public void Param1(object objParam)
        {
            this.Test = objParam + "";
        }

        public string Result1(object objParam)
        {
            return objParam + "";
        }
    }

    public class ClassWithArray
    {
        public int[] Items = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    }

    public class DeepClass
    {
        public TestClass Val = new TestClass();
    }

    public struct TestStruct
    {
        public string Test;
    }

    public struct DeepStruct
    {
        public TestStruct Val;
    }
}
