using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Attribute for disabling a field in the inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DisableAttribute : Attribute
	{
		public readonly bool ExclusiveToPlaymode;

		public DisableAttribute() { }
		/// <param name="exclusiveToPlaymode">If set to <c>true</c>, the serialized property is read-only only during play mode.</param>
		public DisableAttribute(bool exclusiveToPlaymode) => ExclusiveToPlaymode = exclusiveToPlaymode;
	}
}
