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
      }
}