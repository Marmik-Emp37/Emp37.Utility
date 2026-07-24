using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Groups consecutive members onto a single horizontal row in the Inspector.
	/// </summary>
	/// <remarks>
	/// Apply with <c>true</c> to <b>open</b> a horizontal group. Every member drawn afterward joins the same row, including members with no attribute, until the group is closed.
	/// <br>Apply with <c>false</c> to <b>close</b> the current group, returning subsequent members to the default vertical layout.</br>
	/// <br>An open group is also closed automatically at the end of the fields section and the end of the methods section.</br>
	/// <para>Because the group spans until it is explicitly closed, a lone <c>[Horizontal]</c> with no closing <c>[Horizontal(false)]</c> will pull all following members in that section onto one row. This is by design.</para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class HorizontalAttribute : Attribute
	{
		public readonly bool Value;

		public HorizontalAttribute(bool value = true) => Value = value;
	}
}