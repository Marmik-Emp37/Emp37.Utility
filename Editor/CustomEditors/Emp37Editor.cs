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

            private bool showDefaultScript;
            private SerializedProperty defaultScript;

            private SerializedProperty[] serializedProperties;
            private MethodInfo[] serializedMethods;


            private void OnEnable()
            {
                  targetType = target.GetType();

                  showDefaultScript = !targetType.IsDefined(typeof(HideDefaultScriptAttribute));

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

                        defaultScript = properties[0];
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
                  serializedObject.Update();

                  #region D R A W   D E F A U L T   S C R I P T
                  if (showDefaultScript && defaultScript != null)
                  {
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(defaultScript);
                  }
                  #endregion

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

                  GUI.enabled = true;

                  serializedObject.ApplyModifiedProperties();
            }

            private bool EvaluateEnabled(MemberInfo member)
            {
                  bool output = true;
                  var a0 = member.GetCustomAttribute<ReadonlyAttribute>(true);
                  if (a0 != null) output &= a0.ExclusiveToPlaymode && !EditorApplication.isPlaying;
                  var a1 = member.GetCustomAttribute<EnableWhenAttribute>(true);
                  if (a1 != null) output &= ReadAny(a1.ConditionName, target) is bool value && value;
                  var a2 = member.GetCustomAttribute<DisableWhenAttribute>(true);
                  if (a2 != null) output &= ReadAny(a2.ConditionName, target) is bool value && !value;
                  return output;
            }
            private bool EvaluateVisibility(MemberInfo member)
            {
                  bool output = true;
                  var a0 = member.GetCustomAttribute<ShowWhenAttribute>(true);
                  if (a0 != null) output &= ReadAny(a0.ConditionName, target) is bool value && value;
                  var a1 = member.GetCustomAttribute<HideWhenAttribute>(true);
                  if (a1 != null) output &= ReadAny(a1.ConditionName, target) is bool value && !value;
                  return output;
            }
      }
}