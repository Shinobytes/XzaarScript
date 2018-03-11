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

namespace Shinobytes.XzaarScript.UnitTests
{
    [TestClass]
    public class XzaarScript_DotNet_Compiler_Tests
    {


        //[TestMethod]
        //public void for_loop_simple()
        //{
        //    var del = Compile("let x = 0; for(let i = 0; i < 10; i++) { x = 1; }");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(3.0, app.math);
        //}

        //[TestMethod]
        //public void for_loop_simple_2()
        //{
        //    var del = Compile("let j = 0; for(let i = 0; i < 10; i++) { j++; }");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(3.0, app.math);
        //}


        //[TestMethod]
        //public void loop_post_increment_break_when_10()
        //{
        //    var del = Compile("let j = 0; loop { j++; if (j == 10) { break; } }");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(10, app.j);
        //}


        //[TestMethod]
        //public void loop_pre_increment_break_when_10()
        //{
        //    var del = Compile("let j = 0; loop { ++j; if (j == 10) { break; } }");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(10, app.j);
        //}



        //[TestMethod]
        //public void post_increment_test()
        //{
        //    var del = Compile("let j = 0; let i = j++;");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(1, app.i);
        //    Assert.AreEqual(1, app.j);
        //}


        //[TestMethod]
        //public void pre_increment_test()
        //{
        //    var del = Compile("let j = 0; let i = ++j;");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(1, app.i);
        //    Assert.AreEqual(1, app.j);
        //}


        //[TestMethod]
        //public void loop_break()
        //{
        //    var del = Compile("loop {  break; }");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();            
        //}


        //[TestMethod]
        //public void loop_break_assign()
        //{
        //    var del = Compile("let i = 0; loop {  break; i = 2; }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //}

        //[TestMethod]
        //public void loop_break_assign_1()
        //{
        //    var del = Compile("let i = 0; loop {  i = 2; break; }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(2, app.i);
        //}


        //[TestMethod]
        //public void Define_global_variable_assign_string_concat_number_first()
        //{
        //    var del = Compile("let i = 9 + ' test';");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("9 test", app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_string_concat_string_first()
        //{
        //    var del = Compile("let i = 'wee ' + 9;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("wee 9", app.i);
        //}

        //[TestMethod]
        //public void Define_global_string_variable_assign_string_concat_string_first()
        //{
        //    var del = Compile("let i:string = 'wee ' + 9;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("wee 9", app.i);
        //}

        //[TestMethod]
        //public void Concat_string_with_functioncall_and_number()
        //{
        //    var del = Compile("let i = test() + 9; fn test() { return 'Hello world, '; }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("Hello world, 9", app.i);
        //}

        //[TestMethod]
        //public void if_1_equals_1()
        //{
        //    var del = Compile("if (1 == 1) {}");
        //    dynamic app = del.DynamicInvoke();
        //}

        //[TestMethod]
        //public void if_1_equals_1_then_set_val()
        //{
        //    var del = Compile("let i = 0; if (1 == 1) { i = 1 }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(1, app.i);
        //}

        //[TestMethod]
        //public void if_1_equals_0_then_set_val()
        //{
        //    var del = Compile("let i = 0; if (1 == 0) { i = 1 }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //}

        //[TestMethod]
        //public void if_1_equals_0_or_1_equals_1_then_set_val()
        //{
        //    var del = Compile("let i = 0; if (1 == 0 || 1 == 1) { i = 1 }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(1, app.i);
        //}

        //[TestMethod]
        //public void if_1_equals_0_or_0_equals_3_then_set_val()
        //{
        //    var del = Compile("let i = 0; if (1 == 0 || 0 == 3) { i = 1 }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //}

