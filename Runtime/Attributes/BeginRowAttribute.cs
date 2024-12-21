using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Marks the beginning of a horizontal layout group.
      /// </summary>
      /// <remarks>Used for visually grouping fields or methods in the Unity Inspector.</remarks>
      [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
      public class BeginRowAttribute : Attribute
      {
      }
}