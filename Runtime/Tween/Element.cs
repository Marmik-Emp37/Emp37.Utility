using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility.Tweening
{
      using static Ease;

      public class Element : ITweenAction
      {
            // Configuration
            private readonly Transform transform;
            private Vector3 a;
            private readonly Vector3 b;
            private Delta timeMode;
            private bool hasInitialized;

            // Timing
            private readonly float inverseDuration;
            private float progress, delay;

            // Easing
            private Func<float> easeFunction = () => 1F;
            private float easeOvershoot, easePeriod;

            // Callbacks
            private Action onInitialize = delegate { };
            private Action onStart;
            private Action<Vector3> onValueChanged = delegate { };
            private Action<float> onUpdate = delegate { };
            private Action onComplete;

            public bool IsComplete { get; private set; }
            public bool IsInvalid
            {
                  get
                  {
                        List<string> warnings = new();
                        if (!transform) warnings.Add($"Missing or null {typeof(Transform).Name} reference.");
                        if (inverseDuration <= 0F) warnings.Add($"Invalid duration value: {1F / inverseDuration}. It must be greater than 0.");
                        if (warnings.Count > 0)
                        {
                              warnings.ForEach(warning => Debug.LogWarning("Validation Failed: " + warning));
                              return true;
                        }
                        return false;
                  }
            }

            private Element(Transform transform, Vector3 end, float duration)
            {
                  this.transform = transform;
                  b = end;
                  inverseDuration = 1F / duration;
                  easeFunction = () => Linear(progress);
            }
            public static ITweenAction Create(Transform transform, Vector3 end, float duration) => new Element(transform, end, duration);

            Transform ITweenAction.Transform => transform;
            Element ITweenAction.Bind(Vector3 from, Action<Vector3> apply)
            {
                  onInitialize = () => a = from;
                  onValueChanged = apply;
                  return this;
            }

            internal void Update()
            {
                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;

                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }

                  if (!hasInitialized)
                  {
                        hasInitialized = true;
                        onInitialize();

                        onStart?.Invoke();
                  }

                  progress = Mathf.Clamp01(progress + deltaTime * inverseDuration);
                  float easedRatio = easeFunction();
                  Vector3 current = Vector3.LerpUnclamped(a, b, easedRatio);
                  onValueChanged(current);

                  onUpdate(easedRatio);

                  if (progress == 1F)
                  {
                        IsComplete = true;
                        onComplete?.Invoke();
                  }
            }
            public void Reset()
            {
                  progress = 0F;
                  hasInitialized = IsComplete = false;
            }

            #region B U I L D E R   P A T T E R N
#pragma warning disable IDE1006
            public Element setEase(Type type)
            {
                  easeFunction = type switch
                  {
                        Type.InSine => () => InSine(progress),
                        Type.OutSine => () => OutSine(progress),
                        Type.InOutSine => () => InOutSine(progress),
                        Type.InCubic => () => InCubic(progress),
                        Type.OutCubic => () => OutCubic(progress),
                        Type.InOutCubic => () => InOutCubic(progress),
                        Type.InQuint => () => InQuint(progress),
                        Type.OutQuint => () => OutQuint(progress),
                        Type.InOutQuint => () => InOutQuint(progress),
                        Type.InCirc => () => InCirc(progress),
                        Type.OutCirc => () => OutCirc(progress),
                        Type.InOutCirc => () => InOutCirc(progress),
                        Type.InQuad => () => InQuad(progress),
                        Type.OutQuad => () => OutQuad(progress),
                        Type.InOutQuad => () => InOutQuad(progress),
                        Type.InQuart => () => InQuart(progress),
                        Type.OutQuart => () => OutQuart(progress),
                        Type.InOutQuart => () => InOutQuart(progress),
                        Type.InExpo => () => InExpo(progress),
                        Type.OutExpo => () => OutExpo(progress),
                        Type.InOutExpo => () => InOutExpo(progress),
                        Type.InBack => () => InBack(progress, easeOvershoot),
                        Type.OutBack => () => OutBack(progress, easeOvershoot),
                        Type.InOutBack => () => InOutBack(progress, easeOvershoot),
                        Type.InElastic => () => InElastic(progress, easeOvershoot, easePeriod),
                        Type.OutElastic => () => OutElastic(progress, easeOvershoot, easePeriod),
                        Type.InOutElastic => () => InOutElastic(progress, easeOvershoot, easePeriod),
                        Type.InBounce => () => InBounce(progress),
                        Type.OutBounce => () => OutBounce(progress),
                        Type.InOutBounce => () => InOutBounce(progress),
                        _ => () => Linear(progress)
                  };
                  return this;
            }
            /// <summary>
            /// Sets the delay before the tween starts.
            /// </summary>
            /// <param name="duration">In seconds.</param>
            public Element setDelay(float duration) { delay = duration; return this; }
            /// <summary>
            /// Sets the time scale mode (scaled or unscaled) used by the tween.
            /// </summary>
            public Element setTimeMode(Delta value) { timeMode = value; return this; }
            /// <summary>
            /// Sets the overshoot value for applicable easing types.
            /// </summary>
            /// <remarks>Applies only to Back and Elastic easing functions. Default is <c>1</c>.</remarks>
            public Element setEaseOvershoot(float value) { easeOvershoot = value; return this; }
            /// <summary>
            /// Sets the period value for applicable easing types.
            /// </summary>
            /// <remarks>Applies only to Elastic easing functions.</remarks>
            public Element setEasePeriod(float amount) { easePeriod = amount; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween starts.
            /// </summary>
            public Element setOnStart(Action action) { onStart = action; return this; }
            /// <summary>
            /// Sets a callback to invoke during each tween update.
            /// </summary>
            public Element setOnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween completes.
            /// </summary>
            public Element setOnComplete(Action action) { onComplete = action; return this; }
#pragma warning restore IDE1006
            #endregion
      }
}