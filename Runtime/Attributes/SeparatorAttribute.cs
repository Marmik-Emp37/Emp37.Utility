using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to visually separate content in the inspector with a horizontal line.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
      public class SeparatorAttribute : PropertyAttribute
      {
            public readonly Color Color;
            public readonly byte Thickness;
            public readonly bool Stretch;

            public SeparatorAttribute(Shade shade = Shade.Black, byte thickness = 3, bool stretch = false, byte alpha = byte.MaxValue)
            {
                  Color = ShadeLibrary.Pick(shade).WithAlpha(alpha);
                  Thickness = thickness;
                  Stretch = stretch;
            }
      }
}