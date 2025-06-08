using UnityEditor;

using UnityEngine;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(StyledGenericAttribute), true)]
      internal class AttributeDrawer_StyledGeneric : BasePropertyDrawer
      {
            private readonly Texture expandedContent = EditorGUIUtility.IconContent("d_FolderOpened Icon").image, collapsedContent = EditorGUIUtility.IconContent("d_Folder Icon").image;

            private readonly GUIStyle buttonStyle = new(GUI.skin.button) { alignment = TextAnchor.MiddleLeft, };

            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType != SerializedPropertyType.Generic)
                  {
                        ShowInvalidUsageBox(position, SerializedPropertyType.Generic);
                        return;
                  }

                  var attr = attribute as StyledGenericAttribute;

                  label.image = property.isExpanded ? expandedContent : collapsedContent;

                  position.height = attr.Height;
                  property.isExpanded = GUI.Toggle(position, property.isExpanded, label, buttonStyle);

                  if (!property.isExpanded) return;
                  position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ 1 ]

                  Rect boxRect = new(position) { height = GetPropertyHeight(property, label) - position.height - EditorGUIUtility.standardVerticalSpacing };
                  using (new EditorGUIHelper.BackgroundColorScope(attr.BackgroundColor))
                  {
                        GUI.Box(boxRect, GUIContent.none);
                  }
                  position.y += EditorGUIUtility.standardVerticalSpacing + 1F; // - [ 2 ]

                  Rect contentRect = new(position) { width = position.width - 3F };
                  SerializedProperty iterator = property.Copy(), end = iterator.GetEndProperty();
                  using (new EditorGUI.IndentLevelScope(increment: 1))
                  {
                        bool enterChildren = true;
                        while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
                        {
                              contentRect.height = EditorGUI.GetPropertyHeight(iterator, true);
                              EditorGUI.PropertyField(contentRect, iterator, true);
                              contentRect.y += contentRect.height + EditorGUIUtility.standardVerticalSpacing; // - [ 3 ]
                              enterChildren = false;
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType != SerializedPropertyType.Generic) return base.GetPropertyHeight(property, label);

                  var attr = attribute as StyledGenericAttribute;
                  if (!property.isExpanded) return attr.Height; // - [ 1 ]

                  float height = attr.Height;
                  height += 2F * (EditorGUIUtility.standardVerticalSpacing + 1F); // - [ 2 ]

                  SerializedProperty iterator = property.Copy(), end = iterator.GetEndProperty();
                  bool enterChildren = true;
                  while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
                  {
                        height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing; // - [ 3 ]
                        enterChildren = false;
                  }
                  return height;
            }
      }
}