        //[TestMethod]
        //public void if_1_equals_0_and_1_equals_1_then_set_val()
        //{
        //    var del = Compile("let i = 0; if (1 == 0 && 1 == 1) { i = 1 }");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_number()
        //{
        //    var del = Compile("let i:number = 0;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_string()
        //{
        //    var del = Compile("let i = '0'");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("0", app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_and_assign_null()
        //{
        //    var del = Compile("let i = null");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(null, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_boolean_true()
        //{
        //    var del = Compile("let i = true;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_boolean_false()
        //{
        //    var del = Compile("let i = false;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(false, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_1_not_equals_1()
        //{
        //    var del = Compile("let i = 1 != 1;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(false, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_1_not_equals_0()
        //{
        //    var del = Compile("let i = 1 != 0;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}


        //[TestMethod]
        //public void Define_global_variable_assign_string_1_not_equals_string_1()
        //{
        //    var del = Compile("let i = '1' != '1';");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(false, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_string_1_not_equals_string_0()
        //{
        //    var del = Compile("let i = '1' != '0';");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_1_equals_1()
        //{
        //    var del = Compile("let i = 1 == 1;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_1_equals_0()
        //{
        //    var del = Compile("let i = 1 == 0;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(false, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_1_equals_0_plus_1()
        //{
        //    var del = Compile("let i = 1 == 0 + 1;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_string_1_equals_string_1()
        //{
        //    var del = Compile("let i = '1' == '1';");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(true, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_assign_string_1_equals_string_0()
        //{
        //    var del = Compile("let i = '1' == '0';");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(false, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_and_assign_string_then_assign_null()
        //{
        //    var del = Compile("let i = 'hello world'; i = null");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(null, app.i);
        //}

        //[TestMethod]
        //public void Define_global_variable_string_then_assign()
        //{
        //    var del = Compile("let i; i = 'japa'");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("japa", app.i);
        //}

        //[TestMethod]
        //public void Define_two_global_variable_string_then_assign_varA_with_varB()
        //{
        //    var del = Compile("let a; let b = 'hello'; a = b");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("hello", app.a);
        //    Assert.AreEqual("hello", app.b);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(MissingMemberException))]
        //public void Define_two_globals_with_varB_assigned_with_varA_before_varA_is_defined_throws()
        //{
        //    var del = Compile("let b = a; let a;");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual("hello", app.a);
        //    Assert.AreEqual("hello", app.b);
        //}


        //[TestMethod]
        //public void Define_global_variable_multiple()
        //{
        //    var del = Compile("let i:number = 0; let j = 'hehehe'");
        //    dynamic app = del.DynamicInvoke();
        //    Assert.AreEqual(0, app.i);
        //    Assert.AreEqual("hehehe", app.j);
        //}

        //[TestMethod]
        //public void Define_function_no_params_returns_void()
        //{
        //    var del = Compile("fn test() { }");
        //    dynamic app = del.DynamicInvoke();
        //    app.test();
        //}

        //[TestMethod]
        //public void Define_function_no_params_returns_void_calls_other_function()
        //{
        //    var del = Compile("fn test() { test2(); } fn test2() { }");
        //    dynamic app = del.DynamicInvoke();
        //    app.test();
        //}

        //[TestMethod]
        //public void Define_function_return_function_call()
        //{
        //    var del = Compile("fn test() { return test2(); } fn test2() { return 42.0; }");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();

        //    Assert.AreEqual(42.0, result);
        //}

        //[TestMethod]
        //public void Define_function_no_params_one_local_returns_string()
        //{
        //    var del = Compile("fn test() { let a = 'test'; return a; }");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();

        //    Assert.AreEqual("test", result);
        //}

        //[TestMethod]
        //public void Define_and_invoke_function_with_no_params_returns_void()
        //{
        //    var del = Compile("fn test() { } test();");
        //    dynamic app = del.DynamicInvoke();
        //    app.test();
        //}

        //[TestMethod]
        //public void Invoke_then_define_function_with_no_params_returns_void()
        //{
        //    var del = Compile("test(); fn test() { } ");
        //    dynamic app = del.DynamicInvoke();
        //    app.test();
        //}


        //[TestMethod]
        //public void Define_function_and_invoke_return_string()
        //{
        //    var del = Compile("fn test() { return 'hello world'; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual("hello world", result);
        //}


