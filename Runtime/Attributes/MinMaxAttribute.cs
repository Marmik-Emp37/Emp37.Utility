using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Attribute used to display a serialized Vector2 or Vector2Int field as a range slider, where X is the lower and Y is the upper value, limited to the given bounds.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinMaxAttribute : UnityEngine.PropertyAttribute
	{
		public readonly float Min, Max;

		public MinMaxAttribute(float min, float max) { Min = min; Max = max; }
	}
}