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
using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class CollectionStream<T>
    {
        private readonly IList<T> items;
        private int currentIndex;

        /// <summary>
        /// Creates a new instance of <see cref="CollectionStream{T}"/>
        /// </summary>
        /// <param name="items"></param>
        public CollectionStream(IList<T> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Peeks at the next available item without advancing the current position
        /// </summary>
        /// <returns></returns>
        public T PeekNext()
        {
            return PeekAt(1);
        }

        /// <summary>
        /// Peeks at the available item according to the offset; from the current position without advancing
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public T PeekAt(int offset)
        {
            if (currentIndex + offset >= items.Count) return default(T);
            return items[currentIndex + offset];
        }


        public bool CurrentIs(Func<T, bool> predicate)
        {
            return Current != null && predicate(Current);
        }

        public bool NextIs(int offset, Func<T, bool> predicate)
        {
            var peekAt = PeekAt(offset);
            return peekAt != null && predicate(peekAt);
        }

        public bool NextIs(Func<T, bool> predicate)
        {
            var peekNext = PeekNext();
            return peekNext != null && predicate(peekNext);
        }

        /// <summary>
        /// Advances the amount of steps in the current position and returns the item
        /// </summary>
        /// <param name="steps"></param>
        public T Advance(int steps)
        {
            currentIndex += steps;
            if (Available <= 0) return default(T);
            return Current;
        }

        /// <summary>
        /// Peeks at the previous item without decrementing the current position
        /// </summary>
        /// <returns></returns>
        public T PeekPrevious()
        {
            if (currentIndex - 1 < 0 || items.Count == 0) return default(T);
            return items[currentIndex - 1];
        }

        /// <summary>
        /// Returns the current index/position of the stream
        /// </summary>
        public int Index => currentIndex;

        /// <summary>
        /// Returns the current item without changing the position of the stream
        /// </summary>
        public T Current => currentIndex < items.Count ? items[currentIndex] : default(T);

        /// <summary>
        /// Returns the next item if it matches the target and advances the current position by 1
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public T MatchNext(T target)
        {
            if (currentIndex + 1 >= items.Count)
                return default(T);
            if (Equals(items[currentIndex + 1], target))
                return items[++currentIndex];
            return default(T);
        }

        /// <summary>
        /// Take the next available item in the stream and advance the current position by 1
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            if (currentIndex + 1 >= items.Count)
            {
                currentIndex++;
                return default(T);
            }
            return items[++currentIndex];
        }

        public T ConsumeExpected(Func<T, bool> predicate)
        {
            if (currentIndex >= items.Count || !predicate(items[currentIndex]))
            {
                throw new ParserException($"Unexpected token found '{items[currentIndex]}'");
            }

            return items[currentIndex++];
        }

        public T Consume(Func<T, bool> predicate)
        {

            if (currentIndex >= items.Count)
            {
                currentIndex++;
                return default(T);
            }

            if (predicate(items[currentIndex]))
            {
                return items[currentIndex++];
            }

            return default(T);
        }

        public T Consume()
        {
            if (currentIndex >= items.Count)
            {
                currentIndex++;
                return default(T);
            }
            return items[currentIndex++];
        }

        /// <summary>
        /// Takes the previous available item from the stream and decrement the current position by 1
        /// </summary>
        /// <returns></returns>
        public T Previous()
        {
            if (currentIndex - 1 < 0 || items.Count == 0)
                return default(T);
            return items[--currentIndex];
        }

        /// <summary>
        /// Returns whether we have more items in the stream or not
        /// </summary>
        /// <returns></returns>
        public bool EndOfStream()
        {
            return currentIndex >= items.Count || Interrupted;
        }

        public bool Interrupted { get; set; }

        /// <summary>
        /// Returns the size of the stream
        /// </summary>
        public int Length => items.Count;

        /// <summary>
        /// Returns the amount of available items that are left in the stream
        /// </summary>
        public int Available => items.Count - this.currentIndex;

        public void Seek(int i)
        {
            currentIndex = i;
        }

        public int IndexOf(Func<T, bool> func)
        {
            var item = this.items.Skip(this.currentIndex).FirstOrDefault(func);
            if (item != null) return Array.IndexOf(this.items.ToArray(), item);
            return -1;
        }
    }
}