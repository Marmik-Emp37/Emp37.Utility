using UnityEditor;

using UnityEngine;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(ExpandableGroupAttribute))]
      internal class AttributeDrawer_ExpandableGroup : PropertyDrawer
      {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType != SerializedPropertyType.Generic)
                  {
                        EditorGUI.HelpBox(position, $"Use {typeof(ExpandableGroupAttribute).Name} on fields of type {SerializedPropertyType.Generic}.", UnityEditor.MessageType.Error);
                        return;
                  }

                  var attr = attribute as ExpandableGroupAttribute;

                  position.height = attr.Height;
                  property.isExpanded = GUI.Toggle(position, property.isExpanded, Utility.ToStylizedTitleCase(label.text), GUI.skin.button);

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
                              EditorGUI.PropertyField(contentRect, iterator, true);
                              contentRect.y += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing; // - [ 3 ]
                              enterChildren = false;
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  var attr = attribute as ExpandableGroupAttribute;
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