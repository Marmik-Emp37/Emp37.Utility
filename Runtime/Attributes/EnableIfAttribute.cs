using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Attribute to conditionally enable the associated field in the Inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class EnableIfAttribute : Attribute
	{
		public bool Invert;
		public readonly string Condition;

		/// <param name="condition">The name of the boolean member type as (field, property or method) on this target.</param>
		public EnableIfAttribute(string condition) => Condition = condition;
	}
}