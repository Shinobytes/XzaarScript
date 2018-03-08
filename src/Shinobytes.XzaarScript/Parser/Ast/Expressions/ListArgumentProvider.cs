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

using System;
using System.Collections.Generic;
using Expression = Shinobytes.XzaarScript.Parser.Ast.Expressions.XzaarExpression; 
namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    internal class ListArgumentProvider : IList<XzaarExpression>
    {
        private readonly IArgumentProvider _provider;
        private readonly Expression _arg0;

        internal ListArgumentProvider(IArgumentProvider provider, Expression arg0)
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

        public int Count => _provider.ArgumentCount;

        public bool IsReadOnly => true;

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