using System;


namespace Emp37.Utility
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class InlineButtonAttribute : UnityEngine.PropertyAttribute
	{
		public readonly string Method;
		public readonly float Width = 72F;
		public readonly string[] Parameters;
		public string Name;

		public InlineButtonAttribute(string method) => Method = method;
		public InlineButtonAttribute(string method, Size size) : this(method) => Width = size switch { Size.Large => 96F, Size.ExtraLarge => 128F, _ => Width };
		public InlineButtonAttribute(string method, Size size, params string[] parameters) : this(method, size) => Parameters = parameters;
	}
}