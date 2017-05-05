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
