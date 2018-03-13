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
 
using System;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsILGenerator
    {
        private readonly ILGenerator il;
        private OpCode lastEmittedOpCode;

        public XsILGenerator(ILGenerator il)
        {
            this.il = il;
        }

        public virtual void Emit(OpCode opcode)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode);
        }

        public virtual void Emit(OpCode opcode, byte arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, short arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, int arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, MethodInfo meth)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, meth);
        }

        public virtual void Emit(OpCode opcode, XsMethod meth) => Emit(opcode, meth.MethodInfo);

        public virtual void EmitCalli(
            OpCode opcode,
            CallingConventions callingConvention,
            Type returnType,
            Type[] parameterTypes,
            Type[] optionalParameterTypes)
        {
            lastEmittedOpCode = opcode;
            il.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
        }

        public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            lastEmittedOpCode = opcode;
            il.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
        }

        public virtual void EmitCall(OpCode opcode, XsMethod methodInfo, Type[] optionalParameterTypes)
            => EmitCall(opcode, methodInfo.MethodInfo, optionalParameterTypes);

        public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            lastEmittedOpCode = opcode;
            il.EmitCall(opcode, methodInfo, optionalParameterTypes);
        }

        public virtual void Emit(OpCode opcode, SignatureHelper signature)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, signature);
        }

        [System.Runtime.InteropServices.ComVisible(true)]
        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, con);
        }

        public virtual void Emit(OpCode opcode, Type cls)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, cls);
        }

        public virtual void Emit(OpCode opcode, long arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, float arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, double arg)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, Label label)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, label);
        }

        public virtual void Emit(OpCode opcode, Label[] labels)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, labels);
        }

        public virtual void Emit(OpCode opcode, XsField field)
            => Emit(opcode, field.FIeldInfo);

        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, field);
        }

        public virtual void Emit(OpCode opcode, XsParameter param)
            => Emit(opcode, param.Index);

        public virtual void Emit(OpCode opcode, String str)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, str);
        }

        public virtual void Emit(OpCode opcode, XsVariable local) => Emit(opcode, local.VariableInfo);

        public virtual void Emit(OpCode opcode, LocalBuilder local)
        {
            lastEmittedOpCode = opcode;
            il.Emit(opcode, local);
        }

        internal virtual LocalBuilder DeclareLocal(Type localType)
        {
            return DeclareLocal(localType, false);
        }

        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            return il.DeclareLocal(localType, pinned);
        }

        public virtual void UsingNamespace(String usingNamespace)
        {
            il.UsingNamespace(usingNamespace);
        }

        public virtual void MarkSequencePoint(
            ISymbolDocumentWriter document,
            int startLine,       // line number is 1 based 
            int startColumn,     // column is 0 based
            int endLine,         // line number is 1 based 
            int endColumn)       // column is 0 based
        {
            il.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
        }

        public virtual void BeginScope()
        {
            il.BeginScope();
        }

        public virtual void EndScope()
        {
            il.EndScope();
        }

        public virtual Label DefineLabel()
        {
            return il.DefineLabel();
        }

        public virtual void MarkLabel(Label loc)
        {
            il.MarkLabel(loc);
        }

        public virtual void ThrowException(Type excType)
        {
            il.ThrowException(excType);
        }

        public virtual Label BeginExceptionBlock()
        {
            return il.BeginExceptionBlock();
        }

        public virtual void EndExceptionBlock()
        {
            il.EndExceptionBlock();
        }

        public virtual void BeginExceptFilterBlock()
        {
            il.BeginExceptFilterBlock();
        }

        public virtual void BeginCatchBlock(Type exceptionType)
        {
            il.BeginCatchBlock(exceptionType);
        }

        public virtual void BeginFaultBlock()
        {
            il.BeginFaultBlock();
        }

        public virtual void BeginFinallyBlock()
        {
            il.BeginFinallyBlock();
        }

        public OpCode LastEmittedOpCode => lastEmittedOpCode;
    }
}
