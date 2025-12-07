using System.Collections.Generic;
using System.Linq;
using MoreLinq;

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
    }
}