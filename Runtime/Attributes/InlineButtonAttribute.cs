using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for drawing a button in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field)]
      public class InlineButtonAttribute : PropertyAttribute
      {
            public string Method, Name = null;
            public readonly float Width = 50F;
            public readonly string[] Parameters = null;

            public InlineButtonAttribute(string method) => Method = method;
            public InlineButtonAttribute(string method, string name) : this(method) => Name = name;
            public InlineButtonAttribute(string method, string name, Size size) : this(method, name) => Width = size switch { Size.Small => 20F, Size.Medium => 75F, Size.Large => 120F, _ => Width, };
            public InlineButtonAttribute(string method, string name, Size size, params string[] parameters) : this(method, name, size) => Parameters = parameters;
      }
}