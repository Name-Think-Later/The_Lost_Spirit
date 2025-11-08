using System;

namespace Extension.General
{
    public static class ComparableExtension
    {
        public static bool InOpenInterval<T>(this T value, T lower, T upper) where T : IComparable<T> {
            return value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0;
        }

        public static bool InClosedInterval<T>(this T value, T lower, T upper) where T : IComparable<T> {
            return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
        }
    }
}