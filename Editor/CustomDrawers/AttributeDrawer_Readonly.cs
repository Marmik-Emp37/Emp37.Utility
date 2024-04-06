using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
      internal class AttributeDrawer_Readonly : BasePropertyDrawer
      {
            public override void OnPropertyDraw(Rect position, SerializedProperty property, GUIContent label)
            {
                  var attribute = base.attribute as ReadonlyAttribute;

                  GUI.enabled = attribute.ExclusiveToPlaymode && !EditorApplication.isPlaying;
                  EditorGUI.PropertyField(position, property, label, true);
                  GUI.enabled = true;
            }
      }
}