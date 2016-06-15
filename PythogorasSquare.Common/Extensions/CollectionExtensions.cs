using System;
using System.Collections.Generic;

namespace PythogorasSquare.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in items)
            {
                action(item, index++);
            }
        }
    }
}