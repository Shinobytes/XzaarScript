//using System;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Shinobytes.XzaarScript.Ast;
//using Shinobytes.XzaarScript.Ast.Compilers.Linq;

//namespace Shinobytes.XzaarScript.Interpreter.Tests
//{
//    [TestClass]
//    public class XzaarScript_Compile_LambdaExpression_Tests
//    {
//        [TestMethod]
//        public void TransformTest_if_else_then_with_bodies()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("if (a == 0) { a = 3 } else { a = 4 }");
//            var ast = parser.Parse(tokens);
//            ast = transformer.Transform(ast);


//            compiler.RegisterVariable<int>("a");

//            var expression = compiler.Compile(ast) as LambdaXzaarCompiledScript;

//            var str = expression.GetLambdaExpression().ToString();

//            Assert.AreEqual("a => IIF((a == 0), (a = 3), (a = 4))", str);
//        }

//        [TestMethod]
//        public void TransformTest_if_else_then_without_bodies_semicolon_separated()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("if (a == 0) a = 3; else a = 4;");
//            var ast = parser.Parse(tokens);
//            ast = transformer.Transform(ast);


//            compiler.RegisterVariable<int>("a");

//            var expression = compiler.Compile(ast) as LambdaXzaarCompiledScript;

//            var str = expression.GetLambdaExpression().ToString();

//            Assert.AreEqual("a => IIF((a == 0), (a = 3), (a = 4))", str);
//        }

//        [TestMethod]
//        public void TransformTest_if_else_then_with_bodies_semicolon_separated_2()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("if (a == 0) { a = 3; b = 8; } else { a = 4; }");
//            var ast = parser.Parse(tokens);
//            ast = transformer.Transform(ast);

//            compiler.RegisterVariable<int>("a");
//            compiler.RegisterVariable<int>("b");

//            var expression = compiler.Compile(ast) as LambdaXzaarCompiledScript;

//            var expr = expression.GetLambdaExpression();

//            var str = expr.ToString();

//            Assert.AreEqual("(a, b) => IIF((a == 0), { ... }, (a = 4))", str);
//        }

//        [TestMethod]
//        public void TransformTest_if_else_then_without_bodies()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("if (a == 0) a = 3 else a = 4");
//            var ast = parser.Parse(tokens);
//            ast = transformer.Transform(ast);


//            compiler.RegisterVariable<int>("a");

//            var expression = compiler.Compile(ast) as LambdaXzaarCompiledScript;

//            var str = expression.GetLambdaExpression().ToString();

//            Assert.AreEqual("a => IIF((a == 0), (a = 3), (a = 4))", str);
//        }

//        private static string theString = null;
//        public static void WriteToString(string text)
//        {
//            theString = text;
//            Console.WriteLine(text);
//        }


//        [TestMethod]
//        public void Function_main_hello_world()
//        {
//            theString = null;
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();



//            var tokens = lexer.Tokenize("extern fn println(string text);" +
//                                        "fn main() { println(\"hello world \" + 24); }"); //
//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterExternFunction("println", this.GetType().GetMethod("WriteToString", new[] { typeof(string) }), XzaarScope.Global); // typeof(Console) ... WriteLine

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;

//            var lambdaExpression = expression.GetLambdaExpression();

//            // since we return one function, our output will be an Action (extern ones are not included)

//            var result = expression.Invoke<Action>();

//            if (result != null)
//            {
//                result();
//            }

//            Assert.AreEqual("hello world 24", theString);
//            Assert.AreNotEqual(null, lambdaExpression);
//        }

//        //[TestMethod]
//        //public void Function_main_hello_world_2()
//        //{
//        //    theString = null;
//        //    var lexer = new XzaarExpressionLexer();
//        //    var parser = new XzaarExpressionParser();
//        //    var transformer = new XzaarExpressionTransformer();
//        //    var compiler = new XzaarExpressionCompiler();



