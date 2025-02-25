using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to make a serialized float or interger field value to loop between specified values.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class RepeatAttribute : UnityEngine.PropertyAttribute
      {
            public readonly float Min, Max;

            public RepeatAttribute(float minInclusive, float maxExclusive)
            {
                  Min = minInclusive;
                  Max = maxExclusive;
                  order = -50;
            }
      }
}