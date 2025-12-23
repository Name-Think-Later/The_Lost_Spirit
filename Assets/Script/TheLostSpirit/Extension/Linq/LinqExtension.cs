using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;

namespace TheLostSpirit.Extension.Linq
{
    public static class LinqExtension
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable) {
            return enumerable.Shuffle().First();
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> enumerable, int count) {
            return enumerable.Shuffle().Take(count);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable) {
            foreach (var item in enumerable) {
                collection.Add(item);
            }
        }

        public static IEnumerable<U> SelectComponent<U>(this IEnumerable<Behaviour> source) {
            foreach (var item in source) {
                if (item.TryGetComponent<U>(out var component)) {
                    yield return component;
                }
            }
        }
    }
}