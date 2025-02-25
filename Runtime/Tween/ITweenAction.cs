using System;

using UnityEngine;

namespace Emp37.Utility.Tweening
{
      public interface ITweenAction
      {
            internal Transform Transform { get; }
            public Element Bind(Vector3 from, Action<Vector3> apply);

            public Element Move() => Bind(Transform.position, value => Transform.position = value);
            public Element MoveLocal() => Bind(Transform.localPosition, value => Transform.localPosition = value);
            public Element Rotate() => Bind(Transform.eulerAngles, value => Transform.eulerAngles = value);
            public Element RotateLocal() => Bind(Transform.localEulerAngles, value => Transform.localEulerAngles = value);
            public Element Scale() => Bind(Transform.localScale, value => Transform.localScale = value);
            public Element CanvasAlpha()
            {
                  if (Transform.TryGetComponent(out CanvasGroup group)) return Bind(new(group.alpha, 0F), value => group.alpha = value.x);
                  Debug.LogError($"No {typeof(CanvasGroup).Name} component found on '{Transform.name}'.");
                  return null;
            }
      }
}