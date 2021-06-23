using System;
using System.Collections.Generic;
using System.Linq;

namespace SGP.SharedTests.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source.ToList())
            {
                action(item);
            }
        }
    }
}
