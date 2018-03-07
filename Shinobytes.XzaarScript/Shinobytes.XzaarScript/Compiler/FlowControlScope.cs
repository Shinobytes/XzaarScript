using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.Compiler
{
    public class FlowControlScope
    {
        public Label CurrentStartLabel;
        public Label CurrentEndLabel;

        public int Depth;
        private FlowControlScope parent;
        private List<FlowControlScope> children;
        public FlowControlScope(FlowControlScope parent, int depth)
        {
            CurrentStartLabel = Instruction.Label();
            CurrentEndLabel = Instruction.Label();
            this.parent = parent;
            this.Depth = depth;
            this.children = new List<FlowControlScope>();
        }

        public FlowControlScope BeginControlBlock()
        {
            var subScope = new FlowControlScope(this, this.Depth + 1);
            this.children.Add(subScope);
            return subScope;
        }

        public FlowControlScope EndControlBlock()
        {
            return parent;
        }
    }
}