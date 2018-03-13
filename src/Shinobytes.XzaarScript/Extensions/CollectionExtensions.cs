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
 
using System;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Extensions
{
    public static class CollectionExtensions
    {
        //public static List<T2> Select<T, T2>(this IEnumerable<T> items, Func<T, T2> m)
        //{
        //    var output = new List<T2>();
        //    foreach (var item in items)
        //        output.Add(m(item));
        //    return output;            
        //}

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> forEachAction)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (forEachAction == null) return;
            foreach (var item in enumerable)
            {
                forEachAction(item);
            }
        }
    }
}
