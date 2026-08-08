using System.Text;
using System.Globalization;
using UnityEngine;

namespace Emp37.Utility
{
        public static class Extensions
        {
                /// <summary>
                /// Adds left indentation to this rect.
                /// </summary>
                public static Rect Indent(this Rect rect, float value) => new(rect.x + value, rect.y, rect.width - value, rect.height);

                /// <summary>
                /// Resets the transform's local properties to their default values.
                /// </summary>
                /// <param name="transform">The <see cref="Transform"/> to reset.</param>
                /// <remarks>
                /// This method is useful for standardizing a transform's local properties to default values during initialization or setup.
                /// </remarks>
                public static void Reset(this Transform transform)
                {
                        transform.localPosition = transform.localEulerAngles = Vector3.zero;
                        transform.localScale = Vector3.one;
                }

                /// <summary>
                /// Inserts spaces before uppercase letters (splitting camelCase/PascalCase) and applies title casing.
                /// </summary>
                public static string ToTitleCase(this string text)
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
                public static string ToStylizedTitleCase(this string text)
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
                public static string Truncate(this string text, int length) => string.IsNullOrEmpty(text) || length < 0 || text.Length <= length ? text : $"{text[..length]}...";
        }
}