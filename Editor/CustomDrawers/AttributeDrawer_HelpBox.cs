using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
      internal class AttributeDrawer_HelpBox : BaseDecoratorDrawer
      {
            private HelpBoxAttribute Attribute => attribute as HelpBoxAttribute;

            private static readonly GUIStyle contentStyle = new(EditorStyles.label)
            {
                  alignment = TextAnchor.MiddleLeft,
                  wordWrap = true,
                  richText = true,
            };

            private float BoxHeight => Mathf.Max(24F, contentStyle.CalcHeight(Attribute.Content, EditorGUIHelper.ReleventWidth));

            public override void Initialize()
            {
                  MessageType type = Attribute.MessageType;
                  Attribute.Content.image = type is 0 ? default : EditorGUIUtility.IconContent($"console.{type switch { MessageType.Warning => "warnicon", MessageType.Error => "erroricon", _ => "infoicon" }}").image;
            }
            public override void Draw(Rect position)
            {
                  position.height = BoxHeight; // - [ 1 ]
                  EditorGUI.HelpBox(position, string.Empty, 0);
                  EditorGUI.LabelField(position.Indent(2F), Attribute.Content);
            }
            public override float GetHeight() => BoxHeight /* - [ 1 ]*/ + EditorGUIUtility.standardVerticalSpacing;
      }
}