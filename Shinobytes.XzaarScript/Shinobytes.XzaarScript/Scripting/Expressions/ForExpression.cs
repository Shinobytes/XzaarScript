namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    // ForEach

    public class ForExpression : LoopExpression
    {
        internal ForExpression(XzaarExpression initiator, XzaarExpression test, XzaarExpression incrementor, XzaarExpression body) : base(body, null, null, XzaarLoopTypes.For)
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