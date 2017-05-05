namespace Shinobytes.XzaarScript.Ast.Expressions
{
    // ForEach

    public class ForExpression : LoopExpression
    {
        internal ForExpression(XzaarExpression initiator, XzaarExpression test, XzaarExpression incrementor, XzaarExpression body) : base(body, null, null, LoopTypes.For)
        {
            Incrementor = incrementor;
            Test = test;
            Initiator = initiator;
        }

        public XzaarExpression Incrementor { get; set; }
        public XzaarExpression Test { get; set; }
        public XzaarExpression Initiator { get; set; }
    }

    public partial class XzaarExpression
    {
        public static ForExpression For(XzaarExpression initiator, XzaarExpression test, XzaarExpression incrementor, XzaarExpression body)
        {
            return new ForExpression(initiator, test, incrementor, body);
        }
    }
}