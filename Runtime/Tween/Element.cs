using System;

using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
      using static Ease;


      public class Element : ITweenAction
      {
            private bool initialized;
            private readonly Transform transform;
            private readonly float durationMultiplier;
            private readonly Vector3 target;
            private Vector3 initial;
            private Delta deltaMode;
            private float delay, elapsedRatio, overshoot = 1F, period = 0.37F;
            private Func<float> easeMethod;
            private UnityAction onInitialize, onStart, onComplete;
            private UnityAction<float> onUpdate;
            private UnityAction<Vector3> onEase;

            public bool IsComplete { get; private set; }
            public bool IsValid => transform && durationMultiplier > 0F && initial != target;


            private Element(Transform transform, Vector3 target, float duration)
            {
                  this.transform = transform;
                  this.target = target;
                  durationMultiplier = 1F / duration;
                  easeMethod = () => Linear(elapsedRatio);
            }

            public static ITweenAction Create(Transform transform, Vector3 target, float duration) => new Element(transform, target, duration);

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
                        onInitialize.Invoke();
                        initialized = true;
                        onStart?.Invoke();
                  }
                  if (elapsedRatio < 1F)
                  {
                        elapsedRatio = Mathf.Clamp01(elapsedRatio + deltaTime * durationMultiplier);
                        float easedRatio = easeMethod.Invoke();
                        Vector3 interpolatedResult = Vector3.LerpUnclamped(initial, target, easedRatio);
                        onEase.Invoke(interpolatedResult);
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
                        Type.InSine => () => InSine(elapsedRatio),
                        Type.OutSine => () => OutSine(elapsedRatio),
                        Type.InOutSine => () => InOutSine(elapsedRatio),
                        Type.InCubic => () => InCubic(elapsedRatio),
                        Type.OutCubic => () => OutCubic(elapsedRatio),
                        Type.InOutCubic => () => InOutCubic(elapsedRatio),
                        Type.InQuint => () => InQuint(elapsedRatio),
                        Type.OutQuint => () => OutQuint(elapsedRatio),
                        Type.InOutQuint => () => InOutQuint(elapsedRatio),
                        Type.InCirc => () => InCirc(elapsedRatio),
                        Type.OutCirc => () => OutCirc(elapsedRatio),
                        Type.InOutCirc => () => InOutCirc(elapsedRatio),
                        Type.InQuad => () => InQuad(elapsedRatio),
                        Type.OutQuad => () => OutQuad(elapsedRatio),
                        Type.InOutQuad => () => InOutQuad(elapsedRatio),
                        Type.InQuart => () => InQuart(elapsedRatio),
                        Type.OutQuart => () => OutQuart(elapsedRatio),
                        Type.InOutQuart => () => InOutQuart(elapsedRatio),
                        Type.InExpo => () => InExpo(elapsedRatio),
                        Type.OutExpo => () => OutExpo(elapsedRatio),
                        Type.InOutExpo => () => InOutExpo(elapsedRatio),
                        Type.InBack => () => InBack(elapsedRatio, overshoot),
                        Type.OutBack => () => OutBack(elapsedRatio, overshoot),
                        Type.InOutBack => () => InOutBack(elapsedRatio, overshoot),
                        Type.InElastic => () => InElastic(elapsedRatio, overshoot, period),
                        Type.OutElastic => () => OutElastic(elapsedRatio, overshoot, period),
                        Type.InOutElastic => () => InOutElastic(elapsedRatio, overshoot, period),
                        Type.InBounce => () => InBounce(elapsedRatio),
                        Type.OutBounce => () => OutBounce(elapsedRatio),
                        Type.InOutBounce => () => InOutBounce(elapsedRatio),
                        Type.BreakOutBounce => () => BreakOutBounce(elapsedRatio),
                        _ => () => Linear(elapsedRatio)
                  };
                  return this;
            }
            public Element setDelay(float value) { delay = value; return this; }
            public Element setDelta(Delta value) { deltaMode = value; return this; }
            public Element setOvershoot(float value) { overshoot = value; return this; }
            public Element setPeriod(float value) { period = value; return this; }
            public Element setOnStart(UnityAction action) { onStart = action; return this; }
            public Element setOnUpdate(UnityAction<float> action) { onUpdate = action; return this; }
            public Element setOnComplete(UnityAction action) { onComplete = action; return this; }


            #region A C T I O N S
            Element ITweenAction.setMove()
            {
                  onInitialize = () => initial = transform.position;
                  onEase = value => transform.position = value;
                  return this;
            }
            Element ITweenAction.setRotate()
            {
                  onInitialize = () => initial = transform.eulerAngles;
                  onEase = value => transform.eulerAngles = value;
                  return this;
            }
            Element ITweenAction.setScale()
            {
                  onInitialize = () => initial = transform.localScale;
                  onEase = value => transform.localScale = value;
                  return this;
            }
            Element ITweenAction.setAlpha()
            {
                  if (transform.TryGetComponent(out CanvasGroup group))
                  {
                        onInitialize = () => initial = group.alpha * Vector3.one;
                        onEase = value => group.alpha = value.x;
                  }
                  else
                  {
                        throw new MissingComponentException($"'{transform.name}' is missing a {nameof(CanvasGroup)} component, which is required for alpha tweening.");
                  }
                  return this;
            }
            #endregion
      }
}