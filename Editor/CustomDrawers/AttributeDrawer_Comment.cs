using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(CommentAttribute), true)]
      internal class AttributeDrawer_Comment : BaseDecoratorDrawer
      {
            private CommentAttribute Attribute => attribute as CommentAttribute;

            private const byte BackgroundAlpha = 25;
            private const float MinHeight = 21F, HighlightWidth = 3F;

            private readonly GUIStyle contentStyle = new(EditorStyles.label) { richText = true, wordWrap = true };
            private float ContentHeight => Mathf.Max(MinHeight, contentStyle.CalcHeight(Attribute.Content, EditorGUIHelper.ReleventWidth));

            public override void Initialize()
            {
                  contentStyle.fontStyle = Attribute.FontStyle;
            }
            public override void Draw(Rect position)
            {
                  position.height = ContentHeight; // - [ 1 ]
                  EditorGUI.LabelField(position, Attribute.Content, contentStyle);

                  Color32 color = Attribute.Tint;
                  color.a = BackgroundAlpha;
                  EditorGUI.DrawRect(position, color);

                  position.width = HighlightWidth;
                  position.x -= position.width + 1F;
                  EditorGUI.DrawRect(position, Attribute.Tint);
            }
            public override float GetHeight() => ContentHeight /* - [ 1 ]*/ + EditorGUIUtility.standardVerticalSpacing;
      }
}