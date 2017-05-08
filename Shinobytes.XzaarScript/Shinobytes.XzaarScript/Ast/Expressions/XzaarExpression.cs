using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public abstract partial class XzaarExpression
    {
        public bool IsEmpty()
        {
            var block = this as BlockExpression;
            if (block != null && block.Expressions.Count == 0) return true;
            return this is DefaultExpression;
        }

        public virtual ExpressionType NodeType { get; }

        public virtual XzaarType Type { get; }

        public static void RequiresCanWrite(XzaarExpression expression, string paramName)
        {
        }

        private static void RequiresCanRead(XzaarExpression expression, string paramName)
        {
            //if (expression == null)
            //{
            //    throw new ArgumentNullException(paramName);
            //}

            //// validate that we can read the node
            //switch (expression.Kind)
            //{
            //    case ExpressionType.Index:
            //        IndexExpression index = (IndexExpression)expression;
            //        if (index.Indexer != null && !index.Indexer.CanRead)
            //        {
            //            throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
            //        }
            //        break;
            //    case ExpressionType.MemberAccess:
            //        MemberExpression member = (MemberExpression)expression;
            //        XzaarMemberInfo memberInfo = member.Member;
            //        if (memberInfo.MemberType == XzaarMemberTypes.Property)
            //        {
            //            XzaarPropertyInfo prop = (XzaarPropertyInfo)memberInfo;
            //            if (!prop.CanRead)
            //            {
            //                throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
            //            }
            //        }
            //        break;
            //}
        }
    }
}
