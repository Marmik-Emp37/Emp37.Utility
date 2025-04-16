using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(LabelAttribute), true)]
      internal class AttributeDrawer_Label : BasePropertyDrawer
      {
            public override void Draw(Rect position, SerializedProperty property, GUIContent _) => EditorGUI.PropertyField(position, property, (attribute as LabelAttribute).Label, true);
      }
}