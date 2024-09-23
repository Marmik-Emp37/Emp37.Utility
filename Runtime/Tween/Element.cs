using System;

using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
      using static Ease;

#pragma warning disable IDE1006 // Naming Styles
      public class Element : ITweenAction
      {
            private bool initialized;
            private readonly Transform transform;
            private readonly float durationMultiplier;
            private readonly Vector3 target;
            private Vector3 initial;
            private float delay, progress, overshoot = 1F, period = 0.37F;
            private Delta deltaMode;
            private Func<float, float> easeMethod;
            private UnityAction onInitialize, onStart, onComplete;
            private UnityAction<float> onUpdate;
            private UnityAction<Vector3> onTween;

            public bool IsComplete { get; private set; }
            public bool IsValid => transform && durationMultiplier > 0F && initial != target;


            private Element(Transform transform, Vector3 target, float duration)
            {
                  this.transform = transform;
                  this.target = target;
                  durationMultiplier = 1F / duration;
                  easeMethod = value => Linear(value);
            }

            public static ITweenAction Build(Transform transform, Vector3 target, float duration) => new Element(transform, target, duration);

            internal void Update()
            {
                  if (IsComplete) return;

                  float deltaTime = deltaMode == Delta.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }
                  if (!initialized)
                  {
                        onInitialize();
                        initialized = true;
                        onStart?.Invoke();
                  }
                  if (progress < 1F)
                  {
                        progress = Mathf.Clamp01(progress + deltaTime * durationMultiplier);
                        float easedRatio = easeMethod(progress);
                        onTween(Vector3.LerpUnclamped(initial, target, easedRatio));
                        onUpdate?.Invoke(easedRatio);
                  }
                  else
                  {
                        IsComplete = true;
                        onComplete?.Invoke();
                  }
            }
            public Element setEase(Type type)
            {
                  easeMethod = type switch
                  {
                        Type.InSine => value => InSine(value),
                        Type.OutSine => value => OutSine(value),
                        Type.InOutSine => value => InOutSine(value),
                        Type.InCubic => value => InCubic(value),
                        Type.OutCubic => value => OutCubic(value),
                        Type.InOutCubic => value => InOutCubic(value),
                        Type.InQuint => value => InQuint(value),
                        Type.OutQuint => value => OutQuint(value),
                        Type.InOutQuint => value => InOutQuint(value),
                        Type.InCirc => value => InCirc(value),
                        Type.OutCirc => value => OutCirc(value),
                        Type.InOutCirc => value => InOutCirc(value),
                        Type.InQuad => value => InQuad(value),
                        Type.OutQuad => value => OutQuad(value),
                        Type.InOutQuad => value => InOutQuad(value),
                        Type.InQuart => value => InQuart(value),
                        Type.OutQuart => value => OutQuart(value),
                        Type.InOutQuart => value => InOutQuart(value),
                        Type.InExpo => value => InExpo(value),
                        Type.OutExpo => value => OutExpo(value),
                        Type.InOutExpo => value => InOutExpo(value),
                        Type.InBack => value => InBack(value, overshoot),
                        Type.OutBack => value => OutBack(value, overshoot),
                        Type.InOutBack => value => InOutBack(value, overshoot),
                        Type.InElastic => value => InElastic(value, overshoot, period),
                        Type.OutElastic => value => OutElastic(value, overshoot, period),
                        Type.InOutElastic => value => InOutElastic(value, overshoot, period),
                        Type.InBounce => value => InBounce(value),
                        Type.OutBounce => value => OutBounce(value),
                        Type.InOutBounce => value => InOutBounce(value),
                        Type.BreakOutBounce => value => BreakOutBounce(value),
                        _ => value => Linear(value)
                  };
                  return this;
            }
            /// <summary>
            /// Sets delay to the tween.
            /// </summary>
            /// <param name="value">In seconds.</param>
            public Element setDelay(float value) { delay = value; return this; }
            /// <summary>
            /// Sets the delta to use when tweening.
            /// </summary>
            public Element setDelta(Delta value) { deltaMode = value; return this; }
            /// <summary>
            /// Sets the overshoot value for easing types that use overshooting.
            /// </summary>
            /// <remarks>This value only affects 'Back and Elastic' easing types. The default is <c>1</c>.</remarks>
            public Element setOvershoot(float value) { overshoot = value; return this; }
            /// <summary>
            ///  Sets the period value for easing types that use a period.
            /// </summary>
            /// <remarks>This value only affects 'Elastic' easing types. The default is <c>0.37</c>.</remarks>
            public Element setPeriod(float value) { period = value; return this; }
            /// <summary>
            /// Sets a callback to be invoked when the tween starts.
            /// </summary>
            public Element setOnStart(UnityAction action) { onStart = action; return this; }
            /// <summary>
            /// Sets a callback to be invoked during each update of the tween.
            /// </summary>
            /// <param name="action">The action to be invoked on update, with the eased ratio as a parameter.</param>
            public Element setOnUpdate(UnityAction<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Sets a callback to be invoked when the tween completes.
            /// </summary>
            public Element setOnComplete(UnityAction action) { onComplete = action; return this; }

            #region T W E E N    A C T I O N S
            Element ITweenAction.executeMove()
            {
                  onInitialize = () => initial = transform.position;
                  onTween = value => transform.position = value;
                  return this;
            }
            Element ITweenAction.executeRotate()
            {
                  onInitialize = () => initial = transform.eulerAngles;
                  onTween = value => transform.eulerAngles = value;
                  return this;
            }
            Element ITweenAction.executeScale()
            {
                  onInitialize = () => initial = transform.localScale;
                  onTween = value => transform.localScale = value;
                  return this;
            }
            Element ITweenAction.executeAlpha()
            {
                  if (!transform.TryGetComponent(out CanvasGroup group))
                  {
                        throw new MissingComponentException($"'{transform.name}' is missing a {nameof(CanvasGroup)} component, which is required for alpha tweening.");
                  }
                  onInitialize = () => initial = group.alpha * Vector3.right;
                  onTween = value => group.alpha = value.x;
                  return this;
            }
            #endregion
      }
#pragma warning restore IDE1006
}