//        //    var tokens = lexer.Tokenize("extern fn println(string text);" +
//        //                                "fn test(string text) { println(text); } " +
//        //                                "fn main() { test(\"hello world \" + 24); } "); //
//        //    var ast0 = parser.Parse(tokens);
//        //    var ast1 = transformer.Transform(ast0);

//        //    compiler.RegisterExternFunction("println", this.GetType().GetMethod("WriteToString", new[] { typeof(string) })); // typeof(Console) ... WriteLine

//        //    var expression = compiler.Compile(ast)  as LambdaXzaarExpression;

//        //    var lambdaExpression = expression.GetLambdaExpression();

//        //    // since we return one function, our output will be an Action (extern ones are not included)

//        //    var result = expression.Invoke<Action>();

//        //    if (result != null)
//        //    {
//        //        result();
//        //    }
//        //    Assert.AreEqual("hello world 24", theString);
//        //    Assert.AreNotEqual(null, lambdaExpression);
//        //}



//        // v-- works but not necessary needs to be run. 

//        //[TestMethod]
//        //public void BuildTest()
//        //{
//        //    var builder = new XzaarExpressionClrAssemblyBuilder();
//        //    var lexer = new XzaarExpressionLexer();
//        //    var parser = new XzaarExpressionParser();
//        //    var transformer = new XzaarExpressionTransformer();
//        //    var compiler = new XzaarExpressionCompiler();

//        //    var tokens = lexer.Tokenize("extern fn println(string text);" +
//        //                                "fn main() { if (true) { println(\"hello world \" + 24); } }"); //
//        //    var ast0 = parser.Parse(tokens);
//        //    var ast1 = transformer.Transform(ast0);

//        //    compiler.RegisterExternFunction("println", typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }));

//        //    var expression = compiler.Compile(ast)  as LambdaXzaarExpression;

//        //    var lambdaExpression = expression.GetLambdaExpression();

//        //    var asm = builder.Build("c:\\", "MyScript", lambdaExpression);
//        //}


//        [TestMethod]
//        public void Compile_value_lt_1_mul_2()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("(apa == 8) || (apa < (1 * 2))");
//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterVariable<int>("apa");

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;

//            Assert.IsTrue(expression.Invoke<bool>(0));
//            Assert.IsTrue(expression.Invoke<bool>(8));
//            Assert.IsFalse(expression.Invoke<bool>(10));
//        }

//        [TestMethod]
//        public void Compile_value_mul_10()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("apa * 10");
//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterVariable<int>("apa");

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;
//            var result = expression.Invoke<int>(10);

//            Assert.AreEqual(10 * 10, result);
//        }


//        [TestMethod]
//        public void Interpreter_full_chain_complex()
//        {
//            var interpreter = new XzaarScriptInterpreter(
//                new KeyValuePair<string, Type>("apa", typeof(int))
//            );

//            var expression = interpreter.CompileExpression("((apa == 8) || (apa < (1 * 2))) || (apa >= 100)") as LambdaXzaarCompiledScript;

//            var lambdaExpression = expression.GetLambdaExpression();
//            var actual = lambdaExpression.ToString();
//            Assert.AreEqual("apa => (((apa == 8) Or (apa < (1 * 2))) Or (apa >= 100))",
//                actual);

//            Assert.IsTrue(expression.Invoke<bool>(0));
//            Assert.IsTrue(expression.Invoke<bool>(8));
//            Assert.IsFalse(expression.Invoke<bool>(10));
//            Assert.IsTrue(expression.Invoke<bool>(100));
//        }


//        [TestMethod]
//        public void Interpreter_full_chain_complex_2()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<int>("apa");
//            interpreter.RegisterVariable<int>("over");
//            var expression = interpreter.CompileExpression("apa == 8 || over == 9000 || apa == 24") as LambdaXzaarCompiledScript;

