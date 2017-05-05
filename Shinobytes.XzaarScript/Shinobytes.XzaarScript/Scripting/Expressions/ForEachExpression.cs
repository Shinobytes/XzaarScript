namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public class ForEachExpression : LoopExpression
    {
        internal ForEachExpression(XzaarExpression variable, XzaarExpression collection, XzaarExpression body) : base(body, null, null, XzaarLoopTypes.Foreach)
        {
            Variable = variable;
            Collection = collection;
        }

        public XzaarExpression Variable { get; set; }
        public XzaarExpression Collection { get; set; }
    }

    public partial class XzaarExpression
    {
        public static ForEachExpression ForEach(XzaarExpression variable, XzaarExpression collection, XzaarExpression body)
        {
            return new ForEachExpression(variable, collection, body);
        }
    }
}