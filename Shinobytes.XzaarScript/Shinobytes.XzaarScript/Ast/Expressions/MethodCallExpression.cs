﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public class MethodCallExpression : XzaarExpression, IXzaarArgumentProvider
    {
        private readonly FunctionExpression _function;
        private XzaarMethodInfo _method;
        protected IList<XzaarExpression> _arguments;
        private int argumentCount;

        internal MethodCallExpression(XzaarMethodInfo method, IList<XzaarExpression> args = null)
        {
            if (_arguments == null) _arguments = new List<XzaarExpression>();
            _method = method;
        }

        internal MethodCallExpression(FunctionExpression method, IList<XzaarExpression> args = null)
        {
            if (_arguments == null) _arguments = new List<XzaarExpression>();
            this._function = method;
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Call; }
        }

        public virtual XzaarExpression GetInstance()
        {
            return null;
        }

        public override XzaarType Type
        {
            get
            {
                return _method == null ? _function.ReturnType : _method.ReturnType;
            }
        }
        public XzaarExpression Object
        {
            get { return GetInstance(); }
        }

        public string MethodName
        {
            get { return _method != null ? _method.Name : _function.Name; }
        }

        public XzaarMethodInfo Method { get { return _method; } }

        public FunctionExpression MethodExpression { get { return _function; } }

        public ReadOnlyCollection<XzaarExpression> Arguments
        {
            get { return GetOrMakeArguments(); }
        }
        internal virtual ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            throw new InvalidOperationException();
        }
        protected internal override XzaarExpression Accept(IXzaarExpressionVisitor visitor)
        {
            return visitor.Visit(this);
        }
        internal virtual MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            throw new InvalidOperationException();
        }
        public XzaarExpression GetArgument(int index)
        {
            return _arguments[index];
        }

        public int ArgumentCount
        {
            get { return _arguments.Count; }
        }
    }


    #region Specialized Subclasses

    internal class MethodCallExpressionN : MethodCallExpression, IXzaarArgumentProvider
    {
        public MethodCallExpressionN(XzaarMethodInfo method, IList<XzaarExpression> args)
            : base(method, args)
        {
            _arguments = args;
        }

        public MethodCallExpressionN(FunctionExpression method, IList<XzaarExpression> args)
                    : base(method, args)
        {
            _arguments = args;
        }

        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            return _arguments[index];
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return _arguments.Count;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(ref _arguments);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == _arguments.Count);

            return XzaarExpression.Call(Method, args ?? _arguments);
        }
    }

    internal class InstanceMethodCallExpressionN : MethodCallExpression, IXzaarArgumentProvider
    {
        private readonly XzaarExpression _instance;

        public InstanceMethodCallExpressionN(XzaarMethodInfo method, XzaarExpression instance, IList<XzaarExpression> args)
            : base(method)
        {
            _instance = instance;
            _arguments = args;
        }

        public InstanceMethodCallExpressionN(FunctionExpression method, XzaarExpression instance, IList<XzaarExpression> args)
            : base(method)
        {
            _instance = instance;
            _arguments = args;
        }

        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            return _arguments[index];
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return _arguments.Count;
            }
        }

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(ref _arguments);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == _arguments.Count);

            return XzaarExpression.Call(instance, Method, args ?? _arguments);
        }
    }

    internal class MethodCallExpression1 : MethodCallExpression, IXzaarArgumentProvider
    {
        private object _arg0;       // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider

        public MethodCallExpression1(XzaarMethodInfo method, XzaarExpression arg0)
            : base(method)
        {
            _arg0 = arg0;
        }
        public MethodCallExpression1(FunctionExpression method, XzaarExpression arg0)
            : base(method)
        {
            _arg0 = arg0;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 1;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 1);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0]);
            }

            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0));
        }
    }

    internal class MethodCallExpression2 : MethodCallExpression, IXzaarArgumentProvider
    {
        private object _arg0;               // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1;  // storage for the 2nd arg

        public MethodCallExpression2(XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        public MethodCallExpression2(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 2;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1);
        }
    }

    internal class MethodCallExpression3 : MethodCallExpression, IXzaarArgumentProvider
    {
        private object _arg0;           // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1, _arg2; // storage for the 2nd - 3rd args.

        public MethodCallExpression3(XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }


        public MethodCallExpression3(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 3;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2);
        }
    }

    internal class MethodCallExpression4 : MethodCallExpression, IXzaarArgumentProvider
    {
        private object _arg0;               // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1, _arg2, _arg3;  // storage for the 2nd - 4th args.

        public MethodCallExpression4(XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }
        public MethodCallExpression4(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 4;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 4);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2], args[3]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2, _arg3);
        }
    }

    internal class MethodCallExpression5 : MethodCallExpression, IXzaarArgumentProvider
    {
        private object _arg0;           // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1, _arg2, _arg3, _arg4;   // storage for the 2nd - 5th args.

        public MethodCallExpression5(XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3, XzaarExpression arg4)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }
        public MethodCallExpression5(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3, XzaarExpression arg4)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                case 4: return _arg4;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 5;
            }
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 5);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2], args[3], args[4]);
            }

            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2, _arg3, _arg4);
        }
    }

    internal class InstanceMethodCallExpression2 : MethodCallExpression, IXzaarArgumentProvider
    {
        private readonly XzaarExpression _instance;
        private object _arg0;                // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1;   // storage for the 2nd argument

        public InstanceMethodCallExpression2(XzaarMethodInfo method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            //Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
        }
        public InstanceMethodCallExpression2(FunctionExpression method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            //Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 2;
            }
        }

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance != null);
            //Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return XzaarExpression.Call(instance, Method, args[0], args[1]);
            }
            return XzaarExpression.Call(instance, Method, ReturnObject<XzaarExpression>(_arg0), _arg1);
        }
    }

    internal class InstanceMethodCallExpression3 : MethodCallExpression, IXzaarArgumentProvider
    {
        private readonly XzaarExpression _instance;
        private object _arg0;                       // storage for the 1st argument or a readonly collection.  See IXzaarArgumentProvider
        private readonly XzaarExpression _arg1, _arg2;   // storage for the 2nd - 3rd argument

        public InstanceMethodCallExpression3(XzaarMethodInfo method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            // Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }
        public InstanceMethodCallExpression3(FunctionExpression method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            // Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }
        XzaarExpression IXzaarArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                default: throw new InvalidOperationException();
            }
        }

        int IXzaarArgumentProvider.ArgumentCount
        {
            get
            {
                return 3;
            }
        }

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override MethodCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance != null);
            //Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return XzaarExpression.Call(instance, Method, args[0], args[1], args[2]);
            }
            return XzaarExpression.Call(instance, Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2);
        }
    }

    #endregion

    public partial class XzaarExpression
    {
        internal static T ReturnObject<T>(object collectionOrT) where T : class
        {
            T t = collectionOrT as T;
            if (t != null)
            {
                return t;
            }

            return ((ReadOnlyCollection<T>)collectionOrT)[0];
        }
        internal static ReadOnlyCollection<T> ReturnReadOnly<T>(ref IList<T> collection)
        {
            IList<T> value = collection;

            // if it's already read-only just return it.
            ReadOnlyCollection<T> res = value as ReadOnlyCollection<T>;
            if (res != null)
            {
                return res;
            }

            // otherwise make sure only readonly collection every gets exposed
            Interlocked.CompareExchange<IList<T>>(
                ref collection,
                new ReadOnlyCollection<T>(value),
                value
            );

            // and return it
            return (ReadOnlyCollection<T>)collection;
        }

        internal static ReadOnlyCollection<XzaarExpression> ReturnReadOnly(IXzaarArgumentProvider provider, ref object collection)
        {
            XzaarExpression tObj = collection as XzaarExpression;
            if (tObj != null)
            {
                // otherwise make sure only one readonly collection ever gets exposed
                Interlocked.CompareExchange(
                    ref collection,
                    new ReadOnlyCollection<XzaarExpression>(new ListArgumentProvider(provider, tObj)),
                    tObj
                );
            }

            // and return what is not guaranteed to be a readonly collection
            return (ReadOnlyCollection<XzaarExpression>)collection;
        }


        public static MethodCallExpression Call(FunctionExpression method, params XzaarExpression[] arguments)
        {

            if (method == null) throw new ArgumentNullException(nameof(method));

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments.ToList());

            ValidateMethodInfo(method);
            ValidateArgumentTypes(method, XzaarExpressionType.Call, ref argList);

            return new MethodCallExpressionN(method, argList);
        }

        //public static MethodCallExpression Call(XzaarExpression instance, FunctionExpression method, params XzaarExpression[] arguments)
        //{
        //    return Call(instance, method, arguments);
        //}


        public static MethodCallExpression Call(XzaarMethodInfo method, params XzaarExpression[] arguments)
        {
            return Call(null, method, arguments);
        }

        public static MethodCallExpression Call(XzaarMethodInfo method, IEnumerable<XzaarExpression> arguments)
        {
            return Call(null, method, arguments);
        }

        public static MethodCallExpression Call(XzaarExpression instance, XzaarMethodInfo method)
        {
            return Call(instance, method, EmptyReadOnlyCollection<XzaarExpression>.Instance);
        }

        public static MethodCallExpression Call(XzaarExpression instance, XzaarMethodInfo method, params XzaarExpression[] arguments)
        {
            return Call(instance, method, (IEnumerable<XzaarExpression>)arguments);
        }

        public static MethodCallExpression Call(XzaarExpression instance, XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));


            XzaarParameterInfo[] pis = ValidateMethodAndGetParameters(instance, method);

            ValidateArgumentCount(method, XzaarExpressionType.Call, 2, pis);

            arg0 = ValidateOneArgument(method, XzaarExpressionType.Call, arg0, pis[0]);
            arg1 = ValidateOneArgument(method, XzaarExpressionType.Call, arg1, pis[1]);

            if (instance != null)
            {
                return new InstanceMethodCallExpression2(method, instance, arg0, arg1);
            }

            return new MethodCallExpression2(method, arg0, arg1);
        }

        private static MethodCallExpression Call(XzaarExpression instance, XzaarMethodInfo method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));

            XzaarParameterInfo[] pis = ValidateMethodAndGetParameters(instance, method);

            ValidateArgumentCount(method, XzaarExpressionType.Call, 3, pis);

            arg0 = ValidateOneArgument(method, XzaarExpressionType.Call, arg0, pis[0]);
            arg1 = ValidateOneArgument(method, XzaarExpressionType.Call, arg1, pis[1]);
            arg2 = ValidateOneArgument(method, XzaarExpressionType.Call, arg2, pis[2]);

            if (instance != null)
            {
                return new InstanceMethodCallExpression3(method, instance, arg0, arg1, arg2);
            }
            return new MethodCallExpression3(method, arg0, arg1, arg2);
        }

        public static MethodCallExpression Call(XzaarExpression instance, string methodName, XzaarType[] typeArguments, params XzaarExpression[] arguments)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            if (arguments == null)
            {
                arguments = new XzaarExpression[0];
            }


            return XzaarExpression.Call(instance, FindMethod(instance.Type, methodName, typeArguments, arguments), arguments);
        }
        public static MethodCallExpression Call(XzaarType type, string methodName, XzaarType[] typeArguments, params XzaarExpression[] arguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (arguments == null) arguments = new XzaarExpression[] { };
            return XzaarExpression.Call(null, FindMethod(type, methodName, typeArguments, arguments), arguments);
        }

        public static MethodCallExpression Call(XzaarExpression instance, XzaarMethodInfo method, IEnumerable<XzaarExpression> arguments)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments.ToList());

            ValidateMethodInfo(method);
            ValidateStaticOrInstanceMethod(instance, method);
            ValidateArgumentTypes(method, XzaarExpressionType.Call, ref argList);

            if (instance == null)
            {
                return new MethodCallExpressionN(method, argList);
            }
            else
            {
                return new InstanceMethodCallExpressionN(method, instance, argList);
            }
        }

        private static void ValidateMethodInfo(XzaarMethodInfo method)
        {
            //if (method.IsGenericMethodDefinition)
            //    throw Error.MethodIsGeneric(method);
            //if (method.ContainsGenericParameters)
            //    throw Error.MethodContainsGenericParameters(method);
        }

        private static void ValidateMethodInfo(FunctionExpression method)
        {
            //if (method.IsGenericMethodDefinition)
            //    throw Error.MethodIsGeneric(method);
            //if (method.ContainsGenericParameters)
            //    throw Error.MethodContainsGenericParameters(method);
        }

        private static XzaarParameterInfo[] ValidateMethodAndGetParameters(XzaarExpression instance, XzaarMethodInfo method)
        {
            ValidateMethodInfo(method);
            ValidateStaticOrInstanceMethod(instance, method);

            return GetParametersForValidation(method, XzaarExpressionType.Call);
        }

        private static void ValidateStaticOrInstanceMethod(XzaarExpression instance, XzaarMethodInfo method)
        {
            if (method.IsGlobal)
            {
                if (instance != null) throw new InvalidOperationException(); // throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "instance");
            }
            else
            {
                if (instance == null) throw new InvalidOperationException(); // throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "method");
                RequiresCanRead(instance, "instance");
                ValidateCallInstanceType(instance.Type, method);
            }
        }

        private static void ValidateCallInstanceType(XzaarType instanceType, XzaarMethodInfo method)
        {
            //if (!XzaarTypeUtils.IsValidInstanceType(method, instanceType))
            //{
            //    throw Error.InstanceAndMethodTypeMismatch(method, method.DeclaringType, instanceType);
            //}
        }

        private static void ValidateArgumentTypes(FunctionExpression method, XzaarExpressionType nodeKind, ref ReadOnlyCollection<XzaarExpression> arguments)
        {
            // Debug.Assert(nodeKind == XzaarExpressionType.Invoke || nodeKind == XzaarExpressionType.Call || nodeKind == XzaarExpressionType.Dynamic || nodeKind == XzaarExpressionType.New);

            ParameterExpression[] pis = GetParametersForValidation(method, nodeKind);

            ValidateArgumentCount(method, nodeKind, arguments.Count, pis);

            XzaarExpression[] newArgs = null;
            for (int i = 0, n = pis.Length; i < n; i++)
            {
                XzaarExpression arg = arguments[i];
                ParameterExpression pi = pis[i];
                arg = ValidateOneArgument(method, nodeKind, arg, pi);

                if (newArgs == null && arg != arguments[i])
                {
                    newArgs = new XzaarExpression[arguments.Count];
                    for (int j = 0; j < i; j++)
                    {
                        newArgs[j] = arguments[j];
                    }
                }
                if (newArgs != null)
                {
                    newArgs[i] = arg;
                }
            }
            if (newArgs != null)
            {
                arguments = new TrueReadOnlyCollection<XzaarExpression>(newArgs);
            }
        }

        private static void ValidateArgumentTypes(XzaarMethodBase method, XzaarExpressionType nodeKind, ref ReadOnlyCollection<XzaarExpression> arguments)
        {
            // Debug.Assert(nodeKind == XzaarExpressionType.Invoke || nodeKind == XzaarExpressionType.Call || nodeKind == XzaarExpressionType.Dynamic || nodeKind == XzaarExpressionType.New);

            XzaarParameterInfo[] pis = GetParametersForValidation(method, nodeKind);

            ValidateArgumentCount(method, nodeKind, arguments.Count, pis);

            XzaarExpression[] newArgs = null;
            for (int i = 0, n = pis.Length; i < n; i++)
            {
                XzaarExpression arg = arguments[i];
                XzaarParameterInfo pi = pis[i];
                arg = ValidateOneArgument(method, nodeKind, arg, pi);

                if (newArgs == null && arg != arguments[i])
                {
                    newArgs = new XzaarExpression[arguments.Count];
                    for (int j = 0; j < i; j++)
                    {
                        newArgs[j] = arguments[j];
                    }
                }
                if (newArgs != null)
                {
                    newArgs[i] = arg;
                }
            }
            if (newArgs != null)
            {
                arguments = new TrueReadOnlyCollection<XzaarExpression>(newArgs);
            }
        }

        private static ParameterExpression[] GetParametersForValidation(FunctionExpression method, XzaarExpressionType nodeKind)
        {
            var pis = method.GetParameters().ToList();

            if (nodeKind == XzaarExpressionType.Dynamic)
            {
                pis.RemoveAt(0);
                // pis = pis.RemoveFirst(); // ignore CallSite argument
            }
            return pis.ToArray();
        }


        private static XzaarParameterInfo[] GetParametersForValidation(XzaarMethodBase method, XzaarExpressionType nodeKind)
        {
            var pis = method.GetParameters().ToList();

            if (nodeKind == XzaarExpressionType.Dynamic)
            {
                pis.RemoveAt(0);
                // pis = pis.RemoveFirst(); // ignore CallSite argument
            }
            return pis.ToArray();
        }
        private static void ValidateArgumentCount(FunctionExpression method, XzaarExpressionType nodeKind, int count, ParameterExpression[] pis)
        {
            if (pis.Length != count)
            {
                // Throw the right error for the node we were given
                switch (nodeKind)
                {
                    case XzaarExpressionType.New:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfConstructorArguments();
                    case XzaarExpressionType.Invoke:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfLambdaArguments();
                    case XzaarExpressionType.Dynamic:
                    case XzaarExpressionType.Call:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfMethodCallArguments(method);
                    default:
                        throw new InvalidOperationException();
                        // throw ContractUtils.Unreachable;
                }
            }
        }

        private static void ValidateArgumentCount(XzaarMethodBase method, XzaarExpressionType nodeKind, int count, XzaarParameterInfo[] pis)
        {
            if (pis.Length != count)
            {
                // Throw the right error for the node we were given
                switch (nodeKind)
                {
                    case XzaarExpressionType.New:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfConstructorArguments();
                    case XzaarExpressionType.Invoke:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfLambdaArguments();
                    case XzaarExpressionType.Dynamic:
                    case XzaarExpressionType.Call:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfMethodCallArguments(method);
                    default:
                        throw new InvalidOperationException();
                        // throw ContractUtils.Unreachable;
                }
            }
        }
        private static XzaarExpression ValidateOneArgument(FunctionExpression method, XzaarExpressionType nodeKind, XzaarExpression arg, ParameterExpression pi)
        {
            RequiresCanRead(arg, "arguments");
            XzaarType pType = pi.Type;
            if (pType.IsByRef)
            {
                pType = pType.GetElementType();
            }
            // XzaarTypeUtils.ValidateType(pType);
            if (!XzaarTypeUtils.AreReferenceAssignable(pType, arg.Type))
            {
                if (!TryQuote(pType, ref arg))
                {
                    // Throw the right error for the node we were given
                    switch (nodeKind)
                    {
                        case XzaarExpressionType.New:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchConstructorParameter(arg.Type, pType);
                        case XzaarExpressionType.Invoke:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchParameter(arg.Type, pType);
                        case XzaarExpressionType.Dynamic:
                        case XzaarExpressionType.Call:
                            throw new XzaarExpressionTransformerException("Expression type does not match method parameter: '" + arg.Type.Name + "' and '" + pType.Name + "'");
                        // throw Error.ExpressionTypeDoesNotMatchMethodParameter(arg.Type, pType, method);
                        default:
                            throw new InvalidOperationException();
                            // throw ContractUtils.Unreachable;
                    }
                }
            }
            return arg;
        }
        private static XzaarExpression ValidateOneArgument(XzaarMethodBase method, XzaarExpressionType nodeKind, XzaarExpression arg, XzaarParameterInfo pi)
        {
            RequiresCanRead(arg, "arguments");
            XzaarType pType = pi.ParameterType;
            if (pType.IsByRef)
            {
                pType = pType.GetElementType();
            }
            // XzaarTypeUtils.ValidateType(pType);
            if (!XzaarTypeUtils.AreReferenceAssignable(pType, arg.Type))
            {
                if (!TryQuote(pType, ref arg))
                {
                    // Throw the right error for the node we were given
                    switch (nodeKind)
                    {
                        case XzaarExpressionType.New:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchConstructorParameter(arg.Type, pType);
                        case XzaarExpressionType.Invoke:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchParameter(arg.Type, pType);
                        case XzaarExpressionType.Dynamic:
                        case XzaarExpressionType.Call:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchMethodParameter(arg.Type, pType, method);
                        default:
                            throw new InvalidOperationException();
                            // throw ContractUtils.Unreachable;
                    }
                }
            }
            return arg;
        }

        // Attempts to auto-quote the expression tree. Returns true if it succeeded, false otherwise.
        private static bool TryQuote(XzaarType parameterType, ref XzaarExpression argument)
        {
            //XzaarType quoteable = typeof(LambdaExpression);

            //if (XzaarTypeUtils.IsSameOrSubclass(quoteable, parameterType) &&
            //    parameterType.IsAssignableFrom(argument.GetType()))
            //{
            //    argument = XzaarExpression.Quote(argument);
            //    return true;
            //}
            return false;
        }

        private static XzaarMethodInfo FindMethod(XzaarType type, string methodName, XzaarType[] typeArgs, XzaarExpression[] args)
        {
            XzaarMemberInfo[] members = type.GetMethods();
            if (members == null || members.Length == 0)
                // throw Error.MethodDoesNotExistOnType(methodName, type);
                throw new InvalidOperationException();

            XzaarMethodInfo method;

            var methodInfos = members.Select(t => (XzaarMethodInfo)t);
            int count = FindBestMethod(methodInfos, typeArgs, args, out method);

            if (count == 0)
            {
                if (typeArgs != null && typeArgs.Length > 0)
                {
                    // throw Error.GenericMethodWithArgsDoesNotExistOnType(methodName, type);
                    throw new InvalidOperationException();
                }
                else
                {
                    // throw Error.MethodWithArgsDoesNotExistOnType(methodName, type);
                    throw new InvalidOperationException();
                }
            }
            if (count > 1)
                // throw Error.MethodWithMoreThanOneMatch(methodName, type);
                throw new InvalidOperationException();
            return method;
        }

        private static int FindBestMethod(IEnumerable<XzaarMethodInfo> methods, XzaarType[] typeArgs, XzaarExpression[] args, out XzaarMethodInfo method)
        {
            int count = 0;
            method = null;
            foreach (XzaarMethodInfo mi in methods)
            {
                XzaarMethodInfo moo = ApplyTypeArgs(mi, typeArgs);
                if (moo != null && IsCompatible(moo, args))
                {
                    // favor public over non-public methods
                    if (method == null)
                    {
                        method = moo;
                        count = 1;
                    }
                    // only count it as additional method if they both public or both non-public
                    else
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static bool IsCompatible(XzaarMethodBase m, XzaarExpression[] args)
        {
            XzaarParameterInfo[] parms = m.GetParameters();
            if (parms.Length != args.Length)
                return false;
            for (int i = 0; i < args.Length; i++)
            {
                XzaarExpression arg = args[i];
                //ContractUtils.RequiresNotNull(arg, "argument");
                XzaarType argType = arg.Type;
                XzaarType pType = parms[i].ParameterType;
                if (pType.IsByRef)
                {
                    pType = pType.GetElementType();
                }
                if (!XzaarTypeUtils.AreReferenceAssignable(pType, argType)) // &&  !(XzaarTypeUtils.IsSameOrSubclass(typeof(LambdaExpression), pType) && pType.IsAssignableFrom(arg.GetType()))
                {
                    return false;
                }
            }
            return true;
        }

        private static XzaarMethodInfo ApplyTypeArgs(XzaarMethodInfo m, XzaarType[] typeArgs)
        {
            if (typeArgs == null || typeArgs.Length == 0)
            {
                return m;
            }
            return null;
        }

        public static MethodCallExpression ArrayIndex(XzaarExpression array, params XzaarExpression[] indexes)
        {
            return ArrayIndex(array, (IEnumerable<XzaarExpression>)indexes);
        }

        public static MethodCallExpression ArrayIndex(XzaarExpression array, IEnumerable<XzaarExpression> indexes)
        {
            // RequiresCanRead(array, "array");
            // ContractUtils.RequiresNotNull(indexes, "indexes");

            XzaarType arrayType = array.Type;
            if (!arrayType.IsArray)
            {
                // throw Error.ArgumentMustBeArray();
                throw new InvalidOperationException();
            }

            ReadOnlyCollection<XzaarExpression> indexList = new ReadOnlyCollection<XzaarExpression>(indexes.ToList());
            // totally ignored
            //if (arrayType.GetArrayRank() != indexList.Count)
            //{
            //    //throw Error.IncorrectNumberOfIndexes();
            //    throw new InvalidOperationException();
            //}

            foreach (XzaarExpression e in indexList)
            {
                RequiresCanRead(e, "indexes");
                if (e.Type != (XzaarType)typeof(int))
                {
                    // throw Error.ArgumentMustBeArrayIndexType();
                    throw new InvalidOperationException();
                }
            }

            XzaarMethodInfo mi = array.Type.GetMethod("Get");

            return Call(array, mi, indexList);
        }
    }
}