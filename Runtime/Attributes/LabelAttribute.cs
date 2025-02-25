using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to modify the label of a serialized property.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class LabelAttribute : PropertyAttribute
      {
            public readonly string Label = string.Empty, IconName;

            public LabelAttribute() => order = -100;
            public LabelAttribute(string label) : this() => Label = label;
            public LabelAttribute(string label, string iconName) : this(label) => IconName = iconName;
      }
}