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
    public class MethodDefinition : MethodReference
    {
        public MethodDefinition(string name, ParameterDefinition[] parameters, MethodBody body, TypeReference returnType, TypeReference declaringType, bool isExtern)
        {
            this.Parameters = new ParameterCollection(this);
            this.IsExtern = isExtern;
            this.Name = name;
            this.Parameters.AddParameters(parameters);
            this.Body = body;
            this.ReturnType = returnType;
            this.DeclaringType = declaringType;

            if (this.Body == null)
                this.Body = new MethodBody(this);
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            TypeReference returnType,
            TypeReference declaringType,
            bool isExtern)
            : this(name, parameters, null, returnType, declaringType, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            TypeReference returnType,
            bool isExtern)
            : this(name, parameters, null, returnType, null, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            bool isExtern)
            : this(name, parameters, null, null, null, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            bool isExtern)
            : this(name, null, null, null, null, isExtern)
        {
        }

        public MethodDefinition SetParameters(ParameterDefinition[] parameters)
        {
            this.Parameters.Clear();
            this.Parameters.AddParameters(parameters);
            return this;
        }

        public MethodDefinition SetReturnType(TypeReference returnType)
        {
            this.ReturnType = returnType;
            return this;
        }


        public MethodDefinition SetDeclaringType(TypeReference declaringType)
        {
            this.DeclaringType = declaringType;
            return this;
        }

        public MethodDefinition SetBody(MethodBody body)
        {
            this.Body = body;
            return this;
        }

        public bool IsExtern { get; private set; }

        public MethodBody Body { get; private set; }
    }
}