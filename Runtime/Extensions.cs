using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Utility
{
      public static class Extensions
      {
            #region R E C T
            /// <summary>
            /// Adds left indentation to this rect.
            /// </summary>
            public static Rect Indent(this Rect rect, float value) => new(rect.x + value, rect.y, rect.width - value, rect.height);
            #endregion

            #region C O L O R   L I B R A R Y
            public static Color32 WithAlpha(this Color32 color, byte value) => new(color.r, color.g, color.b, value);
            public static void ApplyShade(this Image renderer, Shade shade, byte alpha = byte.MaxValue) => renderer.color = ShadeLibrary.Pick(shade).WithAlpha(alpha);
            public static void ApplyShade(this SpriteRenderer renderer, Shade shade, byte alpha = byte.MaxValue) => renderer.color = ShadeLibrary.Pick(shade).WithAlpha(alpha);
            #endregion

            #region T R A N S F O R M
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
            #endregion

            #region S T R I N G
            /// <summary>
            /// Formats a string by adding spaces between characters and converting them to uppercase.
            /// </summary>
            /// <param name="input">The input string to format.</param>
            public static string ToStylizedTitleCase(this string input) => string.IsNullOrWhiteSpace(input) ? string.Empty : string.Concat(input.Select((c, i) => char.IsWhiteSpace(c) ? "" : char.IsUpper(c) ? (i > 0 ? "   " : "") + c : " " + char.ToUpper(c)));
            public static string ToTitleCase(this string input) => string.IsNullOrWhiteSpace(input) ? string.Empty : string.Concat(input.Select((c, i) => (i > 0 && char.IsUpper(c) ? " " : "") + c));
            #endregion
      }
}