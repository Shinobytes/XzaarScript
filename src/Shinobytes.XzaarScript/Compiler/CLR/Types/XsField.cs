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
using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsField : XsTypedObject
    {
        private TypeBuilder declaringType;
        private FieldBuilder builder;

        internal XsField(TypeBuilder type, string name, Type fieldType) : base(name, fieldType)
        {
            this.declaringType = type;
            this.builder = declaringType.DefineField(name, fieldType, FieldAttributes.Public);
        }

        public FieldInfo FIeldInfo => builder;
    }
}