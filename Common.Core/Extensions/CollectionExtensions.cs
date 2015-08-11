using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> iterator)
        {
            foreach (var item in collection)
            {
                iterator(item);
            }

        }
    }
}
