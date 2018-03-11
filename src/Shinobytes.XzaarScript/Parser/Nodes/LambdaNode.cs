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
    public class LambdaNode : AstNode
    {
        public LambdaNode(FunctionParametersNode parameters, AstNode body, bool isSimple, int nodeIndex)
            : base(SyntaxKind.LambdaFunctionDefinitionExpression, null, null, nodeIndex)
        {
            Parameters = parameters;
            Body = body;
            this.HasCurlyBrackets = body is BlockNode;
            this.IsSimple = isSimple;
        }

        public bool HasCurlyBrackets { get; }

        public FunctionParametersNode Parameters { get; }

        public AstNode Body { get; private set; }

        public bool IsSimple { get; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Body == null || this.Body.IsEmpty();
        }


        public override string ToString()
        {
            var parameters = this.Parameters.Parameters.Select(x => x.ToString());
            var str0 = $"({string.Join(", ", parameters.ToArray())}) => ";
            return HasCurlyBrackets ? str0 + "{" + Body + "}" : str0 + Body;
        }
    }
}