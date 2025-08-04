using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(ToggleButtonAttribute))]
        internal class AttributeDrawer_ToggleButton : PropertyDrawer
        {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        if (property.propertyType is not SerializedPropertyType.Boolean)
                        {
                                EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, SerializedPropertyType.Boolean);
                                return;
                        }

                        label = EditorGUI.BeginProperty(position, label, property);
                        var attr = attribute as ToggleButtonAttribute;

                        position.height = attr.Height;
                        label.text += $" : {property.boolValue}";
                        property.boolValue = GUI.Toggle(position, property.boolValue, label, GUI.skin.button);

                        EditorGUI.EndProperty();
                }
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                        bool isValid = property.propertyType is SerializedPropertyType.Boolean;

                        return isValid ? (attribute as ToggleButtonAttribute).Height : base.GetPropertyHeight(property, label);
                }
        }
}