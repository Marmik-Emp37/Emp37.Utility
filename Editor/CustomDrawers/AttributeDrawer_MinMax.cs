using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	using Type = SerializedPropertyType;

	[CustomPropertyDrawer(typeof(MinMaxAttribute))]
	internal class AttributeDrawer_MinMax : PropertyDrawer
	{
		private const float FieldWidth = 50F, Spacing = 4F;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType is not (Type.Vector2 or Type.Vector2Int))
			{
				EditorGUIHelper.DrawAttributeError(position, attribute.GetType().Name, Type.Vector2, Type.Vector2Int);
				return;
			}
			var attr = attribute as MinMaxAttribute;

			bool isInt = property.propertyType is Type.Vector2Int;
			Vector2 value = isInt ? property.vector2IntValue : property.vector2Value;

			using (new EditorGUI.PropertyScope(position, label, property))
			{
				position = EditorGUI.PrefixLabel(position, label);

				int indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				Rect minRect = new(position) { width = FieldWidth }, maxRect = new(minRect) { x = position.xMax - FieldWidth };
				Rect sliderRect = new(minRect.xMax + Spacing, position.y, maxRect.x - minRect.xMax - 2F * Spacing, position.height);

				using (EditorGUI.ChangeCheckScope scope = new())
				{
					float min = EditorGUI.FloatField(minRect, value.x), max = EditorGUI.FloatField(maxRect, value.y);
					EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, attr.Min, attr.Max);
					if (scope.changed)
					{
						min = Mathf.Clamp(min, attr.Min, attr.Max);
						max = Mathf.Clamp(max, min, attr.Max);
						if (isInt) property.vector2IntValue = new(Mathf.RoundToInt(min), Mathf.RoundToInt(max));
						else property.vector2Value = new(min, max);
					}
				}
				EditorGUI.indentLevel = indent;
			}
		}
	}
}