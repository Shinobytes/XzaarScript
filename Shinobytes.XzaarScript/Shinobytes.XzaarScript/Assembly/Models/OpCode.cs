namespace Shinobytes.XzaarScript.Assembly.Models
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
        ArrayIndexOf = 46
    }
}