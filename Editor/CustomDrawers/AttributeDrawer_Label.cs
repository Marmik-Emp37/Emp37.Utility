using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(LabelAttribute))]
      internal class AttributeDrawer_Label : BasePropertyDrawer
      {
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  var attribute = base.attribute as LabelAttribute;

                  if (attribute.IconName != null) label = EditorGUIUtility.IconContent(attribute.IconName);
                  label.text = attribute.Label;
                  EditorGUI.PropertyField(position, property, label, true);
            }
      }
}