using UnityEngine;

namespace Emp37.Utility.Tween
{
      public class VectorTween : Builder<Vector3>
      {
            public enum Mode
            {
                  Position,
                  LocalPosition,
                  Rotation,
                  Scale
            }

            private readonly Mode mode;


            public VectorTween(Transform transform, float duration, Vector3 end, Ease.Type tweenType, Mode mode) : base(transform, duration, end, tweenType)
            {
                  this.mode = mode;
            }

            protected override void Initialisation(ref Vector3 a) => a = mode switch
            {
                  Mode.Position => Transform.position,
                  Mode.LocalPosition => Transform.localPosition,
                  Mode.Rotation => Transform.eulerAngles,
                  Mode.Scale => Transform.localScale,
                  _ => a,
            };
            protected override void OnEase(Vector3 value)
            {
                  switch (mode)
                  {
                        case Mode.Position:
                              Transform.position = value;
                              break;
                        case Mode.LocalPosition:
                              Transform.localPosition = value;
                              break;
                        case Mode.Rotation:
                              Transform.eulerAngles = value;
                              break;
                        case Mode.Scale:
                              Transform.localScale = value;
                              break;
                  }
            }
            protected override Vector3 Interpolation(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
      }
}