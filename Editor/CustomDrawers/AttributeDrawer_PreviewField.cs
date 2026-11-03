using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	[CustomPropertyDrawer(typeof(PreviewFieldAttribute))]
	internal class AttributeDrawer_PreviewField : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType is not SerializedPropertyType.ObjectReference)
			{
				EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, SerializedPropertyType.ObjectReference);
				return;
			}
			var attr = attribute as PreviewFieldAttribute;

			using (new EditorGUI.PropertyScope(position, label, property))
			{
				Rect labelRect = new(position) { height = EditorGUIUtility.singleLineHeight };
				labelRect = EditorGUI.PrefixLabel(labelRect, label);
				Rect objectRect = new(labelRect.x, position.y, attr.Extent, attr.Extent);

				int indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				using (EditorGUI.ChangeCheckScope scope = new())
				{
					bool allowSceneObjects = !EditorUtility.IsPersistent(property.serializedObject.targetObject);
					var value = EditorGUI.ObjectField(objectRect, property.objectReferenceValue, GetObjectType(), allowSceneObjects);
					if (scope.changed)
					{
						property.objectReferenceValue = value;
					}
				}
				EditorGUI.indentLevel = indent;
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.propertyType is not SerializedPropertyType.ObjectReference) return base.GetPropertyHeight(property, label);

			var attr = attribute as PreviewFieldAttribute;
			return attr.Extent + EditorGUIUtility.standardVerticalSpacing;
		}

		private Type GetObjectType()
		{
			Type type = fieldInfo.FieldType;
			if (type.IsArray) return type.GetElementType();
			if (type.IsGenericType && typeof(IList).IsAssignableFrom(type)) return type.GetGenericArguments()[0];
			return type;
		}
	}
}