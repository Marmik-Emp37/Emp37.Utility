using System;
using UnityEngine;

namespace Emp37.Utility
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : Attribute
	{
		public readonly string[] Parameters = null;
		public readonly float Height = 18F;
		public string Name = null;
		public Color BackgroundColor = Color.white;

		public ButtonAttribute() { }
		public ButtonAttribute(params string[] parameters) => Parameters = parameters;
		public ButtonAttribute(Size size, params string[] parameters) : this(parameters) => Height = size switch { Size.Large => 24F, Size.ExtraLarge => 36F, _ => Height };
	}
}