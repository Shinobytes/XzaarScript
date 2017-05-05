namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class CreateStructExpression : XzaarExpression
    {
        internal CreateStructExpression(string structName)
        {
            StructName = structName;
        }

        internal CreateStructExpression(string structName, XzaarExpression[] fieldInitializers)
        {
            StructName = structName;
            FieldInitializers = fieldInitializers;
        }

        public string StructName { get; }

        public XzaarExpression[] FieldInitializers { get; set; }

        public override ExpressionType NodeType => ExpressionType.CreateStruct;
    }

    public partial class XzaarExpression
    {
        public static CreateStructExpression CreateStruct(string structName)
        {
            return new CreateStructExpression(structName);
        }

        public static CreateStructExpression CreateStruct(string structName, XzaarExpression[] fieldInitializers)
        {
            return new CreateStructExpression(structName, fieldInitializers);
        }
    }
}