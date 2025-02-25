using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(ToggleButtonAttribute))]
      internal class AttributeDrawer_ToggleButton : BasePropertyDrawer
      {
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType is not SerializedPropertyType.Boolean)
                  {
                        ShowInvalidUsageBox(position, SerializedPropertyType.Boolean);
                        return;
                  }

                  label.text += " : " + (property.boolValue ? "On" : "Off");
                  property.boolValue = GUI.Toggle(position, property.boolValue, label, GUI.skin.button);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.propertyType is SerializedPropertyType.Boolean ? (attribute as ToggleButtonAttribute).Height : base.GetPropertyHeight(property, label);
      }
}