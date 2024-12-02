using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(ToggleButtonAttribute), true)]
      internal class AttributeDrawer_ToggleButton : BasePropertyDrawer
      {
            public override void OnPropertyDraw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType is not SerializedPropertyType.Boolean)
                  {
                        EditorGUI.HelpBox(position, $"Use {nameof(ToggleButtonAttribute)} on 'Boolean' field type.", UnityEditor.MessageType.Error);
                        return;
                  }
                  label.text += " : " + (property.boolValue ? "On" : "Off");
                  property.boolValue = GUI.Toggle(position, property.boolValue, label, GUI.skin.button);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => (attribute as ToggleButtonAttribute).Height;
      }
}