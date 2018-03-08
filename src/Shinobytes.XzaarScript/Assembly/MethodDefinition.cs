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