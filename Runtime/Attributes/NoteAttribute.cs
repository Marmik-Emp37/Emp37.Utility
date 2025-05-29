using System;

using UnityEngine;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Class)]
      public class NoteAttribute : Attribute
      {
            public readonly GUIContent Content;

            public NoteAttribute(string text) => Content = new(text);
      }
}