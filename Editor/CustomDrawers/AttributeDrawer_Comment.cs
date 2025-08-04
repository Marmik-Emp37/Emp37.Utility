using System;
using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	[CustomPropertyDrawer(typeof(CommentAttribute), true)]
	internal class AttributeDrawer_Comment : DecoratorDrawer
	{
		private const float backgroundAlpha = 0.175F;
		private const float highlightWidth = 3F;
		private const float minHeight = 21F;

		private static readonly GUIStyle[] contentStyles = CreateContentGUIStyles();


		public override void OnGUI(Rect position)
		{
			position.size = new(EditorGUIHelper.ReleventWidth, position.height - EditorGUIUtility.standardVerticalSpacing);
			var attr = attribute as CommentAttribute;

			EditorGUI.LabelField(position, attr.Content, contentStyles[(int) attr.Style]);

			var color = attr.Tint;
			color.a = backgroundAlpha;

			EditorGUI.DrawRect(position, color);

			color.a = 1F;

			EditorGUI.DrawRect(new(position) { x = position.x - highlightWidth + 1F, width = highlightWidth }, color);
		}
		public override float GetHeight()
		{
			var attr = attribute as CommentAttribute;

			var contentHeight = contentStyles[(int) attr.Style].CalcHeight(attr.Content, EditorGUIHelper.ReleventWidth);

			return Mathf.Max(minHeight, contentHeight) + EditorGUIUtility.standardVerticalSpacing;
		}

		private static GUIStyle[] CreateContentGUIStyles()
		{
			var values = Enum.GetValues(typeof(FontStyle));

			var styles = new GUIStyle[values.Length];
			foreach (FontStyle style in values)
			{
				styles[(int) style] = new(EditorStyles.label) { richText = true, wordWrap = true, fontStyle = style };
			}
			return styles;
		}
	}
}