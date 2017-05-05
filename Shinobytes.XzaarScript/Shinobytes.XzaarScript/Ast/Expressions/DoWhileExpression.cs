namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class DoWhileExpression : LoopExpression
    {
        internal DoWhileExpression(XzaarExpression test, XzaarExpression body) : base(body, null, null, XzaarLoopTypes.DoWhile)
        {
            Test = test;
        }

        public XzaarExpression Test { get; set; }
    }

    public partial class XzaarExpression
    {
        public static DoWhileExpression DoWhile(XzaarExpression test, XzaarExpression body)
        {
            return new DoWhileExpression(test, body);
        }
    }
}