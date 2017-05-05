namespace Shinobytes.XzaarScript
{
    public class XzaarLocalVariableInfo
    {
        private XzaarType type;
        private int localIndex;

        protected XzaarLocalVariableInfo() { }

        public override string ToString()
        {
            return LocalType + " (" + LocalIndex + ")";
        }

        public virtual XzaarType LocalType { get { return type; } }
        public virtual int LocalIndex { get { return localIndex; } }
    }
}