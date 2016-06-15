using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.AsyncLinq
{
    public static class AsyncEnumerable
    {
        public static async Task<List<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> asyncSelector)
        {
            var results = new List<TResult>();
            foreach (var item in source)
            {
                var selectedItem = await asyncSelector(item);
                results.Add(selectedItem);
            }

            return results;
        }
    }
}