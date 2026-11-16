using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Invokes a parameterless method whenever the field is modified through the Inspector.
	/// </summary>
	/// <remarks>
	/// The method runs after the new value has been applied to the object, so it already sees the updated field. It is called on every selected object when multi-editing.
	/// <br>It only fires for edits made in the Inspector, not for changes made from code or at runtime.</br>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class OnValueChangedAttribute : Attribute
	{
		public readonly string Method;

		public OnValueChangedAttribute(string method) => Method = method;
	}
}
