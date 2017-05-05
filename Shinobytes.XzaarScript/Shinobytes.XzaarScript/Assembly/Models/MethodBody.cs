namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class MethodBody
    {
        private readonly MethodDefinition method;

        internal MethodBody()
        {
            MethodVariables = new MethodVariableCollection();
            MethodInstructions = new MethodInstructionCollection();
        }

        public MethodBody(MethodDefinition method) : this()
        {
            this.method = method;
        }

        public MethodInstructionCollection MethodInstructions { get; }
        public MethodVariableCollection MethodVariables { get; }

        public bool IsEmpty
        {
            get { return MethodInstructions.Count == 0; }
        }

        public bool HasVariables
        {
            get { return MethodVariables.Count > 0; }
        }

        public MethodDefinition GetMethod()
        {
            return method;
        }
    }
}