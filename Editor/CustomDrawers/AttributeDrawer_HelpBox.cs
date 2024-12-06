using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
      internal class AttributeDrawer_HelpBox : BaseDecoratorDrawer
      {
            private HelpBoxAttribute Attribute => attribute as HelpBoxAttribute;

            private const float BoxOffset = -4F, TextOffset = 34F;

            private GUIContent Icon;
            private static readonly GUIStyle labelStyle = new(EditorStyles.label)
            {
                  alignment = TextAnchor.MiddleLeft,
                  wordWrap = true,
            };


            public override void Initialize()
            {
                  var type = Attribute.MessageType;
                  Icon = type is 0 ? null : EditorGUIUtility.IconContent("console." + type switch { MessageType.Warning => "warnicon", MessageType.Error => "erroricon", _ => "infoicon", });
            }
            public override void OnGUI(Rect position)
            {
                  base.OnGUI(position);
                  position.height = Attribute.Height;

                  EditorGUI.HelpBox(position.Indent(BoxOffset), string.Empty, 0);

                  if (Icon != null)
                  {
                        EditorGUI.LabelField(position, Icon);
                        position = position.Indent(TextOffset);
                  }
                  EditorGUI.LabelField(position, Attribute.Message, labelStyle);
            }
            public override float GetHeight() => Attribute.Height + EditorGUIUtility.standardVerticalSpacing;
      }
}