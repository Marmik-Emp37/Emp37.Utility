using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
        [CustomPropertyDrawer(typeof(SeparatorAttribute), true)]
        internal class AttributeDrawer_Separator : DecoratorDrawer
        {
                public override void OnGUI(Rect position)
                {
                        var attr = attribute as SeparatorAttribute;

                        if (attr.Stretch)
                        {
                                position.x = 0F;
                                position.width = EditorGUIUtility.currentViewWidth;
                        }
                        position.height = attr.Thickness;

                        EditorGUI.DrawRect(position, attr.Color);
                }
                public override float GetHeight()
                {
                        var attr = attribute as SeparatorAttribute;

                        return attr.Thickness + EditorGUIUtility.standardVerticalSpacing;
                }
        }
}