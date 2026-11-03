using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	[CustomPropertyDrawer(typeof(TitleAttribute), true)]
	internal class AttributeDrawer_Title : DecoratorDrawer
	{
		private const float UnderlineHeight = 2F;

		public override void OnGUI(Rect position)
		{
			var attr = attribute as TitleAttribute;

			var size = GUIStyles.title.CalcSize(attr.Content);
			position.height = size.y;
			using (new EditorGUIHelper.ContentColorScope(attr.Color))
			{
				EditorGUI.LabelField(position, attr.Content, GUIStyles.title);
			}
			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
			if (!attr.Stretch) position.width = size.x;
			position.height = UnderlineHeight;

			EditorGUI.DrawRect(position, attr.Color);
		}
		public override float GetHeight()
		{
			var attr = attribute as TitleAttribute;

			var size = GUIStyles.title.CalcSize(attr.Content);
			return size.y + 2F * EditorGUIUtility.standardVerticalSpacing + UnderlineHeight;
		}
	}
}