using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;

      internal class Emp37Editor : UnityEditor.Editor
      {
            private Type targetType;

            private SerializedProperty defaultProperty;
            private SerializedProperty[] serializedProperties;
            private MethodInfo[] serializedMethods;

            private bool showDefaultProperty;


            private void OnEnable()
            {
                  targetType = target.GetType();

                  showDefaultProperty = !AttributeCache.Contains<HideDefaultPropertyAttribute>(targetType);

                  #region I N I T I A L I Z E   S E R I A L I Z E D   P R O P E R T I E S
                  if (serializedProperties == null)
                  {
                        List<SerializedProperty> properties = new();
                        SerializedProperty iterator = serializedObject.GetIterator();

                        while (iterator.NextVisible(true))
                        {
                              SerializedProperty property = serializedObject.FindProperty(iterator.name);
                              if (property != null) properties.Add(property);
                        }

                        defaultProperty = properties[0];
                        properties.RemoveAt(0);

                        serializedProperties = properties.ToArray();
                  }
                  #endregion

                  #region I N I T I A L I Z E   S E R I A L I Z E D   M E T H O D S
                  serializedMethods = targetType.GetMethods(DEFAULT_FLAGS).Where(static method => method.IsDefined(typeof(MethodAttribute), true)).ToArray();
                  #endregion
            }

            public override void OnInspectorGUI()
            {
                  #region D R A W   A T T R I B U T E S
                  if (AttributeCache.Contains<NoteAttribute>(targetType, false))
                  {
                        EditorGUILayout.HelpBox(AttributeCache.FetchFirst<NoteAttribute>(targetType).Content);
                  }
                  #endregion

                  #region D R A W   D E F A U L T   P R O P E R T Y
                  if (showDefaultProperty && defaultProperty != null)
                  {
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(defaultProperty);
                  }
                  #endregion

                  serializedObject.Update();
                  {
                        #region D R A W   S E R I A L I Z E D   P R O P E R T I E S
                        foreach (SerializedProperty property in serializedProperties)
                        {
                              FieldInfo field = property.GetField();
                              if (field == null || !EvaluateVisibility(field)) continue;

                              GUI.enabled = EvaluateEnabled(field);
                              EditorGUILayout.PropertyField(property, true);
                        }
                        #endregion

                        #region D R A W   S E R I A L I Z E D   M E TH O D S
                        foreach (MethodInfo method in serializedMethods)
                        {
                              ButtonAttribute button = method.GetCustomAttribute<ButtonAttribute>(true);
                              if (button == null || !EvaluateVisibility(method)) continue;

                              GUI.enabled = EvaluateEnabled(method);
                              GUI.backgroundColor = button.BackgroundColor;
                              if (GUILayout.Button(button.Name ?? Utility.ToTitleCase(method.Name), GUILayout.Height(button.Height)))
                              {
                                    AutoInvokeMethod(method, target, button.Parameters);
                              }
                        }
                        #endregion

                  }
                  serializedObject.ApplyModifiedProperties();

                  GUI.enabled = true;
            }

            private bool EvaluateEnabled(MemberInfo member)
            {
                  bool output = true;
                  var a0 = AttributeCache.FetchFirst<ReadonlyAttribute>(member, true);
                  if (a0 != null) output &= a0.ExclusiveToPlaymode && !EditorApplication.isPlaying;
                  var a1 = AttributeCache.FetchFirst<EnableWhenAttribute>(member, true);
                  if (a1 != null) output &= ReadAny(a1.ConditionName, target) is bool value && value;
                  var a2 = AttributeCache.FetchFirst<DisableWhenAttribute>(member, true);
                  if (a2 != null) output &= ReadAny(a2.ConditionName, target) is bool value && !value;
                  return output;
            }
            private bool EvaluateVisibility(MemberInfo member)
            {
                  bool output = true;
                  var a0 = AttributeCache.FetchFirst<ShowWhenAttribute>(member, true);
                  if (a0 != null) output &= ReadAny(a0.ConditionName, target) is bool value && value;
                  var a1 = AttributeCache.FetchFirst<HideWhenAttribute>(member, true);
                  if (a1 != null) output &= ReadAny(a1.ConditionName, target) is bool value && !value;
                  return output;
            }
      }
}