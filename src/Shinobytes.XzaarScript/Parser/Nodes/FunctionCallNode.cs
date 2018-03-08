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

using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class FunctionCallNode : AstNode
    {
        private readonly AstNode function;

        public FunctionCallNode(AstNode instance, AstNode function, int nodeIndex, ArgumentNode[] args)
            : base(SyntaxKind.FunctionInvocation, "FUNCTION", function.Value, nodeIndex)
        {
            this.Instance = instance;
            this.function = function;
            if (args.Length > 0)
                this.AddChildren(args);
        }

        public ArgumentNode[] Arguments => this.Children != null && this.Children.Count > 0 ? this.Children.Cast<ArgumentNode>().ToArray() : new ArgumentNode[0];

        public AstNode Function => function;

        public AstNode Instance { get; set; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            //if (this.Instance != null)
            //{
            //    return this.Instance + "." + this.function + "(" +
            //           string.Join(", ", this.Arguments.Select(i => i.ToString())) + ")";
            //}
            return this.function + "(" +
                   string.Join(", ", this.Arguments.Select(x => x.ToString()).ToArray()) + ")";
        }        
    }
}