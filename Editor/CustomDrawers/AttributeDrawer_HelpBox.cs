using UnityEditor;
using static UnityEditor.EditorGUIUtility;
using UnityEngine;

namespace Emp37.Utility.Editor
{
	[CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
	internal class AttributeDrawer_HelpBox : DecoratorDrawer
	{
		private const float minHeight = 21F;

		private static readonly Texture[] icons = { default, IconContent("console.infoicon").image, IconContent("console.warnicon").image, IconContent("console.erroricon").image, };


		public override void OnGUI(Rect position)
		{
			position.height -= standardVerticalSpacing;
			var attr = attribute as HelpBoxAttribute;

			var content = new GUIContent(attr.Text, icons[(int) attr.Type]);
			EditorGUI.LabelField(position, content, EditorStyles.helpBox);
		}
		public override float GetHeight()
		{
			var attr = attribute as HelpBoxAttribute;

			var content = new GUIContent(attr.Text, icons[(int) attr.Type]);
			var contentHeight = EditorStyles.helpBox.CalcHeight(content, EditorGUIHelper.ReleventWidth);

			return Mathf.Max(minHeight, contentHeight) + standardVerticalSpacing;
		}
	}
}