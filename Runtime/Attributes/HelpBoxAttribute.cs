using System;
using UnityEngine;

namespace Emp37.Utility
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class HelpBoxAttribute : PropertyAttribute
	{
		public readonly string Text;
		public readonly MessageType Type;

		public HelpBoxAttribute(string text) => Text = text;
		public HelpBoxAttribute(string text, MessageType type) : this(text) => Type = type;
	}
}