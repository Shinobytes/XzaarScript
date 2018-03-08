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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    [Serializable]
    public class FieldExpression : XzaarExpression
    {
        private string name;
        private XzaarType type;

        internal FieldExpression(XzaarType type, string name, XzaarType declaringType)
        {
            DeclaringType = declaringType;
            this.type = type;
            this.name = name;
        }

        public XzaarType DeclaringType { get; }

        public string Name => name;

        public XzaarType FieldType => type;

        public override ExpressionType NodeType => ExpressionType.Field;

        public override XzaarType Type => this.type;
    }


    public partial class XzaarExpression
    {
        public static FieldExpression Field(XzaarType type, string name, XzaarType declaringType)
        {
            return new FieldExpression(type, name, declaringType);
        }
    }
}