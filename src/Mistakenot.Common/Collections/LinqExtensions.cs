using System;
using System.Collections.Generic;
using System.Linq;

namespace Mistakenot.Common.Collections
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, S>(this IEnumerable<T> enumerable, Func<T, S> keySelector)
        {
            var cache = new HashSet<S>();

            return enumerable
                .Where(i => 
                    cache.Add(
                        keySelector(i)));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T value) 
            => enumerable.Except(new [] {value});

        public static IEnumerable<T> ExceptBy<T, S>(this IEnumerable<T> enumerable, IEnumerable<S> other, Func<T, S> selector)
            => enumerable.Where(t => !other.Contains(selector(t)));

        public static IEnumerable<T> ExceptBy<T, S>(this IEnumerable<T> enumerable, S other, Func<T, S> selector)
            => ExceptBy(enumerable, new [] {other}, selector);
    }
}