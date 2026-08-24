using UnityEngine;

namespace Emp37.Utility
{
	public static class Utility
	{
		/// <summary>
		/// Rescales a given value from a specified input range to a specified output range.
		/// </summary>
		/// <param name="value">The value to rescale.</param>
		/// <returns>The rescaled value clamped within the specified output range.</returns>
		public static float Remap(float value, float iMin, float iMax, float oMin, float oMax) => Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
		public static string ToHex(Color color) => "#" + (color.a < 1F ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color));
	}
}