//            Assert.AreEqual("(apa, over) => (((apa == 8) Or (over == 9000)) Or (apa == 24))",
//                expression.GetLambdaExpression().ToString());

//            Assert.IsTrue(expression.Invoke<bool>(8, 0));
//            Assert.IsTrue(expression.Invoke<bool>(0, 9000));
//            Assert.IsTrue(expression.Invoke<bool>(24, 0));
//            Assert.IsFalse(expression.Invoke<bool>(0, 0));
//        }


//        [TestMethod]
//        public void AssignExpression_0()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<int>("a");
//            var expression = interpreter.CompileExpression("a = 100") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("a => (a = 100)", expression.GetLambdaExpression().ToString());
//        }

//        //[TestMethod]
//        //public void AssignExpression_1()
//        //{
//        //    var interpreter = new XzaarScriptInterpreter();
//        //    interpreter.RegisterVariable<int>("a");
//        //    var expression = interpreter.CompileExpression("a = (a + a) > 0") as LambdaXzaarCompiledScript;
//        //    Assert.AreEqual("a => ((a = (a + a)) > 0)", expression.GetLambdaExpression().ToString());
//        //}


//        [TestMethod]
//        public void AssignExpression_strings_0()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<string>("a");
//            var expression = interpreter.CompileExpression("a = \"100\"") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("a => (a = \"100\")", expression.GetLambdaExpression().ToString());
//        }


//        [TestMethod]
//        public void AssignExpression_strings_1()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<string>("a");
//            var expression = interpreter.CompileExpression("a = \"1'00\"") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("a => (a = \"1'00\")", expression.GetLambdaExpression().ToString());
//        }


//        [TestMethod]
//        public void AssignExpression_strings_2()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<string>("a");
//            var expression = interpreter.CompileExpression("a = '100'") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("a => (a = \"100\")", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void AssignExpression_decimal()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            interpreter.RegisterVariable<double>("a");
//            var expression = interpreter.CompileExpression("a = 1.01") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("a => (a = 1,01)", expression.GetLambdaExpression().ToString());
//        }


//        //[TestMethod]
//        //public void AssignExpression_2()
//        //{
//        //    var interpreter = new XzaarScriptInterpreter();
//        //    interpreter.RegisterVariable<int>("a");
//        //    var expression = interpreter.CompileExpression("a += 5 > 0") as LambdaXzaarCompiledScript;

//        //    Assert.AreEqual("a => ((a = (a + 5)) > 0)", expression.GetLambdaExpression().ToString());
//        //}


//        //[TestMethod]
//        //public void UnaryExpression_plus_plus_returns_boolean()
//        //{
//        //    var interpreter = new XzaarScriptInterpreter();
//        //    interpreter.RegisterVariable<int>("a");
//        //    var expression = interpreter.CompileExpression("a++ > 0") as LambdaXzaarCompiledScript;
//        //    Assert.AreEqual("a => ((a = (a + 1)) > 0)", expression.GetLambdaExpression().ToString());
//        //}

//        //[TestMethod]
//        //public void UnaryExpression_minus_minus_returns_boolean()
//        //{
//        //    var interpreter = new XzaarScriptInterpreter();
//        //    interpreter.RegisterVariable<int>("a");
//        //    var expression = interpreter.CompileExpression("a-- > 0") as LambdaXzaarCompiledScript;
//        //    Assert.AreEqual("a => ((a = (a - 1)) > 0)", expression.GetLambdaExpression().ToString());
//        //}


//        [TestMethod]
//        public void Multiple_Math_Expressions()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            var expression = interpreter.CompileExpression("25 + (10 * 100)") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("() => (25 + (10 * 100))", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void Multiple_Math()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            var expression = interpreter.CompileExpression("25 + 10 * 100") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("() => (25 + (10 * 100))", expression.GetLambdaExpression().ToString());
//        }


