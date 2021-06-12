using System;
using System.Collections.Generic;
using System.Linq;

namespace SGP.SharedTests.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source.ToList())
            {
                action(item);
                yield return item;
            }
        }
    }
}
