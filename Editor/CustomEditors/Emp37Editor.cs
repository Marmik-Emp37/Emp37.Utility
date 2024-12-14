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

            private bool showMonoScript;
            private SerializedProperty monoScript;

            private SerializedProperty[] serializedProperties;
            private MethodInfo[] serializedMethods;


            private void OnEnable()
            {
                  targetType = target.GetType();

                  showMonoScript = !targetType.IsDefined(typeof(HideDefaultScriptAttribute));

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
                        monoScript = properties.Dequeue();
                        serializedProperties = properties.ToArray();
                  }
                  #endregion

                  #region I N I T I A L I Z E   M E T H O D S
                  serializedMethods = targetType.GetMethods(DefaultFlags);
                  #endregion
            }
            public override void OnInspectorGUI()
            {
                  serializedObject.Update();
                  {
                        #region D E F A U L T   S C R I P T
                        if (showMonoScript)
                        {
                              GUI.enabled = false;
                              EditorGUILayout.PropertyField(monoScript);
                        }
                        #endregion

                        bool isLayoutActive = false;

                        #region S E R I A L I Z E D   P R O P E R T I E S
                        foreach (SerializedProperty property in serializedProperties)
                        {
                              FieldInfo field = FetchInfo<FieldInfo>(property.name, targetType);
                              EvaluateLayout(field, ref isLayoutActive);
                              if (EvaluateVisibility(field))
                              {
                                    GUI.enabled = EvaluateEnabled(field);
                                    EditorGUILayout.PropertyField(property);
                              }
                        }
                        if (isLayoutActive)
                        {
                              isLayoutActive = false;
                              EditorGUILayout.EndHorizontal();
                        }
                        #endregion

                        #region S E R I A L I Z E D   M E TH O D S
                        foreach (MethodInfo method in serializedMethods)
                        {
                              EvaluateLayout(method, ref isLayoutActive);
                              if (method.TryGetAttribute(out ButtonAttribute button) && EvaluateVisibility(method))
                              {
                                    GUI.enabled = EvaluateEnabled(method);
                                    GUI.backgroundColor = button.BackgroundColor;

                                    if (GUILayout.Button(button.Name ?? method.Name.ToTitleCase(), GUILayout.Height(button.Height)))
                                    {
                                          InvokeMethod(method, target, button.Parameters);
                                    }
                              }
                        }
                        if (isLayoutActive)
                        {
                              EditorGUILayout.EndHorizontal();
                        }
                        #endregion

                        GUI.enabled = true;
                  }
                  serializedObject.ApplyModifiedProperties();
            }

            private void EvaluateLayout(MemberInfo member, ref bool state)
            {
                  if (member.TryGetAttribute(out HorizontalGroupAttribute horizontalGroup) && horizontalGroup.State != state)
                  {
                        if (state = horizontalGroup.State)
                        {
                              EditorGUILayout.BeginHorizontal();
                        }
                        else
                        {
                              EditorGUILayout.EndHorizontal();
                        }
                  }
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
