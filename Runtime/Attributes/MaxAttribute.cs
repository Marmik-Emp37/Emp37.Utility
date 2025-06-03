using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to make a serialized float or interger field type be restricted to a specific maximum value.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class MaxAttribute : UnityEngine.PropertyAttribute
      {
            public readonly float Value;

            public MaxAttribute(float value)
            {
                  Value = value;
                  order = -50;
            }
      }
}