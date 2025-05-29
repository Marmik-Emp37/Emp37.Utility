using UnityEditor;

using UnityEngine;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(IconLabelAttribute))]
      internal class AttributeDrawer_IconLabel : BasePropertyDrawer
      {
            private GUIContent content;


            public override void Initialize(SerializedProperty property)
            {
                  content = new(EditorGUIUtility.IconContent((attribute as IconLabelAttribute).IconName)) { text = ' ' + property.displayName };
            }
            public override void Draw(Rect position, SerializedProperty property, GUIContent _)
            {
                  EditorGUI.PropertyField(position, property, content, true);
            }
      }
}