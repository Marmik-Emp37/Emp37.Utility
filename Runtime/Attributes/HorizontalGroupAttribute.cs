using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Specifies that a member belongs to a horizontal group in the custom inspector.
      /// Allows defining whether the group should begin or end.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
      public class HorizontalGroupAttribute : Attribute
      {
            public readonly bool State;

            /// <param name="state">Indicates if the group should begin (<c>true</c>) or end (<c>false</c>).</param>
            public HorizontalGroupAttribute(bool state) => State = state;
      }
}