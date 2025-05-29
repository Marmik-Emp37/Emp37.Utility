using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to araws a Unity editor icon next to the property's display name using the specified built-in icon name.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field)]
      public class IconLabelAttribute : UnityEngine.PropertyAttribute
      {
            public readonly string IconName;

            public IconLabelAttribute(string iconName) => IconName = iconName;
      }
}