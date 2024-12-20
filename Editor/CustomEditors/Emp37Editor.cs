using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;

      internal class Emp37Editor : UnityEditor.Editor
      {
            private Type targetType;

            private bool showDefaultScript;
            private SerializedProperty defaultScript;

            private SerializedProperty[] serializedProperties;
            private MethodInfo[] serializedMethods;


            private void OnEnable()
            {
                  targetType = target.GetType();

                  showDefaultScript = !targetType.IsDefined(typeof(HideDefaultScriptAttribute));

                  #region I N I T I A L I Z E   P R O P E R T I E S
                  if (serializedProperties == null)
                  {
                        Queue<SerializedProperty> properties = new();
                        SerializedProperty iterator = serializedObject.GetIterator();
                        while (iterator.NextVisible(true))
                        {
                              SerializedProperty property = serializedObject.FindProperty(iterator.name);
                              if (property != null)
                              {
                                    properties.Enqueue(property);
                              }
                        }
                        defaultScript = properties.Dequeue();
                        serializedProperties = properties.ToArray();
                  }
                  #endregion

                  #region I N I T I A L I Z E   M E T H O D S
                  serializedMethods = targetType.GetMethods(ReflectionFlags);
                  #endregion
            }

            public override void OnInspectorGUI()
            {
                  serializedObject.Update();
                  {
                        #region D E F A U L T   S C R I P T
                        if (defaultScript != null && showDefaultScript)
                        {
                              GUI.enabled = false;
                              EditorGUILayout.PropertyField(defaultScript);
                        }
                        #endregion

                        #region S E R I A L I Z E D   P R O P E R T I E S
                        foreach (SerializedProperty property in serializedProperties)
                        {
                              if (!TryFetchInfo(property.name, targetType, out FieldInfo field)) continue;

                              if (EvaluateVisibility(field))
                              {
                                    GUI.enabled = EvaluateEnabled(field);
                                    EditorGUILayout.PropertyField(property);
                              }
                        }
                        #endregion

                        #region S E R I A L I Z E D   M E TH O D S
                        foreach (MethodInfo method in serializedMethods)
                        {
                              if (!method.TryGetAttribute(out ButtonAttribute button)) continue;

                              if (EvaluateVisibility(method))
                              {
                                    GUI.enabled = EvaluateEnabled(method);
                                    GUI.backgroundColor = button.BackgroundColor;
                                    if (GUILayout.Button(button.Name ?? method.Name.ToTitleCase(), GUILayout.Height(button.Height)))
                                    {
                                          InvokeMethod(method, target, button.Parameters);
                                    }
                              }
                        }
                        #endregion

                        GUI.enabled = true;
                  }
                  serializedObject.ApplyModifiedProperties();
            }

            private bool EvaluateEnabled(MemberInfo member)
            {
                  bool output = true;

                  if (member.TryGetAttribute(out ReadonlyAttribute readonlyAttr))
                  {
                        output &= readonlyAttr.ExclusiveToPlaymode && !EditorApplication.isPlaying;
                  }
                  if (member.TryGetAttribute(out EnableWhenAttribute enableAttr))
                  {
                        output &= FetchValue(enableAttr.ConditionName, target) is bool enableFlag && enableFlag;
                  }
                  if (member.TryGetAttribute(out DisableWhenAttribute disableAttr))
                  {
                        output &= FetchValue(disableAttr.ConditionName, target) is bool disableFlag && !disableFlag;
                  }
                  return output;
            }
            private bool EvaluateVisibility(MemberInfo member)
            {
                  bool output = true;

                  if (member.TryGetAttribute(out ShowWhenAttribute showAttr))
                  {
                        output &= FetchValue(showAttr.ConditionName, target) is bool showFlag && showFlag;
                  }
                  if (member.TryGetAttribute(out HideWhenAttribute hideAttr))
                  {
                        output &= FetchValue(hideAttr.ConditionName, target) is bool hideFlag && !hideFlag;
                  }
                  return output;
            }
      }
}