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
    public class LambdaExpression : AnonymousFunctionExpression
    {
        internal LambdaExpression(ParameterExpression[] parameters, XzaarExpression body)
        {
            this.Parameters = parameters;
            this.Body = body;
        }

        // name of the variable or parameter assigning this lambda
        public string AssignmentName { get; set; }

        public XzaarExpression Body { get; set; }

        public ParameterExpression[] Parameters { get; set; }
    }

    public partial class XzaarExpression
    {
        public static LambdaExpression Lambda(ParameterExpression[] parameters, XzaarExpression body)
        {
            return new LambdaExpression(parameters, body);
        }
        public static LambdaExpression Lambda(ParameterExpression parameter, XzaarExpression body)
        {
            return new LambdaExpression(new[] { parameter }, body);
        }
        public static LambdaExpression Lambda(XzaarExpression body)
        {
            return new LambdaExpression(new ParameterExpression[] { }, body);
        }
    }
}