using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to indent a serialized property in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field)]
      public class IndentAttribute : UnityEngine.PropertyAttribute
      {
            public readonly int Value;

            public IndentAttribute(int level) => Value = level;
      }
}