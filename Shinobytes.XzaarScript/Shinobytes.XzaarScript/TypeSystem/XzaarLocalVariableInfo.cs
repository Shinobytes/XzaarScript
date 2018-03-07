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

        public virtual XzaarType LocalType => type;
        public virtual int LocalIndex => localIndex;
    }
}