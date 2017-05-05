namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public class CreateStructExpression : XzaarExpression
    {
        internal CreateStructExpression(string structName)
        {
            StructName = structName;
        }

        public string StructName { get; }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.CreateStruct; }
        }
    }

    public partial class XzaarExpression
    {
        public static CreateStructExpression CreateStruct(string structName)
        {
            return new CreateStructExpression(structName);
        }
    }
}