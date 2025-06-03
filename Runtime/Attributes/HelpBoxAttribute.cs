using System;

using UnityEngine;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
      public class HelpBoxAttribute : PropertyAttribute
      {
            public readonly GUIContent Content;
            public readonly MessageType MessageType;

            public HelpBoxAttribute(string content) => Content = new(content);
            public HelpBoxAttribute(string content, MessageType type) : this(content) => MessageType = type;
      }
}