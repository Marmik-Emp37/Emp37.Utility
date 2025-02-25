using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(RequireObjectAttribute))]
      internal class AttributeDrawer_RequireObject : BasePropertyDrawer
      {
            private const float MessageHeight = 21F;

            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType is not SerializedPropertyType.ObjectReference)
                  {
                        ShowInvalidUsageBox(position, SerializedPropertyType.ObjectReference);
                        return;
                  }

                  if (property.objectReferenceValue == null)
                  {
                        position.height = MessageHeight;
                        EditorGUI.HelpBox(position, (attribute as RequireObjectAttribute).Message, UnityEditor.MessageType.Error);
                        position.y += MessageHeight + EditorGUIUtility.standardVerticalSpacing; // - [ 1 ]
                  }

                  position.height = base.GetPropertyHeight(property, label); // - [ 2 ]
                  EditorGUI.PropertyField(position, property, label);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => base.GetPropertyHeight(property, label) /* - [ 2 ]*/ + (property.propertyType is SerializedPropertyType.ObjectReference && property.objectReferenceValue == null ? MessageHeight + EditorGUIUtility.standardVerticalSpacing /* - [ 1 ]*/ : 0F);
      }
}