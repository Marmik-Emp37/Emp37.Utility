using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	using Type = SerializedPropertyType;

	[CustomPropertyDrawer(typeof(UnitAttribute))]
	internal class AttributeDrawer_Unit : PropertyDrawer
	{
		private const float Padding = 4F;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType is not (Type.Integer or Type.Float or Type.String or Type.Vector2 or Type.Vector3 or Type.Vector4 or Type.Vector2Int or Type.Vector3Int))
			{
				EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, Type.Integer, Type.Float, Type.String, Type.Vector2, Type.Vector3);
				return;
			}
			var attr = attribute as UnitAttribute;
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				EditorGUI.PropertyField(position, property, label, true);

				if (Event.current.type is not EventType.Repaint) return;
				int indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				position.width -= Padding;
				GUI.Label(position, attr.Text, GUIStyles.greyMiniLabelRight);
				EditorGUI.indentLevel = indent;
			}
		}
	}
}