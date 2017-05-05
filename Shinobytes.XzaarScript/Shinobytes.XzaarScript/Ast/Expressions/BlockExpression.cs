using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Expression = Shinobytes.XzaarScript.Ast.Expressions.XzaarExpression;
namespace Shinobytes.XzaarScript.Ast.Expressions
{
    sealed class TrueReadOnlyCollection<T> : ReadOnlyCollection<T>
    {
        /// <summary>
        /// Creates instnace of TrueReadOnlyCollection, wrapping passed in array.
        /// !!! DOES NOT COPY THE ARRAY !!!
        /// </summary>
        internal TrueReadOnlyCollection(T[] list)
            : base(list)
        {
        }
    }

    internal static class EmptyReadOnlyCollection<T>
    {
        internal static ReadOnlyCollection<T> Instance = new TrueReadOnlyCollection<T>(new T[0]);
    }

    public class BlockExpression : XzaarExpression
    {       
        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Block; }
        }

        public override XzaarType Type
        {
            get
            {
                if (ExpressionCount == 0) return null;
                return GetExpression(ExpressionCount - 1).Type; }
        }

        internal virtual XzaarExpression GetExpression(int index)
        {
            throw new InvalidOperationException();
        }

        internal virtual int ExpressionCount
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        internal virtual BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
        {
            throw new InvalidOperationException();
        }

        internal virtual ReadOnlyCollection<XzaarExpression> GetOrMakeExpressions()
        {
            throw new InvalidOperationException();
        }

        internal virtual ParameterExpression GetVariable(int index)
        {

            throw new InvalidOperationException();
        }

        internal virtual int VariableCount
        {
            get
            {
                return 0;
            }
        }
        

        internal static ReadOnlyCollection<XzaarExpression> ReturnReadOnlyExpressions(BlockExpression provider, ref object collection)
        {
            XzaarExpression tObj = collection as XzaarExpression;
            if (tObj != null)
            {
                // otherwise make sure only one readonly collection ever gets exposed
                Interlocked.CompareExchange(
                    ref collection,
                    new ReadOnlyCollection<XzaarExpression>(new BlockExpressionList(provider, tObj)),
                    tObj
                );
            }

            // and return what is not guaranteed to be a readonly collection
            return (ReadOnlyCollection<XzaarExpression>)collection;
        }
        internal static T ReturnObject<T>(object collectionOrT) where T : class
        {
            T t = collectionOrT as T;
            if (t != null)
            {
                return t;
            }

            return ((ReadOnlyCollection<T>)collectionOrT)[0];
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

        internal virtual ReadOnlyCollection<ParameterExpression> GetOrMakeVariables()
        {
            return EmptyReadOnlyCollection<ParameterExpression>.Instance;
        }

        public ReadOnlyCollection<XzaarExpression> Expressions
        {
            get { return GetOrMakeExpressions(); }
        }

        public ReadOnlyCollection<ParameterExpression> Variables
        {
            get
            {
                return GetOrMakeVariables();
            }
        }

        public XzaarExpression Result
        {
            get
            {
                return GetExpression(ExpressionCount - 1);
            }
        }

    }
    internal sealed class Block2 : BlockExpression
    {
        private object _arg0;                   // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly Expression _arg1;      // storage for the 2nd argument.

        internal Block2(Expression arg0, Expression arg1)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        internal override Expression GetExpression(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<Expression>(_arg0);
                case 1: return _arg1;
                default: throw new InvalidOperationException();
            }
        }

        internal override int ExpressionCount
        {
            get
            {
                return 2;
            }
        }

        internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
        {
            return ReturnReadOnlyExpressions(this, ref _arg0);
        }

        internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
        {
            Debug.Assert(args.Length == 2);
            Debug.Assert(variables == null || variables.Count == 0);

            return new Block2(args[0], args[1]);
        }
    }

    internal sealed class Block3 : BlockExpression
    {
        private object _arg0;                       // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly Expression _arg1, _arg2;   // storage for the 2nd and 3rd arguments.

        internal Block3(Expression arg0, Expression arg1, Expression arg2)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        internal override Expression GetExpression(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<Expression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                default: throw new InvalidOperationException();
            }
        }

        internal override int ExpressionCount
        {
            get
            {
                return 3;
            }
        }

        internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
        {
            return ReturnReadOnlyExpressions(this, ref _arg0);
        }

        internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
        {
            Debug.Assert(args.Length == 3);
            Debug.Assert(variables == null || variables.Count == 0);

            return new Block3(args[0], args[1], args[2]);
        }
    }

    internal sealed class Block4 : BlockExpression
    {
        private object _arg0;                               // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly Expression _arg1, _arg2, _arg3;    // storarg for the 2nd, 3rd, and 4th arguments.

        internal Block4(Expression arg0, Expression arg1, Expression arg2, Expression arg3)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }

        internal override Expression GetExpression(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<Expression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                default: throw new InvalidOperationException();
            }
        }

        internal override int ExpressionCount
        {
            get
            {
                return 4;
            }
        }

        internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
        {
            return ReturnReadOnlyExpressions(this, ref _arg0);
        }

        internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
        {
            Debug.Assert(args.Length == 4);
            Debug.Assert(variables == null || variables.Count == 0);

            return new Block4(args[0], args[1], args[2], args[3]);
        }
    }

    internal sealed class Block5 : BlockExpression
    {
        private object _arg0;                                       // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly Expression _arg1, _arg2, _arg3, _arg4;     // storage for the 2nd - 5th args.

        internal Block5(Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }

        internal override Expression GetExpression(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<Expression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                case 4: return _arg4;
                default: throw new InvalidOperationException();
            }
        }

        internal override int ExpressionCount
        {
            get
            {
                return 5;
            }
        }

        internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
        {
            return ReturnReadOnlyExpressions(this, ref _arg0);
        }

        internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
        {
            Debug.Assert(args.Length == 5);
            Debug.Assert(variables == null || variables.Count == 0);

            return new Block5(args[0], args[1], args[2], args[3], args[4]);
        }
    }

    internal class BlockN : BlockExpression
    {
        private IList<XzaarExpression> _expressions;         // either the original IList<Expression> or a ReadOnlyCollection if the user has accessed it.

        internal BlockN(IList<XzaarExpression> expressions)
        {
           // Debug.Assert(expressions.Count != 0);

            _expressions = expressions;
        }

        internal override XzaarExpression GetExpression(int index)
        {
           // Debug.Assert(index >= 0 && index < _expressions.Count);

            return _expressions[index];
        }

        internal override int ExpressionCount
        {
            get
            {
                return _expressions.Count;
            }
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
                ToReadOnly(value),
                value
            );

            // and return it
            return (ReadOnlyCollection<T>)collection;
        }
        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeExpressions()
        {
            return ReturnReadOnly(ref _expressions);
        }

        internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, XzaarExpression[] args)
        {
            Debug.Assert(variables == null || variables.Count == 0);

            return new BlockN(args);
        }
    }

    internal class BlockExpressionList : IList<XzaarExpression>
    {
        private readonly BlockExpression _block;
        private readonly Expression _arg0;

        internal BlockExpressionList(BlockExpression provider, Expression arg0)
        {
            _block = provider;
            _arg0 = arg0;
        }

        #region IList<Expression> Members

        public int IndexOf(Expression item)
        {
            if (_arg0 == item)
            {
                return 0;
            }

            for (int i = 1; i < _block.ExpressionCount; i++)
            {
                if (_block.GetExpression(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, Expression item)
        {
            throw new Exception();
        }

        public void RemoveAt(int index)
        {
            throw new Exception();
        }

        public Expression this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return _arg0;
                }

                return _block.GetExpression(index);
            }
            set
            {
                throw new Exception();
            }
        }

        #endregion

        #region ICollection<Expression> Members

        public void Add(Expression item)
        {
            throw new Exception();
        }

        public void Clear()
        {
            throw new Exception();
        }

        public bool Contains(Expression item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(Expression[] array, int arrayIndex)
        {
            array[arrayIndex++] = _arg0;
            for (int i = 1; i < _block.ExpressionCount; i++)
            {
                array[arrayIndex++] = _block.GetExpression(i);
            }
        }

        public int Count
        {
            get { return _block.ExpressionCount; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(Expression item)
        {
            throw new Exception();
        }

        #endregion

        #region IEnumerable<Expression> Members

        public IEnumerator<Expression> GetEnumerator()
        {
            yield return _arg0;

            for (int i = 1; i < _block.ExpressionCount; i++)
            {
                yield return _block.GetExpression(i);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return _arg0;

            for (int i = 1; i < _block.ExpressionCount; i++)
            {
                yield return _block.GetExpression(i);
            }
        }

        #endregion
    }

    public partial class XzaarExpression
    {
        public static BlockExpression Block(XzaarExpression arg0, XzaarExpression arg1)
        {
            RequiresCanRead(arg0, "arg0");
            RequiresCanRead(arg1, "arg1");

            return new Block2(arg0, arg1);
        }
        public static BlockExpression Block(XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
        {
            RequiresCanRead(arg0, "arg0");
            RequiresCanRead(arg1, "arg1");
            RequiresCanRead(arg2, "arg2");
            return new Block3(arg0, arg1, arg2);
        }
        public static BlockExpression Block(XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3)
        {
            RequiresCanRead(arg0, "arg0");
            RequiresCanRead(arg1, "arg1");
            RequiresCanRead(arg2, "arg2");
            RequiresCanRead(arg3, "arg3");
            return new Block4(arg0, arg1, arg2, arg3);
        }
        public static BlockExpression Block(XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3, XzaarExpression arg4)
        {
            RequiresCanRead(arg0, "arg0");
            RequiresCanRead(arg1, "arg1");
            RequiresCanRead(arg2, "arg2");
            RequiresCanRead(arg3, "arg3");
            RequiresCanRead(arg4, "arg4");

            return new Block5(arg0, arg1, arg2, arg3, arg4);
        }
        public static BlockExpression Block(params XzaarExpression[] expressions)
        {
            switch (expressions.Length)
            {
                case 2: return Block(expressions[0], expressions[1]);
                case 3: return Block(expressions[0], expressions[1], expressions[2]);
                case 4: return Block(expressions[0], expressions[1], expressions[2], expressions[3]);
                case 5: return Block(expressions[0], expressions[1], expressions[2], expressions[3], expressions[4]);
                default:
                    // ContractUtils.RequiresNotEmpty(expressions, "expressions");
                    // RequiresCanRead(expressions, "expressions");
                    return new BlockN(Copy(expressions));
            }
        }
        internal static ReadOnlyCollection<T> ToReadOnly<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return EmptyReadOnlyCollection<T>.Instance;
            }

#if SILVERLIGHT
            if (Expression.SilverlightQuirks) {
                // Allow any ReadOnlyCollection to be stored directly
                // (even though this is not safe)
                var r = enumerable as ReadOnlyCollection<T>;
                if (r != null) {
                    return r;
                }
            }
#endif

            var troc = enumerable as TrueReadOnlyCollection<T>;
            if (troc != null)
            {
                return troc;
            }

            //var builder = enumerable as ReadOnlyCollectionBuilder<T>;
            //if (builder != null)
            //{
            //    return builder.ToReadOnlyCollection();
            //}

            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                int count = collection.Count;
                if (count == 0)
                {
                    return EmptyReadOnlyCollection<T>.Instance;
                }

                T[] clone = new T[count];
                collection.CopyTo(clone, 0);
                return new TrueReadOnlyCollection<T>(clone);
            }

            // ToArray trims the excess space and speeds up access
            return new TrueReadOnlyCollection<T>(new List<T>(enumerable).ToArray());
        }

        internal static T[] Copy<T>(T[] array)
        {
            T[] copy = new T[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }
    }
}