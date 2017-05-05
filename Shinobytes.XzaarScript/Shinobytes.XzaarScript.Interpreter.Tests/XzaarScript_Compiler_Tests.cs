using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Extensions;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    [TestClass]
    public class XzaarScript_Compiler_Tests
    {

        [TestMethod]
        public void Script_To_assembly_code()
        {
            var assembly = Assembly("fn main() { }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_return_early()
        {
            var assembly = Assembly("fn looper() { return -1 let j = 0 for(let i = 0; i < 10; i++) { j++ break } return j } let result = looper()");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable result number
  .variable ::temp_number_var3 number
.endGlobalVariables
.globalFunctions
  .function looper
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
      .variable j number
      .variable i number
      .variable ::temp_bool_var0 bool
      .variable ::temp_number_var1 number
      .variable ::temp_number_var2 number
    .endVariables
    .instructions
      Return -1
      Assign j, 0 
      Assign i, 0 
    L_0003:
      CmpLt ::temp_bool_var0, i, 10 
      Jmpf L_0012: ::temp_bool_var0 
      Assign ::temp_number_var1, j 
      Add j, j, 1 
      Jmp L_0012:  
      Assign ::temp_number_var2, i 
      Add i, i, 1 
      Jmp L_0003:  
    L_0012:
      Return j
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
  Callglobal looper, ::temp_number_var3 
  Assign result, ::temp_number_var3 
.endInstructions
", assembly);

        }

        // 

        [TestMethod]
        public void Script_To_assembly_code_assign_global_variable()
        {
            var assembly = Assembly("let j fn main() { j = 0 return }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable j any
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
      Assign j, 0 
      Return
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_return_constant_number_0()
        {
            var assembly = Assembly("fn main() { return 0 }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
      Return 0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_assign_global_variable_expression()
        {
            var assembly = Assembly("let j = 1 + 1");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable j number
  .variable ::temp_number_var0 number
  .variable ::temp_number_var1 number
  .variable ::temp_number_var2 number
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign ::temp_number_var0, 1 
  Assign ::temp_number_var1, 1 
  Add ::temp_number_var2, ::temp_number_var0, ::temp_number_var1 
  Assign j, ::temp_number_var2 
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_simple_if()
        {
            var assembly = Assembly("fn main() { if(0 == 1) { let b = 0 } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable ::temp_bool_var0 bool
      .variable b number
    .endVariables
    .instructions
      CmpEq ::temp_bool_var0, 0, 1 
      Jmpt L_0003: ::temp_bool_var0 
      Jmp L_0006:  
    L_0003:
      Assign b, 0 
      Jmp L_0006:  
    L_0006:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_simple_else_if()
        {
            var assembly = Assembly("fn main() { if(0 == 1) { let b = 0 } else if(1 == 4) { let z = 9999 } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable ::temp_bool_var0 bool
      .variable b number
      .variable ::temp_bool_var1 bool
      .variable z number
    .endVariables
    .instructions
      CmpEq ::temp_bool_var0, 0, 1 
      Jmpt L_0004: ::temp_bool_var0 
      Jmpf L_0007: ::temp_bool_var0 
      Jmp L_0016:  
    L_0004:
      Assign b, 0 
      Jmp L_0016:  
    L_0007:
      CmpEq ::temp_bool_var1, 1, 4 
      Jmpt L_0011: ::temp_bool_var1 
      Jmp L_0014:  
    L_0011:
      Assign z, 9999 
      Jmp L_0014:  
    L_0014:
      Jmp L_0016:  
    L_0016:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Script_To_assembly_code_call_other_function()
        {
            var assembly = Assembly("fn a() { } fn b() { a() } b()");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable ::temp_void_var1 void
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    .endInstructions
  .endFunction
  .function b
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable ::temp_void_var0 void
    .endVariables
    .instructions
      Callglobal a, ::temp_void_var0 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
  Callglobal b, ::temp_void_var1 
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Script_To_assembly_code_return_value_from_call()
        {
            var assembly = Assembly("fn num() { return 0 } fn main() { return num() }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function num
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
      Return 0
    .endInstructions
  .endFunction
  .function main
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
      .variable ::temp_number_var0 number
    .endVariables
    .instructions
      Callglobal num, ::temp_number_var0 
      Return ::temp_number_var0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Switch_case()
        {
            var assembly = Assembly("fn main() { let x = 0 switch(x) { case 0: return case 1: break } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x number
      .variable ::temp_number_var0 number
      .variable ::temp_bool_var1 bool
      .variable ::temp_number_var2 number
      .variable ::temp_bool_var3 bool
    .endVariables
    .instructions
      Assign x, 0 
    L_0001:
      Assign ::temp_number_var0, 0 
      CmpEq ::temp_bool_var1, x, ::temp_number_var0 
      Jmpf L_0006: ::temp_bool_var1 
      Return
    L_0006:
      Assign ::temp_number_var2, 1 
      CmpEq ::temp_bool_var3, x, ::temp_number_var2 
      Jmpf L_0011: ::temp_bool_var3 
      Jmp L_0011:  
    L_0011:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_basic_loop()
        {
            var assembly = Assembly("fn a() { let j = 0 loop { ++j } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable j number
    .endVariables
    .instructions
      Assign j, 0 
    L_0001:
      Add j, j, 1 
      Jmp L_0001:  
    L_0004:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_basic_loop_2()
        {
            var assembly = Assembly("fn a() { let j = 0 loop { j*=2 j/=1 } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable j number
      .variable ::temp_number_var0 number
      .variable ::temp_number_var1 number
      .variable ::temp_number_var2 number
      .variable ::temp_number_var3 number
      .variable ::temp_number_var4 number
      .variable ::temp_number_var5 number
    .endVariables
    .instructions
      Assign j, 0 
    L_0001:
      Assign ::temp_number_var0, j 
      Assign ::temp_number_var1, 2 
      Mul ::temp_number_var2, ::temp_number_var0, ::temp_number_var1 
      Assign j, ::temp_number_var2 
      Assign ::temp_number_var3, j 
      Assign ::temp_number_var4, 1 
      Div ::temp_number_var5, ::temp_number_var3, ::temp_number_var4 
      Assign j, ::temp_number_var5 
      Jmp L_0001:  
    L_0011:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_create_struct()
        {
            var assembly = Assembly("struct s { number c } fn a() { let x = s }");
            Assert.AreEqual(@".structs
  .struct s
    .flags 0
    .fields
      .field c number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x s
    .endVariables
    .instructions
      StructCreate x, s 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_create_complex_struct()
        {
            var assembly = Assembly("struct structA { number structAField } struct structB { structA structBField } fn main() { let s1 = structB }");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field structAField number
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structB
    .flags 0
    .fields
      .field structBField structA
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable s1 structB
    .endVariables
    .instructions
      StructCreate s1, structB 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Invoke_function_dynamically_with_args_4()
        {
            var assembly = Assembly("fn greet(string name) { let j = 0 switch(name) { case \"zerratar\": return true } return false }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function greet
    .flags 0
    .return bool
    .parameters
      .parameter name string
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable j number
      .variable ::temp_string_var0 string
      .variable ::temp_bool_var1 bool
    .endVariables
    .instructions
      Assign j, 0 
    L_0001:
      Assign ::temp_string_var0, ""zerratar"" 
      CmpEq ::temp_bool_var1, name, ::temp_string_var0 
      Jmpf L_0006: ::temp_bool_var1 
      Return True
    L_0006:
      Return False
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_create_complex_struct_get_field_from_structA_and_assign_to_variable()
        {
            var assembly = Assembly("struct structA { number structAField } struct structB { structA structBField } fn main() { let s1 = structB let j1 = s1.structBField.structAField }");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field structAField number
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structB
    .flags 0
    .fields
      .field structBField structA
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable s1 structB
      .variable j1 any
      .variable ::temp_structB_var0 structB
      .variable ::temp_any_var1 any
    .endVariables
    .instructions
      StructCreate s1, structB 
      StructGet ::temp_structB_var0, s1, structBField 
      StructGet ::temp_any_var1, ::temp_structB_var0, structAField 
      Assign j1, ::temp_any_var1 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_create_complex_struct_accessors()
        {
            var assembly = Assembly("struct structA { number structAField } struct structB { structA structBField } struct structC { structB structCField } fn main() { let s1 = structC let j1 = s1.structCField.structBField.structAField }");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field structAField number
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structB
    .flags 0
    .fields
      .field structBField structA
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structC
    .flags 0
    .fields
      .field structCField structB
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable s1 structC
      .variable j1 any
      .variable ::temp_structC_var0 structC
      .variable ::temp_any_var1 any
    .endVariables
    .instructions
      StructCreate s1, structC 
      StructGet ::temp_structC_var0, s1, structCField 
      StructGet ::temp_any_var1, ::temp_structC_var0, structBField 
      StructGet j1, ::temp_any_var1, structAField 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_create_complex_struct_accessors_1()
        {
            var assembly = Assembly("struct structA { number structAField } struct structB { structA structBField } struct structC { structB structCField } fn main() { let s1 = structC s1.structCField.structBField.structAField = 9999 }");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field structAField number
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structB
    .flags 0
    .fields
      .field structBField structA
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structC
    .flags 0
    .fields
      .field structCField structB
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable s1 structC
      .variable ::temp_structC_var0 structC
    .endVariables
    .instructions
      StructCreate s1, structC 
      StructGet ::temp_structC_var0, s1, structCField 
      StructSet ::temp_structC_var0, structAField, 9999 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_create_complex_struct_accessors_2()
        {
            var assembly = Assembly("struct structA { number[] structAField } struct structB { structA structBField } struct structC { structB structCField } fn main() { let s1 = structC s1.structCField.structBField.structAField[0] = 9999 }");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field structAField number[]
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structB
    .flags 0
    .fields
      .field structBField structA
        .flags 0
      .endField
    .endFields
.endStruct
  .struct structC
    .flags 0
    .fields
      .field structCField structB
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable s1 structC
      .variable ::temp_structC_var0 structC
    .endVariables
    .instructions
      StructCreate s1, structC 
      StructGet ::temp_structC_var0, s1, structCField 
      StructSet ::temp_structC_var0, structAField[0], 0, 9999 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_create_struct_assign_variable()
        {
            var assembly = Assembly("struct s { number c } fn a() { let x = s x.c = 0 }");
            Assert.AreEqual(@".structs
  .struct s
    .flags 0
    .fields
      .field c number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x s
    .endVariables
    .instructions
      StructCreate x, s 
      StructSet x, c, 0 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_create_struct_get_variable()
        {
            var assembly = Assembly("struct s { number c } fn a() { let x = s let p = x.c }");
            Assert.AreEqual(@".structs
  .struct s
    .flags 0
    .fields
      .field c number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x s
      .variable p any
    .endVariables
    .instructions
      StructCreate x, s 
      StructGet p, x, c 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_create_struct_get_variable_1()
        {
            var assembly = Assembly("struct s { number c } fn a() { let x = s let p = 0 p = x.c }");
            Assert.AreEqual(@".structs
  .struct s
    .flags 0
    .fields
      .field c number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x s
      .variable p number
      .variable ::temp_number_var0 number
    .endVariables
    .instructions
      StructCreate x, s 
      Assign p, 0 
      StructGet ::temp_number_var0, x, c 
      Assign p, ::temp_number_var0 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Script_To_assembly_code_create_struct_get_variable_2()
        {
            var assembly = Assembly("struct s { number c } fn a() { let x = s let p = x.c }");
            Assert.AreEqual(@".structs
  .struct s
    .flags 0
    .fields
      .field c number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable x s
      .variable p any
    .endVariables
    .instructions
      StructCreate x, s 
      StructGet p, x, c 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_for_loop()
        {
            var assembly = Assembly("fn a() { for(let i = 0; i < 100; ++i) { } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable i number
      .variable ::temp_bool_var0 bool
    .endVariables
    .instructions
      Assign i, 0 
    L_0001:
      CmpLt ::temp_bool_var0, i, 100 
      Jmpf L_0006: ::temp_bool_var0 
      Add i, i, 1 
      Jmp L_0001:  
    L_0006:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_for_loop_2()
        {
            var assembly = Assembly("fn b() { } fn a() { for(let i = 0; i < 100; i++) { b() } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function b
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    .endInstructions
  .endFunction
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable i number
      .variable ::temp_bool_var0 bool
      .variable ::temp_void_var1 void
      .variable ::temp_number_var2 number
    .endVariables
    .instructions
      Assign i, 0 
    L_0001:
      CmpLt ::temp_bool_var0, i, 100 
      Jmpf L_0008: ::temp_bool_var0 
      Callglobal b, ::temp_void_var1 
      Assign ::temp_number_var2, i 
      Add i, i, 1 
      Jmp L_0001:  
    L_0008:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_for_loop_3()
        {
            var assembly = Assembly("fn b() { } fn a() { for(let i = 0; i < 100; ++i) { for(let k = 0; k < 1; ++k) { b() } } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function b
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    .endInstructions
  .endFunction
  .function a
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable i number
      .variable ::temp_bool_var0 bool
      .variable k number
      .variable ::temp_bool_var1 bool
      .variable ::temp_void_var2 void
    .endVariables
    .instructions
      Assign i, 0 
    L_0001:
      CmpLt ::temp_bool_var0, i, 100 
      Jmpf L_0014: ::temp_bool_var0 
      Assign k, 0 
    L_0005:
      CmpLt ::temp_bool_var1, k, 1 
      Jmpf L_0011: ::temp_bool_var1 
      Callglobal b, ::temp_void_var2 
      Add k, k, 1 
      Jmp L_0005:  
    L_0011:
      Add i, i, 1 
      Jmp L_0001:  
    L_0014:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_while_loop()
        {
            var assembly = Assembly("let j fn main() { while(true) { j++ }}");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable j any
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable ::temp_any_var0 any
    .endVariables
    .instructions
    L_0000:
      Jmpf L_0005: True 
      Assign ::temp_any_var0, j 
      Add j, j, 1 
      Jmp L_0000:  
    L_0005:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_do_while_loop()
        {
            var assembly = Assembly("let j fn main() { do { ++j } while(true) }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable j any
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    L_0000:
      Add j, j, 1 
      Jmpt L_0000: True 
      Jmp L_0004:  
    L_0004:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_do_while_loop_loop_break()
        {
            var assembly = Assembly("fn main() { let j = 0 do { ++j loop { --j break } } while(true) }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable j number
    .endVariables
    .instructions
      Assign j, 0 
    L_0001:
      Add j, j, 1 
    L_0003:
      Sub j, j, 1 
      Jmp L_0007:  
      Jmp L_0003:  
    L_0007:
      Jmpt L_0001: True 
      Jmp L_0010:  
    L_0010:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Script_To_assembly_code_foreach_loop()
        {
            var assembly = Assembly("let j fn main() { let p = [] foreach(let j in p) {  } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable j any
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable p array
      .variable ::temp_number_var0 number
      .variable ::temp_number_var1 number
      .variable ::temp_bool_var2 bool
      .variable j any
    .endVariables
    .instructions
      ArrayCreate p 
      Assign ::temp_number_var1, 0 
      ArrayLength ::temp_number_var0, p 
    L_0003:
      CmpLt ::temp_bool_var2, ::temp_number_var1, ::temp_number_var0 
      Jmpf L_0009: ::temp_bool_var2 
      ArrayGetElement j, p, ::temp_number_var1 
      Add ::temp_number_var1, ::temp_number_var1, 1 
      Jmp L_0003:  
    L_0009:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Script_To_assembly_code_goto_label()
        {
            var assembly = Assembly("fn main() { hello_world: goto hello_world }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    hello_world:
      Jmp hello_world:  
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_loop_continue()
        {
            var assembly = Assembly("fn main() { loop { continue } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    L_0000:
      Jmp L_0000:  
      Jmp L_0000:  
    L_0003:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_To_assembly_code_loop_break()
        {
            var assembly = Assembly("fn main() { loop { break } }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    L_0000:
      Jmp L_0003:  
      Jmp L_0000:  
    L_0003:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_loop_return_twice()
        {
            var assembly = Assembly("fn main() { loop { return 0 } return 1 }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
    L_0000:
      Return 0
      Jmp L_0000:  
    L_0003:
      Return 1
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_to_assembly_code_unknown_class_as_argument_and_execute_instance_function()
        {
            var assembly = Assembly("fn test(any s) -> string { s.TestMethod() return s.Test } ");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function test
    .flags 0
    .return string
    .parameters
      .parameter s any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_any_var0 any
      .variable ::temp_any_var1 any
    .endVariables
    .instructions
      Callmethod TestMethod, s, ::temp_any_var0 
      StructGet ::temp_any_var1, s, Test 
      Return ::temp_any_var1
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_set_array_item()
        {
            var assembly = Assembly("fn main() { let a = [] a[0] = 1 }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable a array
    .endVariables
    .instructions
      ArrayCreate a 
      ArraySetElement a[0], 0, 1 
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Script_To_assembly_code_deep_loop_continue_break_return()
        {
            var assembly = Assembly("fn main() { let a = 0 loop { continue a = 1 loop { break a = 2 loop { return } } } }"); // 
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable a number
    .endVariables
    .instructions
      Assign a, 0 
    L_0001:
      Jmp L_0001:  
      Assign a, 1 
    L_0004:
      Jmp L_0012:  
      Assign a, 2 
    L_0007:
      Return
      Jmp L_0007:  
    L_0010:
      Jmp L_0004:  
    L_0012:
      Jmp L_0001:  
    L_0014:
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_function_declare_variable_with_incrementor_expression()
        {
            var assembly = Assembly("fn do_stuff(number val) { let result = val++ return result }");
            Assert.AreEqual(
@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function do_stuff
    .flags 0
    .return number
    .parameters
      .parameter val f64
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable result number
      .variable ::temp_f64_var0 f64
    .endVariables
    .instructions
      Assign ::temp_f64_var0, val 
      Add val, val, 1 
      Assign result, ::temp_f64_var0 
      Return result
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }
        [TestMethod]
        public void Invoke_push_to_array_and_get_length()
        {
            //Assert.Inconclusive("This test needs to be rewritten");
            //return;
            var assembly = Assembly("let array = [] array.push(10) let result = array.length");
            Assert.AreEqual(
            @".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable array array
  .variable result any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  ArrayCreate array 
  ArrayAddElements array 10
  ArrayLength result, array, length 
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression()
        {
            var assembly = Assembly("fn do_stuff(number val) { return val++ }");
            Assert.AreEqual(
@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function do_stuff
    .flags 0
    .return number
    .parameters
      .parameter val f64
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_f64_var0 f64
    .endVariables
    .instructions
      Assign ::temp_f64_var0, val 
      Add val, val, 1 
      Return ::temp_f64_var0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Script_to_assembly_set_managed_field()
        {
            var assembly = Assembly("fn test(any s) -> string { s.Test = \"HEHEHE\" return s.Test } ");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function test
    .flags 0
    .return string
    .parameters
      .parameter s any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_any_var0 any
    .endVariables
    .instructions
      StructSet s, Test, ""HEHEHE"" 
      StructGet ::temp_any_var0, s, Test 
      Return ::temp_any_var0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_function_using_a_clr_class_as_argument_and_execute_function_with_xzaar_object_and_return_result()
        {
            var assembly = Assembly("fn test(any s) -> any { return s.Result1(\"hello there\") } ");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function test
    .flags 0
    .return any
    .parameters
      .parameter s any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_any_var0 any
    .endVariables
    .instructions
      Callmethod Result1, s, ::temp_any_var0 ""hello there""
      Return ::temp_any_var0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_function_accessing_a_struct_with_array()
        {
            var assembly = Assembly("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ");
            Assert.AreEqual(@".structs
  .struct structA
    .flags 0
    .fields
      .field hello number[]
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function do_stuff
    .flags 0
    .return any
    .parameters
    .endParameters
    .variables
      .variable s structA
      .variable i number
      .variable ::temp_bool_var0 bool
      .variable ::temp_number_var1 number
    .endVariables
    .instructions
      StructCreate s, structA 
      Assign i, 0 
    L_0002:
      CmpLt ::temp_bool_var0, i, 5 
      Jmpf L_0009: ::temp_bool_var0 
      StructSet s, hello[i], i, i 
      Assign ::temp_number_var1, i 
      Add i, i, 1 
      Jmp L_0002:  
    L_0009:
      Return s.hello
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Transform_complex_strcat()
        {
            var assembly = Assembly(@"fn main(any console) -> void {
   let anders = ""Anders""
   console.log(""Hey there "" + anders + ""!"")
}");

            Assert.AreEqual(
                @".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable anders string
      .variable ::temp_string_var0 string
      .variable ::temp_string_var1 string
      .variable ::temp_string_var2 string
      .variable ::temp_string_var3 string
      .variable ::temp_string_var4 string
      .variable ::temp_string_var5 string
      .variable ::temp_any_var6 any
    .endVariables
    .instructions
      Assign anders, ""Anders"" 
      Assign ::temp_string_var0, ""Hey there "" 
      Assign ::temp_string_var1, anders 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_string_var1 
      Assign ::temp_string_var3, ::temp_string_var2 
      Assign ::temp_string_var4, ""!"" 
      Add ::temp_string_var5, ::temp_string_var3, ::temp_string_var4 
      Callmethod log, console, ::temp_any_var6 ::temp_string_var5
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_combination_of_strcat_usages()
        {
            var assembly = Assembly(
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
@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable console any
  .variable zerratar string
  .variable ::temp_void_var19 void
.endGlobalVariables
.globalFunctions
  .function favoColor
    .flags 0
    .return string
    .parameters
    .endParameters
    .variables
    .endVariables
    .instructions
      Return ""green""
    .endInstructions
  .endFunction
  .function main
    .flags 0
    .return void
    .parameters
    .endParameters
    .variables
      .variable anders string
      .variable ::temp_string_var0 string
      .variable ::temp_string_var1 string
      .variable ::temp_string_var2 string
      .variable ::temp_string_var3 string
      .variable ::temp_string_var4 string
      .variable ::temp_string_var5 string
      .variable ::temp_any_var6 any
      .variable console any
      .variable ::temp_string_var7 string
      .variable ::temp_string_var8 string
      .variable ::temp_string_var9 string
      .variable ::temp_string_var10 string
      .variable ::temp_string_var11 string
      .variable ::temp_string_var12 string
      .variable ::temp_any_var13 any
      .variable ::temp_string_var14 string
      .variable ::temp_string_var15 string
      .variable ::temp_string_var16 string
      .variable ::temp_string_var17 string
      .variable ::temp_any_var18 any
    .endVariables
    .instructions
      Assign anders, ""Anders"" 
      Assign ::temp_string_var0, ""Hey there "" 
      Assign ::temp_string_var1, anders 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_string_var1 
      Assign ::temp_string_var3, ::temp_string_var2 
      Assign ::temp_string_var4, ""!"" 
      Add ::temp_string_var5, ::temp_string_var3, ::temp_string_var4 
      Assign console, $0 
      Callmethod log, console, ::temp_any_var6 ::temp_string_var5
      Assign ::temp_string_var7, ""My name is "" 
      Assign ::temp_string_var8, zerratar 
      Add ::temp_string_var9, ::temp_string_var7, ::temp_string_var8 
      Assign ::temp_string_var10, ::temp_string_var9 
      Assign ::temp_string_var11, ""."" 
      Add ::temp_string_var12, ::temp_string_var10, ::temp_string_var11 
      Callmethod log, console, ::temp_any_var13 ::temp_string_var12
      Callglobal favoColor, ::temp_string_var14 
      Assign ::temp_string_var15, ""My favorite color is "" 
      Assign ::temp_string_var16, ::temp_string_var14 
      Add ::temp_string_var17, ::temp_string_var15, ::temp_string_var16 
      Callmethod log, console, ::temp_any_var18 ::temp_string_var17
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
  Assign console, $0 
  Assign zerratar, ""Karl"" 
  Callglobal main, ::temp_void_var19 
.endInstructions
", assembly);

        }




        [TestMethod]
        public void Test_modulo()
        {
            var assembly = Assembly("let i = 4 % 2");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable i number
  .variable ::temp_number_var0 number
  .variable ::temp_number_var1 number
  .variable ::temp_number_var2 number
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign ::temp_number_var0, 4 
  Assign ::temp_number_var1, 2 
  Mod ::temp_number_var2, ::temp_number_var0, ::temp_number_var1 
  Assign i, ::temp_number_var2 
.endInstructions
", assembly);
        }



        [TestMethod]
        public void Assign_value_to_managed_field_deep()
        {
            var assembly = Assembly("fn test(any s) -> string { s.Val.Test = \"HEHEHE\" return s.Val.Test } ");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function test
    .flags 0
    .return string
    .parameters
      .parameter s any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_any_var0 any
      .variable ::temp_any_var1 any
    .endVariables
    .instructions
      StructGet ::temp_any_var0, s, Val 
      StructSet ::temp_any_var0, Test, ""HEHEHE"" 
      StructGet ::temp_any_var1, s, Val 
      Return ::temp_any_var1.Test
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Invoke_function_with_loop_2()
        {
            var assembly = Assembly("fn looper() { let j = 0 loop { j++ if (j > 10) { break } } return j } let result = looper()");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable result number
  .variable ::temp_number_var2 number
.endGlobalVariables
.globalFunctions
  .function looper
    .flags 0
    .return number
    .parameters
    .endParameters
    .variables
      .variable j number
      .variable ::temp_number_var0 number
      .variable ::temp_bool_var1 bool
    .endVariables
    .instructions
      Assign j, 0 
    L_0001:
      Assign ::temp_number_var0, j 
      Add j, j, 1 
      CmpGt ::temp_bool_var1, j, 10 
      Jmpt L_0007: ::temp_bool_var1 
      Jmp L_0010:  
    L_0007:
      Jmp L_0012:  
      Jmp L_0010:  
    L_0010:
      Jmp L_0001:  
    L_0012:
      Return j
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
  Callglobal looper, ::temp_number_var2 
  Assign result, ::temp_number_var2 
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Push_array_return_item_value_7()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var assembly = Assembly(@"let console = $console

let names = []

names.push(""Daisy"")

console.log(names[0][1])");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable console any
  .variable names array
  .variable ::temp_any_var0 any
  .variable ::temp_any_var1 any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign console, $console 
  ArrayCreate names 
  ArrayAddElements names ""Daisy""
  Assign ::temp_any_var0, names[0] 
  Callmethod log, console, ::temp_any_var1 ::temp_any_var0[1]
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Push_array_return_item_value_8()
        {
            // Assert.Inconclusive("This feature has not yet been implemented.");

            var assembly = Assembly(@"let console = $console

let names = [""Daisy"",""Friday""]

console.log(names[1][0])");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable console any
  .variable names array
  .variable ::temp_any_var0 any
  .variable ::temp_any_var1 any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign console, $console 
  ArrayCreate names 
  ArrayAddElements names ""Daisy""
  ArrayAddElements names ""Friday""
  Assign ::temp_any_var0, names[1] 
  Callmethod log, console, ::temp_any_var1 ::temp_any_var0[0]
.endInstructions
", assembly);
        }



        [TestMethod]
        public void If_conditional_onvoked_clr_method_1()
        {
            var assembly = Assembly(@"
let woot = $woot
let console = $console
if (1 == 1) console.log('1')
");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable woot any
  .variable console any
  .variable ::temp_bool_var0 bool
  .variable ::temp_any_var1 any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign woot, $woot 
  Assign console, $console 
  CmpEq ::temp_bool_var0, 1, 1 
  Jmpt L_0005: ::temp_bool_var0 
  Jmp L_0008:  
L_0005:
  Callmethod log, console, ::temp_any_var1 ""1""
  Jmp L_0008:  
L_0008:
.endInstructions
", assembly);
        }


        [TestMethod]
        public void If_conditional_onvoked_clr_method_2()
        {
            var assembly = Assembly(@"
let woot = $woot
let console = $console
if (1 == 1) { console.log('1') }
");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable woot any
  .variable console any
  .variable ::temp_bool_var0 bool
  .variable ::temp_any_var1 any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign woot, $woot 
  Assign console, $console 
  CmpEq ::temp_bool_var0, 1, 1 
  Jmpt L_0005: ::temp_bool_var0 
  Jmp L_0008:  
L_0005:
  Callmethod log, console, ::temp_any_var1 ""1""
  Jmp L_0008:  
L_0008:
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Use_bang_not_bool_invoked_clr_method()
        {
            var assembly = Assembly(@"
let woot = $woot
let console = $console
if (1 == 1) console.log('1')
if (!woot.DidItWork('hello')) {
console.log('failed')
} else {
console.log('success')
}

");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable woot any
  .variable console any
  .variable ::temp_bool_var0 bool
  .variable ::temp_any_var1 any
  .variable ::temp_any_var2 any
  .variable ::temp_bool_var3 bool
  .variable ::temp_any_var4 any
  .variable ::temp_any_var5 any
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign woot, $woot 
  Assign console, $console 
  CmpEq ::temp_bool_var0, 1, 1 
  Jmpt L_0005: ::temp_bool_var0 
  Jmp L_0008:  
L_0005:
  Callmethod log, console, ::temp_any_var1 ""1""
  Jmp L_0008:  
L_0008:
  Callmethod DidItWork, woot, ::temp_any_var2 ""hello""
  Not ::temp_bool_var3, ::temp_any_var2 
  Jmpt L_0014: ::temp_bool_var3 
  Jmpf L_0017: ::temp_bool_var3 
  Jmp L_0020:  
L_0014:
  Callmethod log, console, ::temp_any_var4 ""failed""
  Jmp L_0020:  
L_0017:
  Callmethod log, console, ::temp_any_var5 ""success""
  Jmp L_0020:  
L_0020:
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Invoke_function_return_incrementor_expression_2()
        {
            var assembly = Assembly("fn do_stuff(number val) { return ++val }");
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function do_stuff
    .flags 0
    .return number
    .parameters
      .parameter val f64
        .flags 0
      .endParameter
    .endParameters
    .variables
    .endVariables
    .instructions
      Add val, val, 1 
      Return val
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }


        [TestMethod]
        public void Format_struct_usage_in_functions()
        {
            var assembly = Assembly("struct testStruct { number hello } fn fun_with_struct(testStruct s) { s.hello = 9999 } fn do_stuff() { let s1 = testStruct fun_with_struct(s1) return s1.hello }");
            Assert.AreEqual(@".structs
  .struct testStruct
    .flags 0
    .fields
      .field hello number
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function fun_with_struct
    .flags 0
    .return void
    .parameters
      .parameter s testStruct
        .flags 0
      .endParameter
    .endParameters
    .variables
    .endVariables
    .instructions
      StructSet s, hello, 9999 
    .endInstructions
  .endFunction
  .function do_stuff
    .flags 0
    .return any
    .parameters
    .endParameters
    .variables
      .variable s1 testStruct
      .variable ::temp_void_var0 void
    .endVariables
    .instructions
      StructCreate s1, testStruct 
      Callglobal fun_with_struct, ::temp_void_var0 s1
      Return s1.hello
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", assembly);
        }

        [TestMethod]
        public void Compile_complex_strcat_1()
        {
            var code = Assembly(@"struct s {
    number num,
    string str
}
fn main(any console) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num + ""asd"" + 0 + 16
    console.log(j.str + j.num)
}
");

            Assert.AreEqual(
@".structs
  .struct s
    .flags 0
    .fields
      .field num number
        .flags 0
      .endField
      .field str string
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable j s
      .variable ::temp_string_var0 string
      .variable ::temp_any_var1 any
      .variable ::temp_string_var2 string
      .variable ::temp_string_var3 string
      .variable ::temp_string_var4 string
      .variable ::temp_string_var5 string
      .variable ::temp_string_var6 string
      .variable ::temp_number_var7 number
      .variable ::temp_string_var8 string
      .variable ::temp_string_var9 string
      .variable ::temp_number_var10 number
      .variable ::temp_string_var11 string
      .variable ::temp_any_var12 any
      .variable ::temp_any_var13 any
      .variable ::temp_string_var14 string
      .variable ::temp_any_var15 any
    .endVariables
    .instructions
      StructCreate j, s 
      StructSet j, num, 9000 
      Assign ::temp_string_var0, ""over "" 
      StructGet ::temp_any_var1, j, num 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_any_var1 
      Assign ::temp_string_var3, ::temp_string_var2 
      Assign ::temp_string_var4, ""asd"" 
      Add ::temp_string_var5, ::temp_string_var3, ::temp_string_var4 
      Assign ::temp_string_var6, ::temp_string_var5 
      Assign ::temp_number_var7, 0 
      Add ::temp_string_var8, ::temp_string_var6, ::temp_number_var7 
      Assign ::temp_string_var9, ::temp_string_var8 
      Assign ::temp_number_var10, 16 
      Add ::temp_string_var11, ::temp_string_var9, ::temp_number_var10 
      StructSet j, str, ::temp_string_var11 
      StructGet ::temp_any_var12, j, str 
      StructGet ::temp_any_var13, j, num 
      Add ::temp_string_var14, ::temp_any_var12, ::temp_any_var13 
      Callmethod log, console, ::temp_any_var15 ::temp_string_var14
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", code);


        }

        [TestMethod]
        public void Compile_complex_strcat_2()
        {
            var code = Assembly(@"struct s {
    number num,
    string str
}
fn main(any console) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num + ""asd"" + 0
    console.log(j.str + j.num)
}
");

            Assert.AreEqual(
@".structs
  .struct s
    .flags 0
    .fields
      .field num number
        .flags 0
      .endField
      .field str string
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable j s
      .variable ::temp_string_var0 string
      .variable ::temp_any_var1 any
      .variable ::temp_string_var2 string
      .variable ::temp_string_var3 string
      .variable ::temp_string_var4 string
      .variable ::temp_string_var5 string
      .variable ::temp_string_var6 string
      .variable ::temp_number_var7 number
      .variable ::temp_string_var8 string
      .variable ::temp_any_var9 any
      .variable ::temp_any_var10 any
      .variable ::temp_string_var11 string
      .variable ::temp_any_var12 any
    .endVariables
    .instructions
      StructCreate j, s 
      StructSet j, num, 9000 
      Assign ::temp_string_var0, ""over "" 
      StructGet ::temp_any_var1, j, num 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_any_var1 
      Assign ::temp_string_var3, ::temp_string_var2 
      Assign ::temp_string_var4, ""asd"" 
      Add ::temp_string_var5, ::temp_string_var3, ::temp_string_var4 
      Assign ::temp_string_var6, ::temp_string_var5 
      Assign ::temp_number_var7, 0 
      Add ::temp_string_var8, ::temp_string_var6, ::temp_number_var7 
      StructSet j, str, ::temp_string_var8 
      StructGet ::temp_any_var9, j, str 
      StructGet ::temp_any_var10, j, num 
      Add ::temp_string_var11, ::temp_any_var9, ::temp_any_var10 
      Callmethod log, console, ::temp_any_var12 ::temp_string_var11
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", code);
        }

        [TestMethod]
        public void constant_element_property()
        {
            var code = Assembly("fn test(any console) { console.log(\"Hello World\"[0].length) }");            
            Assert.AreEqual(@".structs
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function test
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable ::temp_any_var0 any
      .variable ::temp_any_var1 any
    .endVariables
    .instructions
      StructGet ::temp_any_var0, ""Hello World""[0], length 
      Callmethod log, console, ::temp_any_var1 ::temp_any_var0
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", code);
        }


        [TestMethod]
        public void Compile_complex_strcat_3()
        {
            var code = Assembly(@"struct s {
    number num,
    string str
}
fn main(any console) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + j.num
    console.log(j.str + j.num)
}
");

            Assert.AreEqual(
@".structs
  .struct s
    .flags 0
    .fields
      .field num number
        .flags 0
      .endField
      .field str string
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable j s
      .variable ::temp_string_var0 string
      .variable ::temp_any_var1 any
      .variable ::temp_string_var2 string
      .variable ::temp_any_var3 any
      .variable ::temp_any_var4 any
      .variable ::temp_string_var5 string
      .variable ::temp_any_var6 any
    .endVariables
    .instructions
      StructCreate j, s 
      StructSet j, num, 9000 
      Assign ::temp_string_var0, ""over "" 
      StructGet ::temp_any_var1, j, num 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_any_var1 
      StructSet j, str, ::temp_string_var2 
      StructGet ::temp_any_var3, j, str 
      StructGet ::temp_any_var4, j, num 
      Add ::temp_string_var5, ::temp_any_var3, ::temp_any_var4 
      Callmethod log, console, ::temp_any_var6 ::temp_string_var5
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", code);
        }




        [TestMethod]
        public void Transform_complex_strcat_4()
        {
            var code = Assembly(@"struct s {
    number num,
    string str
}
fn main(any console) -> void {
    let j = s
    j.num = 9000
    j.str = ""over "" + 5
    console.log(j.str + j.num)
}
");

            Assert.AreEqual(
@".structs
  .struct s
    .flags 0
    .fields
      .field num number
        .flags 0
      .endField
      .field str string
        .flags 0
      .endField
    .endFields
.endStruct
.endStructs
.types
.endTypes
.globalVariables
.endGlobalVariables
.globalFunctions
  .function main
    .flags 0
    .return void
    .parameters
      .parameter console any
        .flags 0
      .endParameter
    .endParameters
    .variables
      .variable j s
      .variable ::temp_string_var0 string
      .variable ::temp_number_var1 number
      .variable ::temp_string_var2 string
      .variable ::temp_any_var3 any
      .variable ::temp_any_var4 any
      .variable ::temp_string_var5 string
      .variable ::temp_any_var6 any
    .endVariables
    .instructions
      StructCreate j, s 
      StructSet j, num, 9000 
      Assign ::temp_string_var0, ""over "" 
      Assign ::temp_number_var1, 5 
      Add ::temp_string_var2, ::temp_string_var0, ::temp_number_var1 
      StructSet j, str, ::temp_string_var2 
      StructGet ::temp_any_var3, j, str 
      StructGet ::temp_any_var4, j, num 
      Add ::temp_string_var5, ::temp_any_var3, ::temp_any_var4 
      Callmethod log, console, ::temp_any_var6 ::temp_string_var5
    .endInstructions
  .endFunction
.endGlobalFunctions
.instructions
.endInstructions
", code);
        }



        [TestMethod]
        public void Push_array_return_item_value_3()
        {
            var code = Assembly(@"let console = $console
let names = []

names.push(""Daisy"")

for(let j = 0; j < names.length; j++) { 
   console.log(names[j]) 
}");
            Assert.AreEqual(
        @".structs
.endStructs
.types
.endTypes
.globalVariables
  .variable console any
  .variable names array
  .variable j number
  .variable ::temp_any_var0 any
  .variable ::temp_bool_var1 bool
  .variable ::temp_any_var2 any
  .variable ::temp_number_var3 number
.endGlobalVariables
.globalFunctions
.endGlobalFunctions
.instructions
  Assign console, $console 
  ArrayCreate names 
  ArrayAddElements names ""Daisy""
  Assign j, 0 
L_0004:
  StructGet ::temp_any_var0, names, length 
  CmpLt ::temp_bool_var1, j, ::temp_any_var0 
  Jmpf L_0012: ::temp_bool_var1 
  Callmethod log, console, ::temp_any_var2 names[j]
  Assign ::temp_number_var3, j 
  Add j, j, 1 
  Jmp L_0004:  
L_0012:
.endInstructions
", code);

        }



        [TestMethod]
        public void Compile_empty_global_function()
        {
            var assembly = Compile("fn main() { }");
            Assert.AreEqual(1, assembly.GlobalMethods.Count);
            Assert.AreEqual("main", assembly.GlobalMethods[0].Name);
        }

        [TestMethod]
        public void Compile_global_function_define_one_variable_body_hasVariables_returns_true()
        {
            var assembly = Compile("fn main() { let j = 0 }");
            Assert.AreEqual(1, assembly.GlobalMethods.Count);
            Assert.AreEqual("main", assembly.GlobalMethods[0].Name);
            Assert.AreEqual(true, assembly.GlobalMethods[0].Body.HasVariables);
            Assert.AreEqual("j", assembly.GlobalMethods[0].Body.MethodVariables[0].Name);
        }

        [TestMethod]
        public void Compile_global_variable()
        {
            var assembly = Compile("let j = 0");
            Assert.AreEqual(1, assembly.GlobalVariables.Count);
            Assert.AreEqual("j", assembly.GlobalVariables[0].Name);
        }

        [TestMethod]
        public void Compile_global_variable_and_global_empty_function()
        {
            var assembly = Compile("let j = 0 fn main() { }");
            Assert.AreEqual(1, assembly.GlobalVariables.Count);
            Assert.AreEqual("j", assembly.GlobalVariables[0].Name);
            Assert.AreEqual(1, assembly.GlobalMethods.Count);
            Assert.AreEqual("main", assembly.GlobalMethods[0].Name);
        }


        [TestMethod]
        public void Compile_empty_global_function_with_parameters()
        {
            var assembly = Compile("fn main(number a, string b) { }");

            Assert.AreEqual(1, assembly.GlobalMethods.Count);
            Assert.AreEqual("main", assembly.GlobalMethods[0].Name);
            Assert.AreEqual(2, assembly.GlobalMethods[0].Parameters.Count);
            Assert.AreEqual("a", assembly.GlobalMethods[0].Parameters[0].Name);
            Assert.AreEqual("b", assembly.GlobalMethods[0].Parameters[1].Name);
        }

        private XzaarAssembly Compile(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile();
        }
        private string Assembly(string inputCode)
        {
            return Compile(inputCode)
                .AssemblyCode();
        }
    }
}