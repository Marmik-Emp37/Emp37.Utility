using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using Type = SerializedPropertyType;

      [CustomPropertyDrawer(typeof(MaxAttribute))]
      internal class AttributeDrawer_Max : BasePropertyDrawer
      {
            public override void Initialize(SerializedProperty property) => Validate(property);
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (property.propertyType is not (Type.Float or Type.Integer or Type.Vector2 or Type.Vector3 or Type.Vector2Int or Type.Vector3Int))
                  {
                        ShowInvalidUsageBox(position, Type.Float, Type.Integer);
                        return;
                  }

                  EditorGUI.BeginChangeCheck();
                  EditorGUI.PropertyField(position, property, label);
                  if (EditorGUI.EndChangeCheck())
                  {
                        Validate(property);
                  }
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
                                    property.intValue = @int(value);
                                    break;
                              }
                        case Type.Vector2Int:
                              {
                                    var value = property.vector2IntValue;
                                    property.vector2IntValue = new(x: @int(value.x), y: @int(value.y));
                                    break;
                              }
                        case Type.Vector3Int:
                              {
                                    var value = property.vector3IntValue;
                                    property.vector3IntValue = new(x: @int(value.x), y: @int(value.y), z: @int(value.z));
                                    break;
                              }
                        #endregion

                        #region F L O A T
                        case Type.Float:
                              {
                                    var value = property.floatValue;
                                    property.floatValue = @float(value);
                                    break;
                              }
                        case Type.Vector2:
                              {
                                    var value = property.vector2Value;
                                    property.vector2Value = new(x: @float(value.x), y: @float(value.y));
                                    break;
                              }
                        case Type.Vector3:
                              {
                                    var value = property.vector3Value;
                                    property.vector3Value = new(x: @float(value.x), y: @float(value.y), z: @float(value.z));
                                    break;
                              }
                              #endregion
                  }
                  int @int(int value) => Mathf.Clamp(value, int.MinValue, (int) attr.Value);
                  float @float(float value) => Mathf.Clamp(value, float.MinValue, attr.Value);
            }
      }
}