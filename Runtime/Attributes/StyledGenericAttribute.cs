using System;

using UnityEngine;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class StyledGenericAttribute : PropertyAttribute
      {
            public readonly float Height = 18F;
            public readonly Color BackgroundColor = Color.white;

            public StyledGenericAttribute() { }
            public StyledGenericAttribute(Size size) => Height = size switch { Size.Medium => 27F, Size.Large => 36F, _ => Height };
            public StyledGenericAttribute(Size size, Shade shade) : this(size) => BackgroundColor = ShadeLibrary.Pick(shade);
      }
}