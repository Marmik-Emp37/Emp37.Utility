using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
#pragma warning disable IDE1006 // Naming Styles
      public interface ITweenAction
      {
            public Element executeMove(Vector3? value = null);
            public Element executeRotate(Vector3? value = null);
            public Element executeScale(Vector3? value = null);
            public Element executeAlpha(float? value = null);
      }
#pragma warning restore IDE1006
}