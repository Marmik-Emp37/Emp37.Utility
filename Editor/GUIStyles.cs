using UnityEditor;
using UnityEngine;

namespace Emp37.Utility.Editor
{
	public static class GUIStyles
	{
		public static GUIStyle greyMiniLabelRight = new(EditorStyles.centeredGreyMiniLabel)
		{
			alignment = TextAnchor.MiddleRight
		};
		public static readonly GUIStyle title = new(EditorStyles.boldLabel)
		{
			fontSize = 14,
			stretchHeight = false
		};
	}
}