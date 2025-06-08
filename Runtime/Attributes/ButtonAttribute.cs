using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for drawing a button in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Method, Inherited = true)]
      public class ButtonAttribute : Attribute
      {
            public string Name = null;
            public readonly float Height = 18F;
            public readonly string[] Parameters = null;
            public readonly Color BackgroundColor = Color.white;

            public ButtonAttribute() { }
            public ButtonAttribute(Size size) => Height = size switch { Size.Medium => 27F, Size.Large => 36F, _ => Height };
            public ButtonAttribute(Size size, Shade shade) : this(size) => BackgroundColor = ShadeLibrary.Pick(shade);
            public ButtonAttribute(Size size, Shade shade, params string[] parameterNames) : this(size, shade) => Parameters = parameterNames;
      }
}