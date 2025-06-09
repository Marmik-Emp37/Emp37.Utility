using System;

using UnityEngine;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class StyledGroupAttribute : PropertyAttribute
      {
            public readonly float Height = 18F;
            public readonly Color BackgroundColor = Color.white;

            public StyledGroupAttribute() { }
            public StyledGroupAttribute(Size size) => Height = size switch { Size.Medium => 27F, Size.Large => 36F, _ => Height };
            public StyledGroupAttribute(Size size, Shade shade) : this(size) => BackgroundColor = ShadeLibrary.Pick(shade);
      }
}