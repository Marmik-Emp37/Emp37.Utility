using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Utility.Tweening
{
      using static Utility;

      public static class Extensions
      {
            public static Element TweenMove(this Transform transform, Vector3 target, float duration)
            {
                  Element tween = new(() => transform.position, target, duration, value => transform.position = value, $"{transform.GetInstanceID()}.Move");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveX(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.position, ReplaceAxis(transform.position, 0, target), duration, value => transform.position = value, $"{transform.GetInstanceID()}.MoveX");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveY(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.position, ReplaceAxis(transform.position, 1, target), duration, value => transform.position = value, $"{transform.GetInstanceID()}.MoveY");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveZ(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.position, ReplaceAxis(transform.position, 2, target), duration, value => transform.position = value, $"{transform.GetInstanceID()}.MoveZ");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveLocal(this Transform transform, Vector3 target, float duration)
            {
                  Element tween = new(() => transform.localPosition, target, duration, value => transform.localPosition = value, $"{transform.GetInstanceID()}.MoveLocal");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveLocalX(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localPosition, ReplaceAxis(transform.localPosition, 0, target), duration, value => transform.localPosition = value, $"{transform.GetInstanceID()}.MoveLocalX");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveLocalY(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localPosition, ReplaceAxis(transform.localPosition, 1, target), duration, value => transform.localPosition = value, $"{transform.GetInstanceID()}.MoveLocalY");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenMoveLocalZ(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localPosition, ReplaceAxis(transform.localPosition, 2, target), duration, value => transform.localPosition = value, $"{transform.GetInstanceID()}.MoveLocalZ");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotate(this Transform transform, Vector3 target, float duration)
            {
                  Element tween = new(() => transform.eulerAngles, target, duration, value => transform.eulerAngles = value, $"{transform.GetInstanceID()}.Rotate");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateX(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.eulerAngles, ReplaceAxis(transform.eulerAngles, 0, target), duration, value => transform.eulerAngles = value, $"{transform.GetInstanceID()}.RotateX");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateY(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.eulerAngles, ReplaceAxis(transform.eulerAngles, 1, target), duration, value => transform.eulerAngles = value, $"{transform.GetInstanceID()}.RotateY");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateZ(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.eulerAngles, ReplaceAxis(transform.eulerAngles, 2, target), duration, value => transform.eulerAngles = value, $"{transform.GetInstanceID()}.RotateZ");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateLocal(this Transform transform, Vector3 target, float duration)
            {
                  Element tween = new(() => transform.localEulerAngles, target, duration, value => transform.localEulerAngles = value, $"{transform.GetInstanceID()}.RotateLocal");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateLocalX(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localEulerAngles, ReplaceAxis(transform.localEulerAngles, 0, target), duration, value => transform.localEulerAngles = value, $"{transform.GetInstanceID()}.RotateLocalX");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateLocalY(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localEulerAngles, ReplaceAxis(transform.localEulerAngles, 1, target), duration, value => transform.localEulerAngles = value, $"{transform.GetInstanceID()}.RotateLocalY");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenRotateLocalZ(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localEulerAngles, ReplaceAxis(transform.localEulerAngles, 2, target), duration, value => transform.localEulerAngles = value, $"{transform.GetInstanceID()}.RotateLocalZ");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenScale(this Transform transform, Vector3 target, float duration)
            {
                  Element tween = new(() => transform.localScale, target, duration, value => transform.localScale = value, $"{transform.GetInstanceID()}.Scale");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenScaleX(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localScale, ReplaceAxis(transform.localScale, 0, target), duration, value => transform.localScale = value, $"{transform.GetInstanceID()}.ScaleX");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenScaleY(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localScale, ReplaceAxis(transform.localScale, 1, target), duration, value => transform.localScale = value, $"{transform.GetInstanceID()}.ScaleY");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenScaleZ(this Transform transform, float target, float duration)
            {
                  Element tween = new(() => transform.localScale, ReplaceAxis(transform.localScale, 2, target), duration, value => transform.localScale = value, $"{transform.GetInstanceID()}.ScaleZ");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenAlpha(this CanvasGroup group, float target, float duration)
            {
                  Element tween = new(() => group.alpha, target, duration, value => group.alpha = value, $"{group.GetInstanceID()}.Alpha");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenAlpha(this Image image, float target, float duration)
            {
                  Element tween = new(() => image.color.a, target, duration, value =>
                  {
                        var color = image.color;
                        color.a = value;
                        image.color = color;
                  }, $"{image.GetInstanceID()}.Alpha");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenAlpha(this SpriteRenderer renderer, float target, float duration)
            {
                  Element tween = new(() => renderer.color.a, target, duration, value =>
                  {
                        var color = renderer.color;
                        color.a = value;
                        renderer.color = color;
                  }, $"{renderer.GetInstanceID()}.Alpha");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenColor(this Image image, Color target, float duration)
            {
                  Vector3 init()
                  {
                        Color color = image.color;
                        return new(color.r, color.g, color.b);
                  }
                  Element tween = new(init, new(target.r, target.g, target.b), duration, value => image.color = new(value.x, value.y, value.z), $"{image.GetInstanceID()}.Color");
                  Engine.Add(tween);
                  return tween;
            }

            public static Element TweenColor(this SpriteRenderer renderer, Color target, float duration)
            {
                  Vector3 init()
                  {
                        Color color = renderer.color;
                        return new(color.r, color.g, color.b);
                  }
                  Element tween = new(init, new(target.r, target.g, target.b), duration, value => renderer.color = new(value.x, value.y, value.z), $"{renderer.GetInstanceID()}.Color");
                  Engine.Add(tween);
                  return tween;
            }
      }
}