using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class ParameterExpression : XzaarExpression
    {
        private string _name;

        internal ParameterExpression(string name)
        {
            _name = name;
        }
        public override XzaarType Type
        {
            get { return XzaarBaseTypes.Any; }
        }
        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Parameter; }
        }
        public string Name
        {
            get { return _name; }
        }

        public XzaarExpression ArrayIndex { get; set; }

        internal static ParameterExpression Make(XzaarType type, string name, bool isByRef)
        {
            if (isByRef)
            {                
                return new ByRefParameterExpression(type, name);
            }
            else
            {
                //if (!type.IsEnum)
                {
                    switch (XzaarType.GetTypeCode(type))
                    {
                        case XzaarTypeCode.Boolean: return new PrimitiveParameterExpression<Boolean>(name);
                        case XzaarTypeCode.Byte: return new PrimitiveParameterExpression<Byte>(name);
                        case XzaarTypeCode.Char: return new PrimitiveParameterExpression<Char>(name);
                        case XzaarTypeCode.DateTime: return new PrimitiveParameterExpression<DateTime>(name);                        
                        case XzaarTypeCode.Decimal: return new PrimitiveParameterExpression<Decimal>(name);
                        case XzaarTypeCode.Double: return new PrimitiveParameterExpression<Double>(name);
                        case XzaarTypeCode.Int16: return new PrimitiveParameterExpression<Int16>(name);
                        case XzaarTypeCode.Int32: return new PrimitiveParameterExpression<Int32>(name);
                        case XzaarTypeCode.Int64: return new PrimitiveParameterExpression<Int64>(name);
                        
                        case XzaarTypeCode.Any:
                            // common reference types which we optimize go here.  Of course object is in
                            // the list, the others are driven by profiling of various workloads.  This list
                            // should be kept short.
                            if (type == XzaarBaseTypes.Any)
                            {
                                return new ParameterExpression(name);
                            }
                            //else if (type == typeof(Exception))
                            //{
                            //    return new PrimitiveParameterExpression<Exception>(name);
                            //}
                            //else if (type == typeof(object[]))
                            //{
                            //    return new PrimitiveParameterExpression<object[]>(name);
                            //}
                            break;
                        case XzaarTypeCode.SByte: return new PrimitiveParameterExpression<SByte>(name);
                        case XzaarTypeCode.Single: return new PrimitiveParameterExpression<Single>(name);
                        case XzaarTypeCode.String: return new PrimitiveParameterExpression<String>(name);
                        case XzaarTypeCode.UInt16: return new PrimitiveParameterExpression<UInt16>(name);
                        case XzaarTypeCode.UInt32: return new PrimitiveParameterExpression<UInt32>(name);
                        case XzaarTypeCode.UInt64: return new PrimitiveParameterExpression<UInt64>(name);
                    }
                }
            }

            return new TypedParameterExpression(type, name);
        }

    }
    internal class TypedParameterExpression : ParameterExpression
    {
        private readonly XzaarType _paramType;

        internal TypedParameterExpression(XzaarType type, string name)
            : base(name)
        {
            _paramType = type;
        }

        public sealed override XzaarType Type
        {
            get { return _paramType; }
        }
    }

    internal sealed class ByRefParameterExpression : TypedParameterExpression
    {
        internal ByRefParameterExpression(XzaarType type, string name)
            : base(type, name)
        {
        }

        //internal override bool GetIsByRef()
        //{
        //    return true;
        //}
    }

    internal sealed class PrimitiveParameterExpression<T> : ParameterExpression
    {
        internal PrimitiveParameterExpression(string name)
            : base(name)
        {
        }

        public sealed override XzaarType Type
        {
            get { return XzaarBaseTypes.Typeof(typeof(T)); }
        }
    }

    public partial class XzaarExpression
    {
        public static ParameterExpression Parameter(XzaarType type)
        {
            return Parameter(type, null);
        }
        public static ParameterExpression Variable(XzaarType type)
        {
            return Variable(type, null);
        }
        public static ParameterExpression Variable(XzaarType type, string name)
        {
            if (type == (XzaarType)typeof(void)) throw new InvalidOperationException("Argument cannot be of type void");
            if (type.IsByRef) throw  new InvalidOperationException("Type must not be by ref");// Error.TypeMustNotBeByRef();
            return ParameterExpression.Make(type, name, false);
        }
        public static ParameterExpression Parameter(XzaarType type, string name)
        {
           
            if (type == XzaarBaseTypes.Void)
            {
                // throw Error.ArgumentCannotBeOfTypeVoid();
                throw new InvalidOperationException("Argument cannot be of type void");
            }

            bool byref = type.IsByRef;
            if (byref)
            {
                type = type.GetElementType();
            }

            return ParameterExpression.Make(type, name, byref);
        }

    }
}