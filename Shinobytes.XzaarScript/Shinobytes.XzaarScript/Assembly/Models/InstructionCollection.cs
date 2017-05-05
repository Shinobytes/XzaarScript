namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class InstructionCollection : Collection<XzaarBinaryCode>
    {
        private int currentInstructionIndex;
        public override void Add(XzaarBinaryCode item)
        {
            item.Offset = currentInstructionIndex++;
            base.Add(item);
        }
    }
}