using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Attribute used to display a short text, such as a unit, after the value of a serialized numeric, vector or string field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class UnitAttribute : UnityEngine.PropertyAttribute
	{
		public readonly string Text;

		public UnitAttribute(string text) => Text = text;
	}
}