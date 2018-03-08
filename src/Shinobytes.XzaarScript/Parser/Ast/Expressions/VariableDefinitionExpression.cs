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
    public class VariableDefinitionExpression : ParameterExpression
    {
        private XzaarType type;

        internal VariableDefinitionExpression(XzaarType type, string name, XzaarExpression assignmentExpression)
            : base(name)
        {
            this.type = type;            
            AssignmentExpression = assignmentExpression;
        }

        public override XzaarType Type => type;

        public XzaarExpression AssignmentExpression { get; }        
    }

    public partial class XzaarExpression
    {
        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name)
        {
            return DefineVariable(type, name, null);
        }

        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name, XzaarExpression assignmentExpression)
        {
            return new VariableDefinitionExpression(type, name, assignmentExpression);
        }
    }
}