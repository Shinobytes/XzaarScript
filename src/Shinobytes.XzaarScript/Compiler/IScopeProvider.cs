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

namespace Shinobytes.XzaarScript.Compiler
{
    public interface IScopeProvider
    {
        /// <summary>
        /// Gets the target inner scope by name. If it doesnt exist, it will be created.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scopeType"></param>
        /// <returns></returns>
        ExpressionScope Get(string name, ExpressionScopeType scopeType);

        ExpressionScope Current { get; }

        void EndScope(ExpressionScope expressionScope);
    }
}