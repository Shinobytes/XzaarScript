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
    public class SwitchExpression : XzaarExpression
    {
        private readonly XzaarExpression value;
        private readonly XzaarExpression[] caseExpressions;

        internal SwitchExpression(XzaarExpression value, XzaarExpression[] caseExpressions)
        {
            this.value = value;
            this.caseExpressions = caseExpressions;
        }

        public XzaarExpression Value => value;
        public XzaarExpression[] Cases => caseExpressions;

        public override ExpressionType NodeType => ExpressionType.Switch;
    }

    public class SwitchCaseExpression : XzaarExpression
    {
        private readonly XzaarExpression matchExpression;
        private readonly XzaarExpression body;

        internal SwitchCaseExpression(XzaarExpression matchExpression, XzaarExpression body)
        {
            this.matchExpression = matchExpression;
            this.body = body;
        }
        public bool IsDefaultCase => matchExpression == null;
        public XzaarExpression Match => matchExpression;
        public XzaarExpression Body => body;

        public override ExpressionType NodeType => ExpressionType.SwitchCase;
    }

    public partial class XzaarExpression
    {
        public static SwitchCaseExpression Case(XzaarExpression test, XzaarExpression body)
        {
            return new SwitchCaseExpression(test, body);
        }

        public static SwitchCaseExpression DefaultCase(XzaarExpression body)
        {
            return new SwitchCaseExpression(null, body);
        }

        public static SwitchExpression Switch(XzaarExpression valueExpression, params XzaarExpression[] caseExpressions)
        {
            return new SwitchExpression(valueExpression, caseExpressions);
        }
    }
}