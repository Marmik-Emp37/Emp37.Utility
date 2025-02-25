using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for drawing a button in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
      public class InlineButtonAttribute : UnityEngine.PropertyAttribute
      {
            public string Name;
            public readonly string Method;
            public readonly float Width = 60F;
            public readonly string[] Parameters;

            public InlineButtonAttribute(string methodName) => Method = methodName;
            public InlineButtonAttribute(string methodName, Size size) : this(methodName) => Width = size switch { Size.Small => 30F, Size.Large => 90F, _ => Width };
            public InlineButtonAttribute(string methodName, Size size, params string[] parameterNames) : this(methodName, size) => Parameters = parameterNames;
      }
}