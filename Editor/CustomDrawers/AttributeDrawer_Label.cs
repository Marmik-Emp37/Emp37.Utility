using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(LabelAttribute))]
        internal class AttributeDrawer_Label : PropertyDrawer
        {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        var attr = attribute as LabelAttribute;

                        EditorGUI.PropertyField(position, property, new(EditorGUIUtility.IconContent(attr.IconName)) { text = attr.Label }, true);
                }
        }
}