using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Attribute used to display a serialized object reference field, such as a Sprite, Texture or Prefab, as a large square preview thumbnail.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class PreviewFieldAttribute : UnityEngine.PropertyAttribute
	{
		public readonly float Extent = 64F;

		public PreviewFieldAttribute(Size size = Size.Default) => Extent = size switch { Size.Large => 96F, Size.ExtraLarge => 128F, _ => Extent };
	}
}
