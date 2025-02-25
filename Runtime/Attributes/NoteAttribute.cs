using System;

using UnityEngine;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
      public class NoteAttribute : Attribute
      {
            public readonly GUIContent Content;
            public readonly Color Color = Color.white;

            public NoteAttribute(string text) => Content = new(text);
            public NoteAttribute(string text, Shade shade) : this(text) => Color = ShadeLibrary.Pick(shade);
      }
}