using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(InlineButtonAttribute), true)]
      internal class AttributeDrawer_InlineButton : BasePropertyDrawer
      {
            private const float Gap = 2F;

            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  var attr = attribute as InlineButtonAttribute;

                  position.width -= attr.Width + Gap;
                  EditorGUI.PropertyField(position, property, label, true);

                  position.x += position.width + Gap;
                  position.width = attr.Width;
                  if (GUI.Button(position, attr.Name ?? attr.Method))
                  {
                        object target = property.serializedObject.targetObject;
                        System.Reflection.MethodInfo method = ReflectionUtility.FindMethod(attr.Method, target.GetType());
                        if (method != null) ReflectionUtility.AutoInvokeMethod(method, target, attr.Parameters);
                  }
            }
      }
}