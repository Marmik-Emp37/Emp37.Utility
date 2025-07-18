using System;

using UnityEngine;

namespace Emp37.Utility.Tweening
{
      using static Ease;

      public class Element
      {
            private Vector3 a;
            private readonly Vector3 b;
            private Delta timeMode;
            private bool initialized;

            private readonly float inverseDuration;
            private float progress, delay;

            private readonly Action onTweenInitialize;
            private Function easingFunction = Linear;
            private readonly Action<Vector3> onTweenUpdate;

            private Action onStart;
            private Action<float> onUpdate = delegate { };
            private Action onComplete;

            public object Key { get; private set; }
            public bool IsComplete { get; private set; }
            public bool IsInvalid
            {
                  get
                  {
                        bool output = false;
                        if (inverseDuration <= 0F)
                        {
                              print($"Invalid duration {1F / inverseDuration} value. It must be greater than 0.");
                              output = true;
                        }
                        if (onTweenInitialize == null)
                        {
                              print($"Missing initialization callback. Call IBindableTween.Bind to assign an 'inital' method that assigns the inital value after the tween is processed.");
                              output = true;
                        }
                        if (onTweenUpdate == null)
                        {
                              print($"Missing update callback. Call IBindableTween.Bind to assign a 'tween' method that updates the tween target.");
                              output = true;
                        }
                        return output;

                        static void print(string message) => Debug.LogWarning($"[{typeof(Element).FullName}] Validation Failed: {message}");
                  }
            }


            public Element(Func<float> initial, float target, float duration, Action<float> tween, object key)
            {
                  if (initial != null)
                  {
                        onTweenInitialize = () => a = new(initial(), 0F);
                  }
                  b = new(target, 0F);
                  inverseDuration = 1F / duration;
                  if (tween != null)
                  {
                        onTweenUpdate = v => tween(v.x);
                  }
                  Key = key;
            }
            public Element(Func<Vector3> initial, Vector3 target, float duration, Action<Vector3> tween, object key)
            {
                  if (initial != null)
                  {
                        onTweenInitialize = () => a = initial();
                  }
                  b = target;
                  inverseDuration = 1F / duration;
                  if (tween != null)
                  {
                        onTweenUpdate = tween;
                  }
                  Key = key;
            }

            internal void Update()
            {
                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;

                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }

                  if (!initialized)
                  {
                        initialized = true;
                        onTweenInitialize();

                        onStart?.Invoke();
                  }

                  progress = Mathf.Clamp01(progress + deltaTime * inverseDuration);
                  float eased = easingFunction(progress);
                  Vector3 current = Vector3.LerpUnclamped(a, b, eased);
                  onTweenUpdate(current);

                  onUpdate(eased);

                  if (progress == 1F)
                  {
                        IsComplete = true;
                        onComplete?.Invoke();
                  }
            }
            public void Reset()
            {
                  progress = 0F;
                  initialized = IsComplete = false;
            }

            #region B U I L D E R   P A T T E R N
            public Element SetEase(Type type)
            {
                  easingFunction = type switch
                  {
                        Type.InSine => InSine,
                        Type.OutSine => OutSine,
                        Type.InOutSine => InOutSine,
                        Type.InCubic => InCubic,
                        Type.OutCubic => OutCubic,
                        Type.InOutCubic => InOutCubic,
                        Type.InQuint => InQuint,
                        Type.OutQuint => OutQuint,
                        Type.InOutQuint => InOutQuint,
                        Type.InCirc => InCirc,
                        Type.OutCirc => OutCirc,
                        Type.InOutCirc => InOutCirc,
                        Type.InQuad => InQuad,
                        Type.OutQuad => OutQuad,
                        Type.InOutQuad => InOutQuad,
                        Type.InQuart => InQuart,
                        Type.OutQuart => OutQuart,
                        Type.InOutQuart => InOutQuart,
                        Type.InExpo => InExpo,
                        Type.OutExpo => OutExpo,
                        Type.InOutExpo => InOutExpo,
                        Type.InBack => InBack,
                        Type.OutBack => OutBack,
                        Type.InOutBack => InOutBack,
                        Type.InElastic => InElastic,
                        Type.OutElastic => OutElastic,
                        Type.InOutElastic => InOutElastic,
                        Type.InBounce => InBounce,
                        Type.OutBounce => OutBounce,
                        Type.InOutBounce => InOutBounce,
                        _ => Linear
                  };
                  return this;
            }
            public Element SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
            /// <summary>
            /// Sets the delay before the tween starts.
            /// </summary>
            /// <param name="duration">In seconds.</param>
            public Element SetDelay(float duration) { delay = duration; return this; }
            /// <summary>
            /// Sets the time scale mode (scaled or unscaled) used by the tween.
            /// </summary>
            public Element SetTimeMode(Delta value) { timeMode = value; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween starts.
            /// </summary>
            public Element SetOnStart(Action action) { onStart = action; return this; }
            /// <summary>
            /// Sets a callback to invoke during each tween update.
            /// </summary>
            public Element SetOnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween completes.
            /// </summary>
            public Element SetOnComplete(Action action) { onComplete = action; return this; }
            #endregion
      }
}