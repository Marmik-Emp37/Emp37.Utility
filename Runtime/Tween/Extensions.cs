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
                  var position = transform.localPosition;
                  position.x = target;
                  return TweenMoveLocal(transform, position, duration);
            }
            public static Element TweenMoveLocalY(this Transform transform, float target, float duration)
            {
                  var position = transform.localPosition;
                  position.y = target;
                  return TweenMoveLocal(transform, position, duration);
            }
            public static Element TweenMoveLocalZ(this Transform transform, float target, float duration)
            {
                  var position = transform.localPosition;
                  position.z = target;
                  return TweenMoveLocal(transform, position, duration);
            }

            public static Element TweenRotate(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Element.Build(transform, target, duration).executeRotate();
                  Engine.Push(tween);
                  return tween;
            }
            public static Element TweenRotateX(this Transform transform, float target, float duration)
            {
                  var rotation = transform.eulerAngles;
                  rotation.x = target;
                  return TweenRotate(transform, rotation, duration);
            }
            public static Element TweenRotateY(this Transform transform, float target, float duration)
            {
                  var rotation = transform.eulerAngles;
                  rotation.y = target;
                  return TweenRotate(transform, rotation, duration);
            }
            public static Element TweenRotateZ(this Transform transform, float target, float duration)
            {
                  var rotation = transform.eulerAngles;
                  rotation.z = target;
                  return TweenRotate(transform, rotation, duration);
            }

            public static Element TweenScale(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Element.Build(transform, target, duration).executeScale();
                  Engine.Push(tween);
                  return tween;
            }
            public static Element TweenScaleX(this Transform transform, float target, float duration)
            {
                  var scale = transform.localScale;
                  scale.x = target;
                  return TweenScale(transform, scale, duration);
            }
            public static Element TweenScaleY(this Transform transform, float target, float duration)
            {
                  var scale = transform.localScale;
                  scale.y = target;
                  return TweenScale(transform, scale, duration);
            }
            public static Element TweenScaleZ(this Transform transform, float target, float duration)
            {
                  var scale = transform.localScale;
                  scale.z = target;
                  return TweenScale(transform, scale, duration);
            }

            public static Element TweenAlpha(this CanvasGroup group, float target, float duration)
            {
                  var tween = Element.Build(group.transform, new(target, 0F), duration).executeCanvasAlpha();
                  Engine.Push(tween);
                  return tween;
            }

            public static Element TweenAlpha(this SpriteRenderer renderer, float target, float duration)
            {
                  var tween = Element.Build(renderer.transform, new(target, 0F), duration).executeSpriteAlpha();
                  Engine.Push(tween);
                  return tween;
            }
            public static Element TweenTint(this SpriteRenderer renderer, Color target, float duration)
            {
                  var tween = Element.Build(renderer.transform, new(target.r, target.g, target.b), duration).executeSpriteTint();
                  Engine.Push(tween);
                  return tween;
            }
      }
}