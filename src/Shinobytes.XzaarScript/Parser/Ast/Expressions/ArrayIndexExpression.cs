/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

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

//        public override ExpressionType Kind
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