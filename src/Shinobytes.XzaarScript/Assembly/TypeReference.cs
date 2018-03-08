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