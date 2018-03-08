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
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetFlowControlScope
    {
        public Label CurrentStartLabel;
        public Label CurrentEndLabel;

        public int Depth;
        private readonly DotNetFlowControlScope parent;
        private readonly List<DotNetFlowControlScope> children;
        public DotNetFlowControlScope(DotNetFlowControlScope parent, int depth, Label start, Label end)
        {
            this.CurrentStartLabel = start;
            this.CurrentEndLabel = end;
            this.parent = parent;
            this.Depth = depth;
            this.children = new List<DotNetFlowControlScope>();
        }

        public DotNetFlowControlScope BeginControlBlock(Label start, Label end)
        {
            var subScope = new DotNetFlowControlScope(this, this.Depth + 1, start, end);
            this.children.Add(subScope);
            return subScope;
        }

        public DotNetFlowControlScope EndControlBlock()
        {
            return parent;
        }
    }
}