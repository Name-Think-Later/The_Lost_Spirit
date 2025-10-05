using System;
using System.Collections.Generic;
using MoreLinq;
using ZLinq;

namespace Extension.ZLinq {
    public static class ZLinqExtensions
    {
        public static IEnumerable<T> AsEnumerable<TEnumerator, T>(this ValueEnumerable<TEnumerator, T> valueEnumerable)
            where TEnumerator : struct, IValueEnumerator<T>
        {
            using (var e = valueEnumerable.Enumerator)
            {
                while (e.TryGetNext(out var current))
                {
                    yield return current;
                }
            }
        }
    
        public static void ForEach<TEnumerator, T>(this ValueEnumerable<TEnumerator, T> valueEnumerable, Action<T> action)
            where TEnumerator : struct, IValueEnumerator<T>
        {
            valueEnumerable.AsEnumerable().ForEach(action);
        }
    
        public static void ForEach<TEnumerator, T>(this ValueEnumerable<TEnumerator, T> valueEnumerable, Action<T, int> action)
            where TEnumerator : struct, IValueEnumerator<T>
        {
            valueEnumerable.AsEnumerable().ForEach(action);
        }
    }
}
