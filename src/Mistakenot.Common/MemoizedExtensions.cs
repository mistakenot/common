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
                    // This might not be threadsafe...
                    var result = func(param);
                    cache.Add(param, result);
                    return result;
                }
            };
        }

        public static Func<T1, T2, S> Memoized<T1, T2, S>(
            this Func<T1, T2, S> func,
            bool threadSafe = false) 
        {
            var memoized = Memoized<Tuple<T1, T2>, S>(t => func(t.Item1, t.Item2), threadSafe);
            return (t1, t2) => memoized(new Tuple<T1, T2>(t1, t2));
        }

        public static Func<T1, T2, S> Memoized<T1, T2, S>(
            this Func<T1, T2, S> func,
            IDictionary<Tuple<T1, T2>, S> cache) 
        {
            var memoized = Memoized<Tuple<T1, T2>, S>(t => func(t.Item1, t.Item2), cache);
            return (t1, t2) => memoized(new Tuple<T1, T2>(t1, t2));
        }
    }
}