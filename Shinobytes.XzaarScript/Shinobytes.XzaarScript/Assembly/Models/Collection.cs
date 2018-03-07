using System.Collections;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Assembly.Models
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
            get { return this.InternalItems[index]; }
            set { this.InternalItems[index] = value; }
        }
    }
}