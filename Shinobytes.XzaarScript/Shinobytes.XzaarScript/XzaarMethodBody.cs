using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript
{
    public class XzaarMethodBody
    {
        private XzaarLocalVariableInfo[] localVariables;

        internal XzaarMethodBase methodBase;

        protected XzaarMethodBody() { }

        public virtual IReadOnlyList<XzaarLocalVariableInfo> LocalVariables { get { return localVariables.ToList(); } }
    }
}