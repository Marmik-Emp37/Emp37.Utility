using UnityEditor;
using UnityEngine;

namespace Emp37.Utility.Editor
{
        public static class EditorGUIHelper
        {
                public static float ReleventWidth => EditorGUIUtility.currentViewWidth - 22F;


                public class BackgroundColorScope : GUI.Scope
                {
                        private readonly Color original = GUI.backgroundColor;

                        public Color BackgroundColor { get => GUI.backgroundColor; set => GUI.backgroundColor = value; }


                        public BackgroundColorScope(Color color) => GUI.backgroundColor = color;

                        protected override void CloseScope() => GUI.backgroundColor = original;
                }
                public class ContentColorScope : GUI.Scope
                {
                        private readonly Color original = GUI.contentColor;

                        public Color ContentColor { get => GUI.contentColor; set => GUI.contentColor = value; }


                        public ContentColorScope(Color color) => GUI.contentColor = color;

                        protected override void CloseScope() => GUI.contentColor = original;
                }


                public static void DrawAttributeError(Rect position, string attributeName, params SerializedPropertyType[] expectedTypes) => EditorGUI.HelpBox(position, $"Use {attributeName} on fields of type {string.Join(" | ", expectedTypes)}.", UnityEditor.MessageType.Error);
        }
}