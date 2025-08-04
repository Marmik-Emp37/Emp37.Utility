using UnityEditor;
using UnityEngine;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(InlineButtonAttribute), true)]
        internal class AttributeDrawer_InlineButton : PropertyDrawer
        {
                private const float gap = 2F;


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        var attr = attribute as InlineButtonAttribute;

                        position.width -= attr.Width + gap;
                        EditorGUI.PropertyField(position, property, label, true);

                        position.x += position.width + gap;
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