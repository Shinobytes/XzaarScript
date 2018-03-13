/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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