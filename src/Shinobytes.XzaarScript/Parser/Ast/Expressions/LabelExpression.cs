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
    public sealed class LabelExpression : XzaarExpression
    {
        private readonly LabelTarget target;
        private readonly XzaarExpression defaultValue;        

        internal LabelExpression(LabelTarget label, XzaarExpression defaultValue)
        {
            this.target = label;
            this.defaultValue = defaultValue;
        }

        public sealed override XzaarType Type => this.target.Type;

        public LabelTarget Target => target;

        public XzaarExpression DefaultValue => defaultValue;

        public LabelExpression Update(LabelTarget target, XzaarExpression defaultValue)
        {
            if (target == Target && defaultValue == DefaultValue)
            {
                return this;
            }
            return XzaarExpression.Label(target, defaultValue);
        }


        public override ExpressionType NodeType => ExpressionType.Label;
    }

    public partial class XzaarExpression
    {
        public static LabelExpression Label(LabelTarget target)
        {
            return Label(target, null);
        }

        public static LabelExpression Label(LabelTarget target, XzaarExpression defaultValue)
        {
            return new LabelExpression(target, defaultValue);
        }
    }
}