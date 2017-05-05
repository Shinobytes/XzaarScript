//namespace Shinobytes.XzaarScript.Scripting.Expressions
//{
//    public class ArrayIndexExpression : XzaarExpression
//    {
//        private readonly XzaarExpression array;
//        private readonly XzaarExpression index;
//        private readonly XzaarType elementType;
//        private ExpressionType nodeType;

//        internal ArrayIndexExpression(XzaarExpression array, XzaarExpression index)
//        {
//            this.array = array;
//            this.index = index;
//            this.array.Type.GetElementType();
//        }

//        public XzaarExpression Array
//        {
//            get { return array; }
//        }

//        public XzaarExpression Index
//        {
//            get { return index; }
//        }

//        public override XzaarType Type
//        {
//            get
//            {
//                return elementType;
//            }
//        }

//        public override ExpressionType NodeType
//        {
//            get { return ExpressionType.ArrayIndex; }
//        }
//    }

//    public partial class XzaarExpression
//    {
//        public static ArrayIndexExpression ArrayIndex(XzaarExpression array, XzaarExpression index)
//        {
//            return new ArrayIndexExpression(array, index);
//        }
//    }
//}