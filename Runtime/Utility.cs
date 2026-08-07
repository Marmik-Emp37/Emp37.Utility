using System.Text;
using System.Globalization;
using UnityEngine;

namespace Emp37.Utility
{
        public static class Utility
        {
                /// <summary>
                /// Inserts spaces before uppercase letters (splitting camelCase/PascalCase) and applies title casing.
                /// </summary>
                public static string ToTitleCase(string text)
                {
                        if (string.IsNullOrWhiteSpace(text)) return text;

                        int length = text.Length;

                        StringBuilder builder = new(length + 8);
                        for (int i = 0; i < length; i++)
                        {
                                char c = text[i];
                                if (i > 0 && char.IsUpper(c) && !char.IsWhiteSpace(text[i - 1])) builder.Append(' ');
                                builder.Append(c);
                        }
                        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(builder.ToString());
                }

                /// <summary>
                /// Formats a string by spacing out its characters and converting them to uppercase.
                /// </summary>
                public static string ToStylizedTitleCase(string text)
                {
                        if (string.IsNullOrWhiteSpace(text)) return text;

                        text = ToTitleCase(text);
                        int length = text.Length;

                        StringBuilder builder = new(length);
                        for (int i = 0; i < length; i++)
                        {
                                char c = text[i];
                                if (i > 0) builder.Append(char.IsUpper(c) ? "   " : " ");
                                builder.Append(char.ToUpper(c));
                        }
                        return builder.ToString();
                }

                /// <summary>
                /// Truncates <paramref name="text"/> to <paramref name="length"/> characters, appending an ellipsis if shortened.
                /// </summary>
                public static string Truncate(string text, int length) => string.IsNullOrEmpty(text) || length < 0 || text.Length <= length ? text : $"{text[..length]}...";

                /// <summary>
                /// Rescales a given value from a specified input range to a specified output range.
                /// </summary>
                /// <param name="value">The value to rescale.</param>
                /// <returns>The rescaled value clamped within the specified output range.</returns>
                public static float Remap(float value, float iMin, float iMax, float oMin, float oMax) => Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }
}