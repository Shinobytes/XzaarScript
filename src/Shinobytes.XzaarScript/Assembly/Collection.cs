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

using System.Collections;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Assembly
{
    public class Collection<T> : IList<T>//, IList<T>
    {
        protected readonly List<T> InternalItems;

        public Collection()
        {
            this.InternalItems = new List<T>();
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items) this.Add(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return InternalItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T item)
        {
            InternalItems.Add(item);
        }

        public virtual void Clear()
        {
            InternalItems.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this.InternalItems.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalItems.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return this.InternalItems.Remove(item);
        }

        public virtual int Count => this.InternalItems.Count;

        public virtual bool IsReadOnly => false;

        public virtual int IndexOf(T item)
        {
            return this.InternalItems.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            this.InternalItems.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            this.InternalItems.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get => this.InternalItems[index];
            set => this.InternalItems[index] = value;
        }
    }
}