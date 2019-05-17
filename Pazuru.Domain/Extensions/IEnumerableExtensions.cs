using System;
using System.Collections.Generic;

namespace Pazuru.Domain.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Func<T, bool> predicate, Action<T> action)
        {
            foreach (T item in collection)
                if (predicate(item))
                    action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
                    action(item);
        }
    }
}
