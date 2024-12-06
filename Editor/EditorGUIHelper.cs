using static UnityEditor.EditorGUIUtility;

namespace Emp37.Utility.Editor
{
      public static class EditorGUIHelper
      {
            public static readonly float ExcessWidth = 2F * standardVerticalSpacing + singleLineHeight;
            public static float ReleventWidth => currentViewWidth - ExcessWidth;
      }
}