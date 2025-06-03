using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(InlineButtonAttribute))]
      internal class AttributeDrawer_InlineButton : BasePropertyDrawer
      {
            private const float Gap = 2F;

            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  InlineButtonAttribute attribute = base.attribute as InlineButtonAttribute;

                  position.width -= attribute.Width + Gap;
                  EditorGUI.PropertyField(position, property, label);

                  position.x += position.width + Gap;
                  position.width = attribute.Width;
                  if (GUI.Button(position, attribute.Name ?? attribute.Method))
                  {
                        object target = property.serializedObject.targetObject;
                        System.Reflection.MethodInfo method = ReflectionUtility.FindMethod(attribute.Method, target.GetType());
                        if (method != null) ReflectionUtility.AutoInvokeMethod(method, target, attribute.Parameters);
                  }
            }
      }
}