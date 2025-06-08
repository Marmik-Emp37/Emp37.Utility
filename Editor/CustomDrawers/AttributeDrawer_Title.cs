using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(TitleAttribute), true)]
      internal class AttributeDrawer_Title : BaseDecoratorDrawer
      {
            private TitleAttribute Attribute => attribute as TitleAttribute;

            private const float InitialSpacing = 8F, UnderlineHeight = 1F;

            private Vector2 contentSize;

            private readonly GUIStyle labelStyle = new(EditorStyles.boldLabel)
            {
                  fontSize = 14,
                  stretchHeight = false,
            };

            public override void Initialize()
            {
                  labelStyle.normal.textColor = Attribute.Text;

                  GUIContent content = Attribute.Content;
                  contentSize = labelStyle.CalcSize(content);
                  if (Attribute.Stretch)
                  {
                        contentSize.x = EditorGUIHelper.ReleventWidth;
                  }
            }
            public override void Draw(Rect position)
            {
                  position.y += InitialSpacing; // - [ 1 ]
                  position.height = contentSize.y;
                  EditorGUI.LabelField(position, Attribute.Content, labelStyle);

                  position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ 2 ]
                  position.size = new(contentSize.x, UnderlineHeight); // - [ 3 ]
                  EditorGUI.DrawRect(position, Attribute.Underline);
            }
            public override float GetHeight() => InitialSpacing /* - [ 1 ]*/ + contentSize.y + 2F * EditorGUIUtility.standardVerticalSpacing /* - [ 2 ]*/ + UnderlineHeight /* - [ 3 ]*/;
      }
}