using System;
using UnityEngine;

namespace Emp37.Utility
{
        /// <summary>
        /// Attribute used to visually separate content in the inspector with a horizontal line.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
        public class SeparatorAttribute : PropertyAttribute
        {
                public readonly Color Color;
                public byte Thickness = 2;
                public bool Stretch;

                public SeparatorAttribute(Shade shade = Shade.Black, float alpha = 1F) => Color = shade.ToColor(alpha);
        }
}