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
