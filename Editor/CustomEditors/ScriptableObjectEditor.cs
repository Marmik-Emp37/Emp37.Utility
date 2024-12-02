using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CanEditMultipleObjects, CustomEditor(typeof(UnityEngine.ScriptableObject), true, isFallback = true)]
      internal class ScriptableObjectEditor : Emp37Editor
      {
      }
}