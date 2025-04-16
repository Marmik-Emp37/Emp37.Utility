using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(IndentAttribute))]
      internal class AttributeDrawer_Indent : BasePropertyDrawer
      {
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  int indent = EditorGUI.indentLevel;
                  EditorGUI.indentLevel = (attribute as IndentAttribute).Level;
                  EditorGUI.PropertyField(position, property, label, true);
                  EditorGUI.indentLevel = indent;
            }
      }
}