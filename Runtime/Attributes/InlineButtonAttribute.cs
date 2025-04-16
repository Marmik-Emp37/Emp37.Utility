using System;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for drawing a button in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
      public class InlineButtonAttribute : UnityEngine.PropertyAttribute
      {
            public string Name = null;
            public readonly string Method;
            public readonly float Width = 60F;
            public readonly string[] Parameters = null;

            public InlineButtonAttribute(string method) => Method = method;
            public InlineButtonAttribute(string method, Size size) : this(method) => Width = size switch { Size.Small => 30F, Size.Medium => 60F, Size.Large => 90F, _ => Width, };
            public InlineButtonAttribute(string method, Size size, params string[] parameterNames) : this(method, size) => Parameters = parameterNames;
      }
}