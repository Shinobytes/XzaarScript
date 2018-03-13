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

namespace Shinobytes.XzaarScript.Assembly
{
    public class TypeReference : MemberReference
    {
        public TypeVariableCollection Fields { get; internal set; }

        public MethodCollection Methods { get; internal set; }

        public TypeReference(XzaarType type)
        {
            this.Fields = new TypeVariableCollection();
            this.Methods = new MethodCollection();

            if ((object)type == null) throw new ArgumentNullException(nameof(type));
            if ((object)type.BaseType != null)
                this.BaseType = new TypeReference(type.BaseType);
            base.Name = type.Name;
            this.IsArray = type.IsArray;
            if (this.IsArray)
                this.ArrayElementType = type.GetElementType();
        }

        public TypeReference(string name, TypeReference baseType)
        {
            this.Fields = new TypeVariableCollection();
            this.Methods = new MethodCollection();
            base.Name = name;
            this.BaseType = baseType;
        }

        public override MemberTypes MemberType => IsClass ? MemberTypes.Class : MemberTypes.Struct;

        public XzaarType ArrayElementType { get; internal set; }

        public bool IsClass { get; internal set; }

        public bool IsStruct => MemberType == MemberTypes.Struct;

        public TypeReference BaseType { get; internal set; }

        public bool IsArray { get; internal set; }
    }
}