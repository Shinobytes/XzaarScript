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
    public class StructExpression : XzaarExpression
    {
        private readonly string name;
        private readonly XzaarExpression[] fields;

        internal StructExpression(string name, XzaarExpression[] fields)
        {
            this.name = name;
            this.fields = fields;
        }

        public string Name => name;
        public XzaarExpression[] Fields => fields;


        public override ExpressionType NodeType => ExpressionType.Struct;
    }

    public partial class XzaarExpression
    {
        public static StructExpression Struct(string name, params XzaarExpression[] fields)
        {
            return new StructExpression(name, fields);
        }
    }
}