//        [TestMethod]
//        public void Multiple_Math_test_op_ordering_0()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            var expression = interpreter.CompileExpression("25 + 10 * 100 - 8 / 2 * 5") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("() => ((25 + (10 * 100)) - ((8 / 2) * 5))", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void Multiple_Math_test_op_ordering_1()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            var expression = interpreter.CompileExpression("10 * 100 - 8 / 2 * 5") as LambdaXzaarCompiledScript;
//            Assert.AreEqual("() => ((10 * 100) - ((8 / 2) * 5))", expression.GetLambdaExpression().ToString());
//        }



//        [TestMethod]
//        public void EmptyStatement()
//        {
//            var interpreter = new XzaarScriptInterpreter();
//            var expression = interpreter.CompileExpression("()") as LambdaXzaarCompiledScript;
//            var lambdaExpression = expression.GetLambdaExpression();
//            Assert.AreEqual(null, lambdaExpression);
//        }




//        [TestMethod]
//        public void Conditiona_test_op_ordering()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("a || b && c");

//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterVariable<bool>("a");
//            compiler.RegisterVariable<bool>("b");
//            compiler.RegisterVariable<bool>("c");

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;

//            Assert.AreEqual("(a, b, c) => (a Or (b And c))", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void Conditiona_test_op_ordering_1()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("a && b || c");

//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterVariable<bool>("a");
//            compiler.RegisterVariable<bool>("b");
//            compiler.RegisterVariable<bool>("c");

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;

//            Assert.AreEqual("(a, b, c) => ((a And b) Or c)", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void Conditiona_test_op_ordering_2()
//        {
//            var compiler = new DotNetXzaarScriptCompiler();

//            compiler.RegisterVariable<bool>("a");
//            compiler.RegisterVariable<bool>("b");
//            compiler.RegisterVariable<bool>("c");
//            compiler.RegisterVariable<bool>("e");

//            var expression = compiler.Compile(
//                new XzaarScriptTransformerOld().Transform(
//                    new XzaarScriptParserOld().Parse(
//                        new XzaarScriptLexerOld().Tokenize("a && b || c && e")))) as LambdaXzaarCompiledScript;

//            Assert.AreEqual("(a, b, c, e) => ((a And b) Or (c And e))", expression.GetLambdaExpression().ToString());
//        }

//        [TestMethod]
//        public void Interpreter_full_chain_complex_3()
//        {
//            var lexer = new XzaarScriptLexerOld();
//            var parser = new XzaarScriptParserOld();
//            var transformer = new XzaarScriptTransformerOld();
//            var compiler = new DotNetXzaarScriptCompiler();
//            var tokens = lexer.Tokenize("apa == 8 && over == 9000 || apa == 24");
//            var ast0 = parser.Parse(tokens);
//            var ast1 = transformer.Transform(ast0);

//            compiler.RegisterVariable<int>("apa");
//            compiler.RegisterVariable<int>("over");

//            var expression = compiler.Compile(ast1) as LambdaXzaarCompiledScript;

//            Assert.AreEqual("(apa, over) => (((apa == 8) And (over == 9000)) Or (apa == 24))", expression.GetLambdaExpression().ToString());

//            Assert.IsTrue(expression.Invoke<bool>(8, 9000));
//            Assert.IsFalse(expression.Invoke<bool>(8, 0));
//            Assert.IsTrue(expression.Invoke<bool>(24, 0));
//            Assert.IsFalse(expression.Invoke<bool>(0, 0));
//        }

//        //[TestMethod]
//        //public void Interpreter_assign_return_value()
//        //{
//        //    var interpreter = new XzaarScriptInterpreter();
//        //    interpreter.RegisterVariable<int>("a");
//        //    var expression = interpreter.CompileExpression("a++") as LambdaXzaarCompiledScript;

//        //    Assert.AreEqual("a => (a = (a + 1))", expression.GetLambdaExpression().ToString());
//        //    Assert.AreEqual(2, expression.Invoke<int>(1));
//        //}
//    }
//}