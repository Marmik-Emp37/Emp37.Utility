using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;


      #region B A S E   E D I T O R S
      [CanEditMultipleObjects, CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
      internal class MonoBehaviourEditor : Emp37Editor
      {
      }

      [CanEditMultipleObjects, CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
      internal class ScriptableObjectEditor : Emp37Editor
      {
      }
      #endregion

      internal class Emp37Editor : UnityEditor.Editor
      {
            private Type targetType;

            private SerializedProperty m_Script;
            private SerializedProperty[] serializedProperties;

            private MethodInfo[] methods;

            private bool shouldHideDefaultScript;


            private void OnEnable()
            {
                  targetType = target.GetType();
                  shouldHideDefaultScript = targetType.IsDefined(typeof(HideDefaultScriptAttribute));

                  #region I N I T I A L I Z E   P R O P E R T I E S
                  if (serializedProperties == null)
                  {
                        var properties = new List<SerializedProperty>();
                        var iterator = serializedObject.GetIterator();
                        while (iterator.NextVisible(true))
                        {
                              var property = serializedObject.FindProperty(iterator.name);
                              if (property != null)
                              {
                                    if (property.name == nameof(m_Script))
                                    {
                                          m_Script = property;
                                          continue;
                                    }
                                    properties.Add(property);
                              }
                        }
                        serializedProperties = properties.ToArray();
                  }
                  #endregion

                  #region I N I T I A L I Z E   M E T H O D S
                  methods = targetType.GetMethods(DEFAULT_FLAGS);
                  #endregion
            }
            public override void OnInspectorGUI()
            {
                  serializedObject.Update();
                  {
                        #region D E F A U L T   F I E L D
                        if (m_Script != null && !shouldHideDefaultScript)
                        {
                              GUI.enabled = false;
                              EditorGUILayout.PropertyField(m_Script);
                        }
                        #endregion

                        #region S E R I A L I Z E D   P R O P E R T I E S
                        foreach (var property in serializedProperties)
                        {
                              var field = FetchInfo<FieldInfo>(property.name, targetType);
                              if (!EvaluateVisibility(field)) continue;

                              GUI.enabled = EvaluateEnabled(field);
                              EditorGUILayout.PropertyField(property);
                        }
                        #endregion

                        #region B U T T O N S
                        foreach (var method in methods)
                        {
                              var button = method.GetCustomAttribute<ButtonAttribute>();
                              if (button != null && EvaluateVisibility(method))
                              {
                                    GUI.enabled = EvaluateEnabled(method);
                                    GUI.backgroundColor = ColorLibrary.Pick(button.Shade);

                                    if (GUILayout.Button(method.Name, GUILayout.Height(button.Height)))
                                    {
                                          InvokeWithNamedParametersAndReflection(method, target, button.Parameters);
                                    }
                              }
                        }
                        #endregion

                        GUI.enabled = true;
                  }
                  serializedObject.ApplyModifiedProperties();
            }

            private bool EvaluateVisibility(MemberInfo member)
            {
                  var showAttribute = member.GetCustomAttribute<ShowWhenAttribute>();
                  if (showAttribute != null)
                  {
                        if (FetchValue(showAttribute.ConditionName, target) is bool value)
                        {
                              return value;
                        }
                  }
                  var hideAttribute = member.GetCustomAttribute<HideWhenAttribute>();
                  if (hideAttribute != null)
                  {
                        if (FetchValue(hideAttribute.ConditionName, target) is bool value)
                        {
                              return !value;
                        }
                  }
                  return true;
            }
            private bool EvaluateEnabled(MemberInfo member)
            {
                  var enableAttribute = member.GetCustomAttribute<EnableWhenAttribute>();
                  if (enableAttribute != null)
                  {
                        if (FetchValue(enableAttribute.ConditionName, target) is bool value)
                        {
                              return value;
                        }
                  }
                  var disableAttribute = member.GetCustomAttribute<DisableWhenAttribute>();
                  if (disableAttribute != null)
                  {
                        if (FetchValue(disableAttribute.ConditionName, target) is bool value)
                        {
                              return !value;
                        }
                  }
                  return true;
            }
      }
}