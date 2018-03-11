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

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class TokenStream : CollectionStream<SyntaxToken>
    {
        public TokenStream(IList<SyntaxToken> items)
            : base(items)
        {
        }

        public bool NextIs(SyntaxKind kind)
        {
            var peekNext = PeekNext();
            return peekNext != null && peekNext.Kind == kind;
        }

        public bool CurrentIs(SyntaxKind kind)
        {
            return Current != null && Current.Kind == kind;
        }

        public SyntaxToken Consume(SyntaxKind kind)
        {
            return this.Consume(x => x.Kind == kind);
        }

        public SyntaxToken ConsumeExpected(SyntaxKind kind)
        {
            return this.ConsumeExpected(x => x.Kind == kind);
        }
    }
}