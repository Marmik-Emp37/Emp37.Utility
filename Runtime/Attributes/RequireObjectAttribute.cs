using System;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field, Inherited = true)]
      public class RequireObjectAttribute : UnityEngine.PropertyAttribute
      {
            public readonly string Message = "This field requires an assigned object.";

            public RequireObjectAttribute()
            {
            }
            public RequireObjectAttribute(string message) => Message = message;
      }
}