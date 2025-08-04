using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
        using Type = SerializedPropertyType;

        [CustomPropertyDrawer(typeof(MaxAttribute))]
        internal class AttributeDrawer_Max : PropertyDrawer
        {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        if (property.propertyType is not (Type.Float or Type.Integer or Type.Vector2 or Type.Vector3 or Type.Vector2Int or Type.Vector3Int))
                        {
                                EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, Type.Float, Type.Integer);
                                return;
                        }

                        label = EditorGUI.BeginProperty(position, label, property);

                        using (EditorGUI.ChangeCheckScope scope = new())
                        {
                                EditorGUI.PropertyField(position, property, label);
                                if (scope.changed) Validate(property);
                        }

                        EditorGUI.EndProperty();
                }

                private void Validate(SerializedProperty property)
                {
                        var attr = attribute as MaxAttribute;
                        switch (property.propertyType)
                        {
                                #region I N T E G E R
                                case Type.Integer:
                                        {
                                                var value = property.intValue;
                                                property.intValue = (int) Mathf.Min(value, attr.Value);
                                                break;
                                        }
                                case Type.Vector2Int:
                                        {
                                                var value = property.vector2IntValue;
                                                property.vector2IntValue = new(x: (int) Mathf.Min(value.x, attr.Value), y: (int) Mathf.Min(value.y, attr.Value));
                                                break;
                                        }
                                case Type.Vector3Int:
                                        {
                                                var value = property.vector3IntValue;
                                                property.vector3IntValue = new(x: (int) Mathf.Min(value.x, attr.Value), y: (int) Mathf.Min(value.y, attr.Value), z: (int) Mathf.Min(value.z, attr.Value));
                                                break;
                                        }
                                #endregion

                                #region F L O A T
                                case Type.Float:
                                        {
                                                var value = property.floatValue;
                                                property.floatValue = Mathf.Min(value, attr.Value);
                                                break;
                                        }
                                case Type.Vector2:
                                        {
                                                var value = property.vector2Value;
                                                property.vector2Value = new(x: Mathf.Min(value.x, attr.Value), y: Mathf.Min(value.y, attr.Value));
                                                break;
                                        }
                                case Type.Vector3:
                                        {
                                                var value = property.vector3Value;
                                                property.vector3Value = new(x: Mathf.Min(value.x, attr.Value), y: Mathf.Min(value.y, attr.Value), z: Mathf.Min(value.z, attr.Value));
                                                break;
                                        }
                                #endregion
                        }
                }
        }
}