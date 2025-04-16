using System.Globalization;
using System.Linq;

using UnityEngine;

namespace Emp37.Utility
{
      public static class Utility
      {
            public static string ToTitleCase(string text)
            {
                  if (string.IsNullOrWhiteSpace(text)) return text;
                  string spacedText = string.Concat(text.Select((c, idx) => (idx > 0 && char.IsUpper(c) && !char.IsWhiteSpace(text[idx - 1]) ? ' ' : string.Empty) + c.ToString()));
                  return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spacedText);
            }
            /// <summary>
            /// Formats a string by adding spaces between characters and converting them to uppercase.
            /// </summary>
            /// <param name="text">The input string to format.</param>
            public static string ToStylizedTitleCase(string text) => string.IsNullOrWhiteSpace(text) ? text : string.Concat(ToTitleCase(text).Select((c, idx) => (idx > 0 ? (char.IsUpper(c) ? "   " : " ") : string.Empty) + char.ToUpper(c)));
            public static string Truncate(string text, int length) => text.Length > length ? $"{text[..length]}..." : text;
            /// <summary>
            /// Rescales a given value from a specified input range to a specified output range.
            /// </summary>
            /// <param name="value">The value to rescale.</param>
            /// <returns>The rescaled value clamped within the specified output range.</returns>
            public static float Remap(float value, float iMin, float iMax, float oMin, float oMax) => Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
      }
}