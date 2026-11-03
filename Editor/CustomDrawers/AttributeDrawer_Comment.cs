using System;
using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	[CustomPropertyDrawer(typeof(CommentAttribute), true)]
	internal class AttributeDrawer_Comment : DecoratorDrawer
	{
		private const float BackgroundAlpha = 0.175F;
		private const float HighlightWidth = 3F;
		private const float MinHeight = 21F;

		private static readonly GUIStyle[] contentStyles = CreateContentGUIStyles();


		public override void OnGUI(Rect position)
		{
			var attr = attribute as CommentAttribute;

			position.size = new(EditorGUIHelper.ReleventWidth, position.height - EditorGUIUtility.standardVerticalSpacing);
			EditorGUI.LabelField(position, attr.Content, contentStyles[(int) attr.Style]);

			Color color = attr.Tint;
			color.a = BackgroundAlpha;
			EditorGUI.DrawRect(position, color);
			color.a = 1F;

			EditorGUI.DrawRect(new(position) { x = position.x - HighlightWidth + 1F, width = HighlightWidth }, color);
		}
		public override float GetHeight()
		{
			var attr = attribute as CommentAttribute;

			var contentHeight = contentStyles[(int) attr.Style].CalcHeight(attr.Content, EditorGUIHelper.ReleventWidth);
			return Mathf.Max(MinHeight, contentHeight) + EditorGUIUtility.standardVerticalSpacing;
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