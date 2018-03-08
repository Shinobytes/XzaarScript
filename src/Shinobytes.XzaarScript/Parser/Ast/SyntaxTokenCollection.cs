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

using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Compiler.Extensions.Properties;

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class SyntaxTokenCollection : Collection<SyntaxToken>
    {
        private int currentTokenIndex;
        private int currentTokenOffset;
        private int currentTokenLine;
        private int currentTokenColumn;

        public override void Add([NotNull] SyntaxToken item)
        {
            base.Add(new SyntaxToken(item.Kind, item.TypeName, item.Value, currentTokenIndex++, currentTokenOffset, currentTokenLine, currentTokenColumn));
            currentTokenOffset += item.Value.Length;
        }

        public void AdvanceSourcePosition(int count = 1)
        {
            currentTokenOffset += count;
            currentTokenColumn += count;
        }

        public void AdvanceSourceNewLine()
        {
            currentTokenLine++;
            currentTokenColumn = 0;
        }
    }
}