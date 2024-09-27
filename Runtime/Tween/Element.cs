using System;
using System.Linq;
using System.Collections.Generic;

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
            private ITweenAction.Type actionType;
            private float delay, progress, overshoot = 1F, period = 0.37F;
            private Delta deltaMode;
            private Func<float> easeMethod;
            private UnityAction onInitialize, onStart, onComplete;
            private UnityAction<float> onUpdate;
            private UnityAction<Vector3> onTween;

            public bool IsComplete { get; private set; }
            public bool IsInvalid
            {
                  get
                  {
                        bool output;
                        List<string> warnings = new (bool condition, string message)[]
                        {
                              (!transform, "Missing or null Transform reference."),
                              (durationMultiplier <= 0F, $"Invalid duration {1F / durationMultiplier} value. It must be greater than 0."),
                              (initial == target, "Initial and target values are the same, no tweening necessary."),
                              (actionType == 0, "No tween action defined for this element."),
                        }.Where(warning => warning.condition).Select(warning => warning.message).ToList();

                        if (output = warnings.Any())
                        {
                              warnings.ForEach(warning => Debug.LogWarning("Validation Failed: " + warning));
                        }
                        return output;
                  }
            }


            private Element(Transform transform, Vector3 target, float duration)
            {
                  this.transform = transform;
                  this.target = target;
                  durationMultiplier = 1F / duration;
                  easeMethod = () => Linear(progress);
            }

            public static ITweenAction Build(Transform transform, Vector3 target, float duration) => new Element(transform, target, duration);

            internal void Update()
            {
                  if (IsComplete) return;

                  float deltaTime = (deltaMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;

                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }
                  if (!initialized)
                  {
                        initialized = true;
                        onInitialize();
                        onStart?.Invoke();
                  }
                  progress = Mathf.Clamp01(progress + deltaTime * durationMultiplier);

                  float easedRatio = easeMethod();
                  Vector3 value = Vector3.LerpUnclamped(initial, target, easedRatio);

                  onTween(value);
                  onUpdate?.Invoke(easedRatio);

                  if (progress == 1F)
                  {
                        IsComplete = true;
                        onComplete?.Invoke();
                  }
            }
            internal bool ConflictsWith(Element other) => transform == other.transform && actionType == other.actionType;

            #region T W E E N   C O N F I G U R A T I O N
            public Element setEase(Type type)
            {
                  easeMethod = type switch
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
                        Type.InBack => () => InBack(progress, overshoot),
                        Type.OutBack => () => OutBack(progress, overshoot),
                        Type.InOutBack => () => InOutBack(progress, overshoot),
                        Type.InElastic => () => InElastic(progress, overshoot, period),
                        Type.OutElastic => () => OutElastic(progress, overshoot, period),
                        Type.InOutElastic => () => InOutElastic(progress, overshoot, period),
                        Type.InBounce => () => InBounce(progress),
                        Type.OutBounce => () => OutBounce(progress),
                        Type.InOutBounce => () => InOutBounce(progress),
                        Type.BreakOutBounce => () => BreakOutBounce(progress),
                        _ => () => Linear(progress)
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
            #endregion

            #region T W E E N    A C T I O N S
            Element ITweenAction.executeMove(Vector3? value)
            {
                  actionType = ITweenAction.Type.Move;
                  onInitialize = () => initial = value ?? transform.position;
                  onTween = value => transform.position = value;
                  return this;
            }
            Element ITweenAction.executeMoveLocal(Vector3? value)
            {
                  actionType = ITweenAction.Type.MoveLocal;
                  onInitialize = () => initial = value ?? transform.localPosition;
                  onTween = value => transform.localPosition = value;
                  return this;
            }
            Element ITweenAction.executeRotate(Vector3? value)
            {
                  actionType = ITweenAction.Type.Rotate;
                  onInitialize = () => initial = value ?? transform.eulerAngles;
                  onTween = value => transform.eulerAngles = value;
                  return this;
            }
            Element ITweenAction.executeScale(Vector3? value)
            {
                  actionType = ITweenAction.Type.Scale;
                  onInitialize = () => initial = value ?? transform.localScale;
                  onTween = value => transform.localScale = value;
                  return this;
            }
            Element ITweenAction.executeCanvasAlpha(float? value)
            {
                  if (!transform.TryGetComponent(out CanvasGroup group))
                  {
                        throw new MissingComponentException($"'{transform.name}' is missing a {nameof(CanvasGroup)} component, which is required for alpha tweening.");
                  }
                  actionType = ITweenAction.Type.CanvasAlpha;
                  onInitialize = () => initial = new(x: value ?? group.alpha, y: 0F);
                  onTween = value => group.alpha = value.x;
                  return this;
            }
            #endregion
      }
#pragma warning restore IDE1006
}