namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class VariableDefinitionExpression : ParameterExpression
    {
        private XzaarType type;

        internal VariableDefinitionExpression(XzaarType type, string name, XzaarExpression assignmentExpression)
            : base(name)
        {
            this.type = type;            
            AssignmentExpression = assignmentExpression;
        }

        public override XzaarType Type { get { return type; } }        

        public XzaarExpression AssignmentExpression { get; }        
    }

    public partial class XzaarExpression
    {
        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name)
        {
            return DefineVariable(type, name, null);
        }

        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name, XzaarExpression assignmentExpression)
        {
            return new VariableDefinitionExpression(type, name, assignmentExpression);
        }
    }
}