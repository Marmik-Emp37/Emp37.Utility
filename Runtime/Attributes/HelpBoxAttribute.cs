using System;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
      public class HelpBoxAttribute : UnityEngine.PropertyAttribute
      {
            public readonly string Message;
            public readonly MessageType MessageType;

            public HelpBoxAttribute(string message) => Message = message;
            public HelpBoxAttribute(string message, MessageType type) : this(message) => MessageType = type;
      }
}