using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(RequireObjectAttribute))]
        internal class AttributeDrawer_RequireObject : PropertyDrawer
        {
                private const float MessageHeight = 21F;

                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        if (property.propertyType is not SerializedPropertyType.ObjectReference)
                        {
                                EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, SerializedPropertyType.ObjectReference);
                                return;
                        }

                        label = EditorGUI.BeginProperty(position, label, property);

                        if (property.objectReferenceValue == null)
                        {
                                var attr = attribute as RequireObjectAttribute;

                                position.height = MessageHeight;
                                EditorGUI.HelpBox(position, attr.Message, UnityEditor.MessageType.Error);
                                position.y += MessageHeight + EditorGUIUtility.standardVerticalSpacing;
                        }
                        position.height = base.GetPropertyHeight(property, label);
                        EditorGUI.PropertyField(position, property, label, true);
                        position.y += position.height;

                        EditorGUI.EndProperty();
                }
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
                        base.GetPropertyHeight(property, label) + (property.propertyType is SerializedPropertyType.ObjectReference && property.objectReferenceValue == null ? MessageHeight + EditorGUIUtility.standardVerticalSpacing : 0F);
        }
}