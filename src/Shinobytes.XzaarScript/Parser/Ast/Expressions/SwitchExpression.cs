/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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