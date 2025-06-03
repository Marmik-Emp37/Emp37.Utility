using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to indent a serialized property in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class IndentAttribute : UnityEngine.PropertyAttribute
      {
            public readonly int Level;

            public IndentAttribute(int level) => Level = level;
      }
}