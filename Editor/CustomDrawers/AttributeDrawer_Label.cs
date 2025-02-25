using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(LabelAttribute))]
      internal class AttributeDrawer_Label : BasePropertyDrawer
      {
            private GUIContent content = new();


            public override void Initialize(SerializedProperty property)
            {
                  var attr = attribute as LabelAttribute;
                  if (!string.IsNullOrWhiteSpace(attr.IconName)) content = EditorGUIUtility.IconContent(attr.IconName);
                  content.text = attr.Label;
            }
            public override void Draw(Rect position, SerializedProperty property, GUIContent _) => EditorGUI.PropertyField(position, property, content, true);
      }
}