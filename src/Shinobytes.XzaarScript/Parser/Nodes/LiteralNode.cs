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
 
using System;
using System.Globalization;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LiteralNode : AstNode
    {
        public LiteralNode(string nodeName, object value, int nodeIndex)
            : base(SyntaxKind.LiteralExpression, nodeName, value, nodeIndex)
        {
            if (this.Type == null)
            {
                this.Type = "any";
            }

            if (this.Type.ToLower() == "any")
            {
                var nodeNameLower = nodeName.ToLower();
                switch (nodeNameLower)
                {
                    case "string":
                        this.Type = "string";
                        break;
                    case "number":
                        this.Type = "number";
                        break;
                }
            }

            if (this.Type.ToLower() == "number")
            {
                this.Value = ParseNumberValue(value?.ToString());
            }
        }

        private object ParseNumberValue(string value)
        {
            if (value == null) return 0;
            var str = value.ToLowerInvariant();
            if (str.Contains("."))
            {
                try
                {
                    return double.Parse(value, CultureInfo.InvariantCulture);
                }
                catch
                {
                    return 0;
                }
            }

            if (str.StartsWith("0x"))
            {
                return ParseHexNumber(value);
            }

            if (str.StartsWith("0b"))
            {
                return ParseBinaryNumber(value);
            }

            return int.TryParse(value, out var i) ? i : 0;
        }

        private double ParseBinaryNumber(string bin) => Convert.ToInt32(bin, 2);

        private int ParseHexNumber(string hex) => Convert.ToInt32(hex.Substring(2), 16);

        public override string ToString()
        {
            if (this.NodeName == "STRING")
            {
                return "\"" + Value + "\"";
            }
            return "" + Value;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}