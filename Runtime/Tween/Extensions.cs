using UnityEngine;

namespace Emp37.Utility.Tween
{
      public static class Extensions
      {
            public static Element TweenMove(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Element.Build(transform, target, duration).executeMove();
                  Engine.Push(tween);
                  return tween;
            }
            public static Element TweenMoveX(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.x = target;
                  return TweenMove(transform, position, duration);
            }
            public static Element TweenMoveY(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.y = target;
                  return TweenMove(transform, position, duration);
            }
            public static Element TweenMoveZ(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.z = target;
                  return TweenMove(transform, position, duration);
            }

            public static Element TweenMoveLocal(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Element.Build(transform, target, duration).executeMoveLocal();
                  Engine.Push(tween);
                  return tween;
            }
            public static Element TweenMoveLocalX(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.x = target;
                  return TweenMove(transform, position, duration);
            }
            public static Element TweenMoveLocalY(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.y = target;
                  return TweenMove(transform, position, duration);
            }
            public static Element TweenMoveLocalZ(this Transform transform, float target, float duration)
            {
                  var position = transform.position;
                  position.z = target;
                  return TweenMove(transform, position, duration);
            }
      }
}