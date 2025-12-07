using UnityEngine;

namespace TheLostSpirit.Extension.General
{
    public static class StringExtension
    {
        public static string Colored(this string str, string colorCode) {
            return $"<color={colorCode}>{str}</color>";
        }

        public static string Colored(this string str, Color color) {
            var colorCode = ColorUtility.ToHtmlStringRGB(color);

            return str.Colored($"#{colorCode}");
        }
    }
}