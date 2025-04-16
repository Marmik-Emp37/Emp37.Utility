using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to expand the representation of a serializable object value in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field)]
      public class ExpandableObjectAttribute : UnityEngine.PropertyAttribute
      {
      }
}