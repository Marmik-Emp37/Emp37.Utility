using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
      internal class AttributeDrawer_HelpBox : BaseDecoratorDrawer
      {
            private HelpBoxAttribute Attribute => attribute as HelpBoxAttribute;

            private GUIContent icon, message;
            private static readonly GUIStyle contentStyle = new(EditorStyles.label)
            {
                  alignment = TextAnchor.MiddleLeft,
                  wordWrap = true,
                  richText = true,
            };

            private const float MinHeight = 24F;


            public override void Initialize()
            {
                  MessageType type = Attribute.MessageType;
                  icon = type is 0 ? null : EditorGUIUtility.IconContent($"console.{type switch { MessageType.Warning => "warnicon", MessageType.Error => "erroricon", _ => "infoicon" }}");
                  message = new(Attribute.Message);
            }
            public override void Draw(Rect position)
            {
                  position.height = Mathf.Max(MinHeight, contentStyle.CalcHeight(message, position.width)); // - [ 1 ]
                  EditorGUI.HelpBox(position, string.Empty, 0);
                  if (icon != null)
                  {
                        EditorGUI.LabelField(position, icon, contentStyle);
                        position = position.Indent(contentStyle.CalcSize(new(icon)).x);
                  }
                  EditorGUI.LabelField(position, message, contentStyle);
            }
            public override float GetHeight() => Mathf.Max(MinHeight, contentStyle.CalcHeight(message, EditorGUIHelper.ReleventWidth)) /*- [ 1 ]*/ + EditorGUIUtility.standardVerticalSpacing;
      }
}