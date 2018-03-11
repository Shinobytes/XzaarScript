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

namespace Shinobytes.XzaarScript.Compiler
{
    public class DefaultScopeProvider : IScopeProvider
    {
        private const string GlobalScopeIdentifier = "$GLOBAL";

        private ExpressionScope globalScope;

        private readonly Dictionary<string, ExpressionScope> scopes
            = new Dictionary<string, ExpressionScope>();

        public DefaultScopeProvider()
        {
            this.Current = globalScope = new ExpressionScope(this, null, GlobalScopeIdentifier, ExpressionScopeType.Global, 0);
        }

        public ExpressionScope Current { get; private set; }

        public ExpressionScope Get(string name, ExpressionScopeType scopeType)
        {
            var depth = this.Current.Depth + 1;
            var scopeKey = depth + name + "_" + scopeType;
            if (scopes.TryGetValue(scopeKey, out var scope))
            {
                return scope;
            }

            return Current = scopes[scopeKey] = new ExpressionScope(this, Current, name, scopeType, depth);
        }

        public void EndScope(ExpressionScope expressionScope)
        {
            Current = expressionScope.Parent;
        }
    }
}