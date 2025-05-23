using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(RequireObjectAttribute))]
      internal class AttributeDrawer_RequireObject : BasePropertyDrawer
      {
            private const float ErrorBoxHeight = 21F;

            public override void Initialize(SerializedProperty property)
            {
            }
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType != SerializedPropertyType.ObjectReference)
                  {
                        EditorGUI.HelpBox(position, $"Use RequireObject attribute on a field of type '{SerializedPropertyType.ObjectReference}'.", UnityEditor.MessageType.Error);
                        return;
                  }
                  if (property.objectReferenceValue == null)
                  {
                        var attribute = base.attribute as RequireObjectAttribute;

                        position.height = ErrorBoxHeight;
                        EditorGUI.HelpBox(position, attribute.Message, UnityEditor.MessageType.Error);
                        position.y += ErrorBoxHeight + EditorGUIUtility.standardVerticalSpacing; // - [ 1 ]
                  }
                  position.height = EditorGUIUtility.singleLineHeight;
                  EditorGUI.PropertyField(position, property, label);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  float height = base.GetPropertyHeight(property, label);
                  if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
                  {
                        height += ErrorBoxHeight + EditorGUIUtility.standardVerticalSpacing; // - [ 1 ]
                  }
                  return height;
            }
      }
}