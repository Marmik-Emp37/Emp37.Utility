using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(IndentAttribute))]
      internal class AttributeDrawer_Indent : BasePropertyDrawer
      {
            public override void Initialize(SerializedProperty property)
            {
            }
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  var attribute = base.attribute as IndentAttribute;

                  int indent = EditorGUI.indentLevel;
                  EditorGUI.indentLevel = attribute.Value;
                  EditorGUI.PropertyField(position, property, label, true);
                  EditorGUI.indentLevel = indent;
            }
      }
}