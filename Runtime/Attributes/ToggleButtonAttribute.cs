using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to display a serialized boolean field as a toggle button.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field)]
      public class ToggleButtonAttribute : PropertyAttribute
      {
            public readonly float Height = 21F;

            public ToggleButtonAttribute()
            {
            }
            public ToggleButtonAttribute(Size size) => Height = size switch { Size.Small => 18F, Size.Medium => 27F, Size.Large => 36F, _ => Height };
      }
}