        //[TestMethod]
        //public void Define_function_and_invoke_return_arithmetic_expression()
        //{
        //    var del = Compile("fn test() { return 4+5; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual(9, result);
        //}

        //[TestMethod]
        //public void Define_function_and_invoke_return_arithmetic_expression_2()
        //{
        //    var del = Compile("fn test() { return 4+5*2; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual(14, result);
        //}

        //[TestMethod]
        //public void Define_function_and_invoke_return_arithmetic_expression_with_function_call_unknown_returntype()
        //{
        //    var del = Compile("fn test() { return 4 + 5 * test3(); } fn test3() { return 2; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual(14, result);
        //}

        //[TestMethod]
        //public void Define_function_and_invoke_return_arithmetic_expression_with_function_call()
        //{
        //    var del = Compile("fn test() -> number { return 4 + 5 * test3(); } fn test3() { return 2; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual(14, result);
        //}

        //[TestMethod]
        //public void Define_function_and_invoke_return_arithmetic_expression_with_function_call_2()
        //{
        //    var del = Compile("fn test() -> number { return 4 + 5 * test3(); } fn test3() -> number { return 2; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual(14, result);
        //}


        //[TestMethod]
        //public void Define_function_and_invoke_return_field()
        //{
        //    var del = Compile("let j = 'hello world'; fn test() { return j; } ");
        //    dynamic app = del.DynamicInvoke();
        //    var result = app.test();
        //    Assert.AreEqual("hello world", result);
        //}

        //[TestMethod]
        //public void Arithmetic_1_plus_2()
        //{
        //    var del = Compile("let math = 1 + 2");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(3.0, app.math);
        //}


        //[TestMethod]
        //public void Arithmetic_1_plus_3_minus_5()
        //{
        //    var del = Compile("let math = 1 + 3 - 5");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(-1.0, app.math);
        //}

        //[TestMethod]
        //public void Arithmetic_1_plus_3_multiply_2_minus_5()
        //{
        //    var del = Compile("let math = 1 + 3 * 2 - 5");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(2, app.math);
        //}


        //[TestMethod]
        //public void Arithmetic_1_plus_3_multiply_2_minus_5_divide_2()
        //{
        //    var del = Compile("let math = 1 + 3 * 2 - 5 / 2");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(4.5, app.math);
        //}


        //[TestMethod]
        //public void Arithmetic_grouped_1_plus_3_then_multiply_2_minus_5_then_divide_2()
        //{
        //    // NOTE: This will fail for now, but let it be a reminder that we need to remove our initial block parsing because our expression ends at the first ')' end_of_group
        //    //       this breaks the rest of the expression because "math" will be assigned "1 + 3", and then we have an arithmetic '*' operator that cannot be handled.
        //    //       making the whole expression into a group will fix the issue as seen in a test below ( Arithmetic_second_grouped_1_plus_3_then_multiply_2_minus_5_then_divide_2 )
        //    var del = Compile("let math = (1 + 3) * (2 - 5) / 2");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(-6, app.math);
        //}


        //[TestMethod]
        //public void Arithmetic_second_grouped_1_plus_3_then_multiply_2_minus_5_then_divide_2()
        //{
        //    var del = Compile("let math = ((1 + 3) * (2 - 5) / 2)");
        //    dynamic app = del.DynamicInvoke();
        //    // var result = app.test();
        //    Assert.AreEqual(-6, app.math);
        //}


        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void Define_same_function_twice_throws_exception()
        //{
        //    var del = Compile("fn test() { } fn test() { }");
        //    dynamic app = del.DynamicInvoke();
        //    app.test();
        //}

        ////[TestMethod]
        ////[ExpectedException(typeof(Exception))]
        ////public void Define_same_variable_twice_throws_exception()
        ////{
        ////    var del = Compile("let i = 0; let i = 0;");
        ////    dynamic app = del.DynamicInvoke();
        ////}

        //private Delegate Compile(string inputCode)
        //{
        //    return inputCode
        //        .Tokenize()
        //        .Parse()
        //        .AnalyzeExpression()
        //        .CompileToDotNet();
        //}
    }
}