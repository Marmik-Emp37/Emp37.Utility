using UnityEngine;

using static UnityEditor.EditorGUIUtility;

namespace Emp37.Utility.Editor
{
      public static class EditorGUIHelper
      {
            public static readonly float ExcessWidth = 2F * standardVerticalSpacing + singleLineHeight;
            public static float ReleventWidth => currentViewWidth - ExcessWidth;


            public class BackgroundColorScope : GUI.Scope
            {
                  private readonly Color original = GUI.backgroundColor;
                  public Color BackgroundColor { get => GUI.backgroundColor; set => GUI.backgroundColor = value; }

                  public BackgroundColorScope() { }
                  public BackgroundColorScope(Color color) => GUI.backgroundColor = color;

                  protected override void CloseScope() => GUI.backgroundColor = original;
            }
      }
}