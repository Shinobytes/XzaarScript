using System;
using System.Collections.Generic;
using Expression = Shinobytes.XzaarScript.Scripting.Expressions.XzaarExpression; 
namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    internal class ListArgumentProvider : IList<XzaarExpression>
    {
        private readonly IXzaarArgumentProvider _provider;
        private readonly Expression _arg0;

        internal ListArgumentProvider(IXzaarArgumentProvider provider, Expression arg0)
        {
            _provider = provider;
            _arg0 = arg0;
        }

        #region IList<Expression> Members

        public int IndexOf(Expression item)
        {
            if (_arg0 == item)
            {
                return 0;
            }

            for (int i = 1; i < _provider.ArgumentCount; i++)
            {
                if (_provider.GetArgument(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, Expression item)
        {
            throw new InvalidOperationException();
        }

        public void RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        public Expression this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return _arg0;
                }

                return _provider.GetArgument(index);
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region ICollection<Expression> Members

        public void Add(Expression item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(Expression item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(Expression[] array, int arrayIndex)
        {
            array[arrayIndex++] = _arg0;
            for (int i = 1; i < _provider.ArgumentCount; i++)
            {
                array[arrayIndex++] = _provider.GetArgument(i);
            }
        }

        public int Count
        {
            get { return _provider.ArgumentCount; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(Expression item)
        {
            throw new InvalidOperationException();
        }

        #endregion

        #region IEnumerable<Expression> Members

        public IEnumerator<Expression> GetEnumerator()
        {
            yield return _arg0;

            for (int i = 1; i < _provider.ArgumentCount; i++)
            {
                yield return _provider.GetArgument(i);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return _arg0;

            for (int i = 1; i < _provider.ArgumentCount; i++)
            {
                yield return _provider.GetArgument(i);
            }
        }

        #endregion
    }
}