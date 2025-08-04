using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(ExpandableObjectAttribute))]
        internal class AttributeDrawer_ExpandableObject : PropertyDrawer // ~Warped Imagination
        {
                private UnityEditor.Editor editor = null;


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        if (property.propertyType != SerializedPropertyType.ObjectReference)
                        {
                                EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, SerializedPropertyType.ObjectReference);
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
                }
        }
}