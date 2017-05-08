using System;
using System.Globalization;
using Shinobytes.XzaarScript.Ast;

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