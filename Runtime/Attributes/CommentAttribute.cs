using System;
using UnityEngine;
 
namespace Emp37.Utility
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class CommentAttribute : PropertyAttribute
	{
		public readonly GUIContent Content;
		public readonly Color Tint = Color.black;
		public FontStyle Style;

		public CommentAttribute(string text) => Content = new(text);
		public CommentAttribute(string text, Shade tint) : this(text) => Tint = tint.ToColor();
	}
}