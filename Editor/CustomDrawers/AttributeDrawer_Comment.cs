using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(CommentAttribute), true)]
      internal class AttributeDrawer_Comment : BasePropertyDrawer
      {
            private CommentAttribute Attribute => attribute as CommentAttribute;

            private const byte BackgroundAlpha = 25;
            private const float MinHeight = 21F, StripWidth = 3F;

            private readonly GUIStyle labelStyle = new(EditorStyles.label) { richText = true, wordWrap = true };


            public override void Initialize(SerializedProperty property)
            {
                  labelStyle.fontStyle = Attribute.Style;
            }
            public override void OnPropertyDraw(Rect position, SerializedProperty property, GUIContent label)
            {
                  Rect commentRect = position;
                  commentRect.size = new(x: EditorGUIHelper.ReleventWidth, y: GetStyleHeight(Attribute.Content) /* - [ 1 ]*/);
                  EditorGUI.LabelField(commentRect, Attribute.Content, labelStyle);

                  EditorGUI.DrawRect(commentRect, Attribute.Tint.WithAlpha(BackgroundAlpha));

                  commentRect.width = StripWidth;
                  commentRect.x -= commentRect.width + 1;
                  EditorGUI.DrawRect(commentRect, Attribute.Tint);

                  position.y += commentRect.height + EditorGUIUtility.standardVerticalSpacing; // - [ 2 ]
                  position.height = EditorGUI.GetPropertyHeight(property);
                  EditorGUI.PropertyField(position, property, label, true);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  float height = base.GetPropertyHeight(property, label);
                  height += GetStyleHeight(Attribute.Content) + EditorGUIUtility.standardVerticalSpacing; // - [ 1 ] + [ 2 ]
                  return height;
            }

            private float GetStyleHeight(GUIContent content) => Mathf.Clamp(labelStyle.CalcHeight(content, EditorGUIHelper.ReleventWidth), MinHeight, float.MaxValue);
      }
}