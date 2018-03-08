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
    public sealed class ErrorExpression : XzaarExpression
    {
        public string ErrorMessage { get; set; }

        public ErrorExpression(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    public sealed class DefaultExpression : XzaarExpression
    {
        private XzaarType type;

        internal DefaultExpression(XzaarType type)
        {
            this.type = type;
        }

        public sealed override XzaarType Type => type;
        public sealed override ExpressionType NodeType => ExpressionType.Default;
    }

    public partial class XzaarExpression
    {
        public static DefaultExpression Empty()
        {
            return new DefaultExpression(XzaarBaseTypes.Void);
        }

        public static DefaultExpression Default(XzaarType type)
        {
            if (type == XzaarBaseTypes.Void)
            {
                return Empty();
            }
            return new DefaultExpression(type);
        }

        public static ErrorExpression Error()
        {
            return new ErrorExpression(null);
        }

        public static ErrorExpression Error(string errorMessage)
        {
            return new ErrorExpression(errorMessage);
        }

    }
}