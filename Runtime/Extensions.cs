using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Utility
{
      public static class Extensions
      {
            /// <summary>
            /// Rescales a given value from a specified input range to a specified output range.
            /// </summary>
            /// <param name="value">The value to rescale.</param>
            /// <returns>The rescaled value clamped within the specified output range.</returns>
            public static float Remap(this float value, float iMin, float iMax, float oMin, float oMax) => Mathf.Lerp(a: oMin, b: oMax, t: Mathf.InverseLerp(a: iMin, b: iMax, value));

            /// <summary>
            /// Adds left indentation to the Rect.
            /// </summary>
            public static Rect Indent(this Rect rect, float value) => new(rect.x + value, rect.y, rect.width - value, rect.height);


            public static Color32 WithAlpha(this Color32 color, byte value) => new(color.r, color.g, color.b, value);
            public static void ApplyShade(this Image renderer, Shades shade, byte alpha = byte.MaxValue) => renderer.color = ColorLibrary.Pick(shade).WithAlpha(alpha);
            public static void ApplyShade(this SpriteRenderer renderer, Shades shade, byte alpha = byte.MaxValue) => renderer.color = ColorLibrary.Pick(shade).WithAlpha(alpha);


            public static void Reset(this Transform transform) => transform.SetLocalPositionAndRotation(transform.localScale = default, default);
      }
}