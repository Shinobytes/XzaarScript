namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class WhileExpression : LoopExpression
    {
        internal WhileExpression(XzaarExpression test, XzaarExpression body) : base(body, null, null, XzaarLoopTypes.While)
        {
            Test = test;
        }

        public XzaarExpression Test { get; set; }
    }

    public partial class XzaarExpression
    {
        public static WhileExpression While(XzaarExpression test, XzaarExpression body)
        {
            return new WhileExpression(test, body);
        }
    }
}