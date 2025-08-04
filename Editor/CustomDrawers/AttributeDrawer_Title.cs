using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(TitleAttribute), true)]
        internal class AttributeDrawer_Title : DecoratorDrawer
        {
                private const float underlineHeight = 2F;

                private static readonly GUIStyle labelStyle = new(EditorStyles.boldLabel)
                {
                        fontSize = 14,
                        stretchHeight = false,
                };


                public override void OnGUI(Rect position)
                {
                        var attr = attribute as TitleAttribute;

                        var size = labelStyle.CalcSize(attr.Content);
                        position.height = size.y;
                        using (new EditorGUIHelper.ContentColorScope(attr.Color))
                        {
                                EditorGUI.LabelField(position, attr.Content, labelStyle);
                        }

                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                        if (!attr.Stretch) position.width = size.x;
                        position.height = underlineHeight;

                        EditorGUI.DrawRect(position, attr.Color);
                }
                public override float GetHeight()
                {
                        var attr = attribute as TitleAttribute;

                        var size = labelStyle.CalcSize(attr.Content);
                        return size.y + 2F * EditorGUIUtility.standardVerticalSpacing + underlineHeight;
                }
        }
}