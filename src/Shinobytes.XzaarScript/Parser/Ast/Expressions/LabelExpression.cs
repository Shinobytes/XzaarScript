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