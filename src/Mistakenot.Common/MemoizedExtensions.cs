using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mistakenot.Common
{
    public static class MemoizedExtensions
    {
        public static Func<T, S> Memoized<T, S>(
            this Func<T, S> func,
            bool threadSafe = false) 
            => threadSafe ?
                Memoized(func, new ConcurrentDictionary<T, S>()) :
                Memoized(func, new Dictionary<T, S>());

        public static Func<T, S> Memoized<T, S>(this Func<T, S> func, IDictionary<T, S> cache)
        {
            return (param) =>
            {
                if (cache.ContainsKey(param))
                {
                    return cache[param];
                }
                else
                {
                    var result = func(param);
                    cache.Add(param, result);
                    return result;
                }
            };
        }
    }
}