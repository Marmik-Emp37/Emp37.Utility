using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(ExpandableObjectAttribute))]
      internal class AttributeDrawer_ExpandableObject : BasePropertyDrawer // ~Warped Imagination
      {
            private UnityEditor.Editor editor = null;

            public override void Initialize(SerializedProperty property)
            {
                  property.isExpanded = false;
            }
            public override void Draw(Rect position, SerializedProperty property, GUIContent label)
            {
#if UNITY_2022_1_OR_NEWER
                  if (property.propertyType != SerializedPropertyType.ObjectReference)
                  {
                        ShowInvalidUsageBox(position, SerializedPropertyType.ObjectReference);
                        return;
                  }

                  EditorGUI.PropertyField(position, property, label);
                  property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, true);
                  if (property.objectReferenceValue != null && property.isExpanded)
                  {
                        using (new EditorGUI.IndentLevelScope(1))
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                              if (editor == null)
                              {
                                    UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                              }
                              else
                              {
                                    position.y += EditorGUI.GetPropertyHeight(property);
                                    editor.OnInspectorGUI();
                              }
                        }
                  }
#else
                  EditorGUI.HelpBox(position, $"The {typeof(ExpandableObjectAttribute).Name} is not supported in Unity versions older than 2022.", UnityEditor.MessageType.Error);
#endif
            }
      }
}