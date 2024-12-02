using UnityEditor;

namespace Emp37.Utility.Editor
{
      [CanEditMultipleObjects, CustomEditor(typeof(UnityEngine.MonoBehaviour), true, isFallback = true)]
      internal class MonoBehaviourEditor : Emp37Editor
      {
      }
}