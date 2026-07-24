using System;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for displaying titles above fields in the inspector.
      /// </summary>
      [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
      public class TitleAttribute : PropertyAttribute
      {
            public readonly GUIContent Content;
            public readonly Color Text = ShadePalette.ToColor32(Shade.EditorText), Underline = ShadePalette.ToColor32(Shade.White);
            /// <summary>
            /// Specifies weather the underline should stretch to default width or adjust to the title width.
            /// </summary>
            public bool Stretch = true;

            public TitleAttribute(string title) => Content = new(title);
            public TitleAttribute(string title, Shade shade) : this(title) => Text = Underline = ShadePalette.ToColor32(shade);
            public TitleAttribute(string title, Shade text, Shade underline) : this(title)
            {
                  Text = ShadePalette.ToColor32(text);
                  Underline = ShadePalette.ToColor32(underline);
            }
      }
}