using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(IndentAttribute))]
        internal class AttributeDrawer_Indent : PropertyDrawer
        {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        label = EditorGUI.BeginProperty(position, label, property);

                        int indent = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = (attribute as IndentAttribute).Level;
                        EditorGUI.PropertyField(position, property, label, true);
                        EditorGUI.indentLevel = indent;

                        EditorGUI.EndProperty();
                }
        }
}