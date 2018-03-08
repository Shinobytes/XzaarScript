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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class CreateStructExpression : XzaarExpression
    {
        internal CreateStructExpression(string structName)
        {
            StructName = structName;
        }

        internal CreateStructExpression(string structName, XzaarExpression[] fieldInitializers)
        {
            StructName = structName;
            FieldInitializers = fieldInitializers;
        }

        public string StructName { get; }

        public XzaarExpression[] FieldInitializers { get; set; }

        public override ExpressionType NodeType => ExpressionType.CreateStruct;
    }

    public partial class XzaarExpression
    {
        public static CreateStructExpression CreateStruct(string structName)
        {
            return new CreateStructExpression(structName);
        }

        public static CreateStructExpression CreateStruct(string structName, XzaarExpression[] fieldInitializers)
        {
            return new CreateStructExpression(structName, fieldInitializers);
        }
    }
}