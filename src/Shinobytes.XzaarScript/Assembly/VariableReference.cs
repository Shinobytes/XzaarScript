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
    public class VariableReference : MemberReference
    {
        private VariableReference reference;

        public VariableReference() { }

        public VariableReference(string name, TypeReference type)
        {
            Name = name;
            Type = type;
        }

        public TypeReference Type { get; set; }

        public object InitialValue { get; set; }

        public override MemberTypes MemberType => MemberTypes.Variable;

        public object ArrayIndex { get; set; }

        public VariableReference Reference
        {
            get => reference;
            set => reference = value;
        }

        public bool IsRef => this.reference != null;

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public virtual VariableReference Clone()
        {
            return new VariableReference
            {
                ArrayIndex = this.ArrayIndex,
                InitialValue = this.InitialValue,
                Reference = this.reference,
                Type = this.Type,
                Name = this.Name
            };
        }
    }

    public class FieldReference : VariableReference
    {
        public VariableReference Instance { get; set; }

        public override MemberTypes MemberType => MemberTypes.Field;

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public override VariableReference Clone()
        {
            return new FieldReference
            {
                Instance = this.Instance,
                Name = this.Name,
                Type = this.Type,
                InitialValue = this.InitialValue,
                ArrayIndex = this.ArrayIndex,
                Reference = this.Reference
            };
        }
    }
}