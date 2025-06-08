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

            private SerializedProperty[] serializedProperties;
            private MethodInfo[] serializedMethods;

            private bool showDefaultProperty;
            private SerializedProperty defaultProperty;

            private bool isHorizontalLayoutActive;


            private void OnEnable()
            {
                  targetType = target.GetType();

                  showDefaultProperty = !HasAttribute<HideDefaultPropertyAttribute>(targetType);

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
                  serializedMethods = targetType.GetMethods(DEFAULT_FLAGS).Where(static method => method.IsDefined(typeof(ButtonAttribute), true)).ToArray();
                  #endregion
            }

            public override void OnInspectorGUI()
            {
                  if (TryGetAttributes(targetType, out NoteAttribute[] notes, true))
                  {
                        foreach (NoteAttribute attribute in notes)
                              using (new EditorGUIHelper.BackgroundColorScope(attribute.Color))
                                    EditorGUILayout.HelpBox(attribute.Content);
                  }

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

                              EvaluateGroup(field);

                              GUI.enabled = EvaluateEnabled(field);
                              EditorGUILayout.PropertyField(property, true);
                        }
                        EndActiveGroup();
                        #endregion

                        #region D R A W   S E R I A L I Z E D   M E TH O D S
                        foreach (MethodInfo method in serializedMethods)
                        {
                              ButtonAttribute button = GetAttribute<ButtonAttribute>(method, true);
                              if (button == null || !EvaluateVisibility(method)) continue;

                              EvaluateGroup(method);

                              GUI.enabled = EvaluateEnabled(method);
                              GUI.backgroundColor = button.BackgroundColor;
                              if (GUILayout.Button(button.Name ?? Utility.ToTitleCase(method.Name), GUILayout.Height(button.Height)))
                              {
                                    AutoInvokeMethod(method, target, button.Parameters);
                              }
                        }
                        EndActiveGroup();
                        #endregion
                  }
                  serializedObject.ApplyModifiedProperties();

                  GUI.enabled = true;
            }

            private void EvaluateGroup(ICustomAttributeProvider provider)
            {
                  if (!TryGetAttribute(provider, out HorizontalAttribute horizontal)) return;

                  if (horizontal.BeginGroup && !isHorizontalLayoutActive)
                  {
                        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                        isHorizontalLayoutActive = true;
                  }
                  else
                  if (!horizontal.BeginGroup)
                  {
                        EndActiveGroup();
                  }
            }
            private void EndActiveGroup()
            {
                  if (isHorizontalLayoutActive)
                  {
                        EditorGUILayout.EndHorizontal();
                        isHorizontalLayoutActive = false;
                  }
            }
            private bool EvaluateVisibility(ICustomAttributeProvider provider)
            {
                  bool output = true;

                  if (TryGetAttribute(provider, out ShowWhenAttribute a0, true))
                        output &= ReadMember(a0.ConditionName, target) is bool value && value;
                  if (TryGetAttribute(provider, out HideWhenAttribute a1, true))
                        output &= ReadMember(a1.ConditionName, target) is bool value && !value;

                  return output;
            }
            private bool EvaluateEnabled(ICustomAttributeProvider provider)
            {
                  bool output = true;

                  if (TryGetAttribute(provider, out ReadonlyAttribute a0, true))
                        output &= a0.ExclusiveToPlaymode && !EditorApplication.isPlaying;
                  if (TryGetAttribute(provider, out EnableWhenAttribute a1, true))
                        output &= ReadMember(a1.ConditionName, target) is bool value && value;
                  if (TryGetAttribute(provider, out DisableWhenAttribute a2, true))
                        output &= ReadMember(a2.ConditionName, target) is bool value && !value;

                  return output;
            }
      }
}