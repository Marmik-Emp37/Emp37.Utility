using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for drawing a button in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Method)]
      public class ButtonAttribute : PropertyAttribute
      {
            public string Name = null;
            public readonly float Height = 21F;
            public readonly string[] Parameters = null;
            public readonly Color BackgroundColor = Color.white;

            public ButtonAttribute()
            {
            }
            public ButtonAttribute(Size size) => Height = size switch { Size.Small => 18F, Size.Medium => 27F, Size.Large => 36F, _ => Height };
            public ButtonAttribute(Size size, Shade shade) : this(size) => BackgroundColor = ShadeLibrary.Pick(shade);
            /// <param name="parameters">The method parameter name(s) as Field or Property within this type.</param>
            public ButtonAttribute(Size size, Shade shade, params string[] parameters) : this(size, shade) => Parameters = parameters;
      }
}