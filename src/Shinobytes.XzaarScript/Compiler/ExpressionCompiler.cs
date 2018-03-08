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

using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Ast;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler
{
    public class ExpressionCompiler : NodeToExpressionVisitor, IExpressionCompiler
    {
        //private readonly XzaarExpression _tree = new XzaarExpression();
        //private readonly Stack<Scope> _scopes = new Stack<Scope>();
        //private readonly Stack<BoundConstants> _constants = new Stack<BoundConstants>();

        public XzaarExpression Compile(AstNode entry)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            // 3. compile expressions
            return Bind(entry);
        }

        private XzaarExpression Bind(AstNode entry)
        {
            var binder = new NodeTypeBinder();
            var boundNode = binder.Process(entry);
            var program = Visit(boundNode);
            return program;
        }
    }
}