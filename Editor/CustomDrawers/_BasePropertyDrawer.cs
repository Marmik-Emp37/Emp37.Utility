using UnityEngine;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      internal abstract class BasePropertyDrawer : PropertyDrawer
      {
            private bool hasInitialized;


            public virtual void Initialize(SerializedProperty property) { }
            public abstract void Draw(Rect position, SerializedProperty property, GUIContent label);

            public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  if (!hasInitialized)
                  {
                        Initialize(property);
                        hasInitialized = true;
                  }
                  EditorGUI.BeginProperty(position, label, property);
                  Draw(position, property, label);
                  EditorGUI.EndProperty();
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);
      }
}