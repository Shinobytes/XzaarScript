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
 
namespace Shinobytes.XzaarScript.Assembly
{
    // Dont we like 11 ?
    public enum OpCode : byte
    {
        Nop = 0,
        Add = 1,
        Sub = 2,
        Mul = 3,
        Div = 4,
        Mod = 5,
        Not = 6,
        Neg = 7,
        Assign = 8,
        Cast = 9,
        CmpEq = 10,
        CmpNotEq = 11,
        CmpLt = 12,
        CmpLte = 13,
        CmpGt = 14,
        CmpGte = 15,
        Jmp = 16,
        Jmpt = 17,
        Jmpf = 18,
        Callmethod = 19,
        Callglobal = 20,
        Callparent = 21,
        Callextern = 22,
        Callstatic = 23,
        Callanonymous = 47,
        Callunknown = 48,
        Return = 24,
        Strcat = 25,
        PropGet = 26,
        PropSet = 27,
        ArrayCreate = 28,
        ArrayLength = 30,
        ArrayGetElement = 31,
        ArraySetElement = 32,
        ArrayFindElement = 33, // Find First Matching Element
        ArrayFindLastElement = 34, // Find Last Matching Element, ArrayRFindElement

        Is = 35,
        StructCreate = 36, // StructGet ::output_variable, struct_instance_variable, struct_field, [optional: arrayindex]
        StructGet = 37,    // StructSet struct_instance_variable, struct_field, [optional: arrayindex], value
        StructSet = 38,
        ArrayFindStruct = 39, // Find First Matching element of an array inside a Struct
        ArrayFindLastStruct = 40, // Find Last Matching element of an array inside a struct, ArrayRFindStruct
        ArrayAddElements = 41,
        ArrayInsertElement = 42,
        ArrayRemoveLastElement = 43,
        ArrayRemoveElements = 44,
        ArrayClearElements = 45,
        ArrayIndexOf = 46,
    }
}