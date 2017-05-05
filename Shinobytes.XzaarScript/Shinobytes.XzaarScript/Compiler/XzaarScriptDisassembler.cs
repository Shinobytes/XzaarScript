﻿using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;
using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Compiler
{
    public class XzaarScriptDisassembler
    {
        private XzaarAssembly asm;
        private int indent;
        public XzaarScriptDisassembler(XzaarAssembly asm)
        {
            this.asm = asm;
        }

        public static string Disassemble(XzaarAssembly asm)
        {
            return new XzaarScriptDisassembler(asm).Disassemble();
        }

        private string Disassemble()
        {
            var codeWriter = new XzaarCodeWriter();
            WriteStructs(codeWriter);
            WriteTypes(codeWriter);
            WriteGlobalVariables(codeWriter);
            WriteGlobalFunctions(codeWriter);
            WriteGlobalInstructions(codeWriter);
            return codeWriter.ToString();
        }

        private void WriteGlobalInstructions(XzaarCodeWriter codeWriter)
        {
            codeWriter.WriteLine(".instructions");
            indent++;
            foreach (var t in asm.GlobalInstructions)
            {
                codeWriter.Write(WriteInstruction(t));
            }
            indent--;
            codeWriter.WriteLine(".endInstructions");
        }

        private void WriteGlobalFunctions(XzaarCodeWriter codeWriter)
        {
            codeWriter.WriteLine(".globalFunctions");
            indent++;
            foreach (var t in asm.GlobalMethods)
            {
                codeWriter.Write(WriteFunction(t));
            }
            indent--;
            codeWriter.WriteLine(".endGlobalFunctions");
        }


        private void WriteGlobalVariables(XzaarCodeWriter codeWriter)
        {
            codeWriter.WriteLine(".globalVariables");
            indent++;
            foreach (var t in asm.GlobalVariables)
            {
                codeWriter.Write(WriteVariable(t));
            }
            indent--;
            codeWriter.WriteLine(".endGlobalVariables");
        }

        private void WriteStructs(XzaarCodeWriter codeWriter)
        {
            codeWriter.WriteLine(".structs");
            indent++;
            foreach (var t in asm.Types.Where(t => t.IsStruct))
            {
                codeWriter.Write(WriteStruct(t));
            }
            indent--;
            codeWriter.WriteLine(".endStructs");
        }

        private void WriteTypes(XzaarCodeWriter codeWriter)
        {
            codeWriter.WriteLine(".types");
            indent++;
            foreach (var t in asm.Types.Where(t => !t.IsStruct))
            {
                codeWriter.Write(WriteType(t));
            }
            indent--;
            codeWriter.WriteLine(".endTypes");
        }

        private string WriteVariable(VariableReference variableReference)
        {
            var cw = new XzaarCodeWriter();
            cw.WriteLine(".variable " + variableReference.Name + " " + variableReference.Type.Name, indent);
            //indent++;
            //cw.WriteLine(".flags 0", indent);
            //if (variableReference.InitialValue != null)
            //{
            //    var val = variableReference.InitialValue is string
            //        ? "\"" + variableReference.InitialValue + "\""
            //        : variableReference.InitialValue;
            //    cw.WriteLine(".initialValue " + val, indent);
            //}

            //indent--;
            //cw.WriteLine(".endVariable", indent);
            return cw.ToString();
        }
        private string WriteField(VariableReference variableReference)
        {
            var cw = new XzaarCodeWriter();
            cw.WriteLine(".field " + variableReference.Name + " " + variableReference.Type.Name, indent);
            indent++;
            cw.WriteLine(".flags 0", indent);
            if (variableReference.InitialValue != null)
            {
                var val = variableReference.InitialValue is string
                    ? "\"" + variableReference.InitialValue + "\""
                    : variableReference.InitialValue;
                cw.WriteLine(".initialValue " + val, indent);
            }

            indent--;
            cw.WriteLine(".endField", indent);
            return cw.ToString();
        }

        private string WriteInstruction(XzaarBinaryCode o)
        {
            var cw = new XzaarCodeWriter();
            var instruction = o as Instruction;
            if (instruction != null)
            {
                if (instruction.OpCode == OpCode.Return)
                {
                    if (instruction.Arguments.Count > 0)
                    {
                        var returnExpr = instruction.Arguments[0];
                        var field = returnExpr as FieldReference;
                        if (field != null)
                        {
                            
                            var chain = new List<string>();
                            var fRef = field.Instance;
                            while (fRef != null)
                            {
                                chain.Insert(0, fRef.Name);
                                if (fRef is FieldReference)
                                    fRef = (fRef as FieldReference).Instance;
                                else
                                    fRef = null;
                            }
                            chain.Add(field.Name);
                            cw.WriteLine(instruction.OpCode + " " + string.Join(".", chain.ToArray()), indent);
                        }
                        else
                        {                            
                            cw.WriteLine(instruction.OpCode + " " + instruction.Arguments.ToString(", "), indent);
                        }
                    }
                    else
                    {
                        cw.WriteLine(instruction.OpCode + "", indent);
                    }
                }
                else if (instruction.TargetLabel != null)
                {
                    var labelName = GetLabelName(instruction.TargetLabel);
                    cw.WriteLine(
                        instruction.OpCode + " " + labelName + ": " + instruction.Arguments.ToString(", ") + " " +
                        instruction.OperandArguments.ToString(", "), indent);
                }
                else
                    cw.WriteLine(
                        instruction.OpCode + " " + instruction.Arguments.ToString(", ") + " " +
                        instruction.OperandArguments.ToString(", "), indent);
            }
            else
            {
                var label = o as Label;
                var labelName = GetLabelName(label);
                cw.WriteLine(labelName + ":", indent - 1);
            }
            return cw.ToString();
        }

        private object GetLabelName(Label instructionTargetLabel)
        {
            if (!string.IsNullOrEmpty(instructionTargetLabel.Name))
            {
                return instructionTargetLabel.Name;
            }
            return "L_" + instructionTargetLabel.Offset.ToString("0000");
        }

        private string WriteFunction(MethodDefinition methodDefinition)
        {
            var cw = new XzaarCodeWriter();
            cw.WriteLine(".function " + methodDefinition.Name, indent);
            indent++;
            cw.WriteLine(".flags 0", indent);
            cw.WriteLine(".return " + methodDefinition.ReturnType.Name, indent);
            cw.WriteLine(".parameters", indent);
            if (methodDefinition.Parameters != null)
            {
                indent++;

                foreach (var t in methodDefinition.Parameters)
                    cw.Write(WriteParameter(t));

                indent--;
            }
            cw.WriteLine(".endParameters", indent);
            cw.WriteLine(".variables", indent);
            if (methodDefinition.Body.MethodVariables != null)
            {
                indent++;

                foreach (var t in methodDefinition.Body.MethodVariables)
                    cw.Write(WriteVariable(t));

                indent--;
            }
            cw.WriteLine(".endVariables", indent);
            cw.WriteLine(".instructions", indent);
            if (methodDefinition.Body.MethodInstructions != null)
            {
                indent++;

                foreach (var t in methodDefinition.Body.MethodInstructions)
                    cw.Write(WriteInstruction(t));

                indent--;
            }
            cw.WriteLine(".endInstructions", indent);
            indent--;
            cw.WriteLine(".endFunction", indent);
            return cw.ToString();
        }

        private string WriteParameter(ParameterDefinition parameterDefinition)
        {
            var cw = new XzaarCodeWriter();
            cw.WriteLine(".parameter " + parameterDefinition.Name + " " + parameterDefinition.Type.Name, indent);
            indent++;
            cw.WriteLine(".flags 0", indent);
            if (parameterDefinition.InitialValue != null)
            {
                var val = parameterDefinition.InitialValue is string
                    ? "\"" + parameterDefinition.InitialValue + "\""
                    : parameterDefinition.InitialValue;
                cw.WriteLine(".initialValue " + val, indent);
            }
            indent--;
            cw.WriteLine(".endParameter", indent);
            return cw.ToString();
        }

        private string WriteType(TypeDefinition typeDefinition)
        {
            var cw = new XzaarCodeWriter();
            return cw.ToString();
        }

        private string WriteStruct(TypeDefinition typeDefinition)
        {
            var cw = new XzaarCodeWriter();
            cw.WriteLine(".struct " + typeDefinition.Name, indent);
            indent++;
            cw.WriteLine(".flags 0", indent);
            cw.WriteLine(".fields", indent);
            indent++;
            foreach (var t in typeDefinition.Fields)
            {
                cw.Write(WriteField(t));
            }
            indent--;
            cw.WriteLine(".endFields", indent);
            indent--;
            cw.WriteLine(".endStruct");

            return cw.ToString();
        }
    }
}