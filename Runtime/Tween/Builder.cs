using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
#pragma warning disable IDE1006
      public abstract class Builder<T> where T : struct
      {
            private readonly Transform transform;

            private T start;
            private readonly T end;

            private bool initialised, completed;
            private float delay, overshoot = 1F, elapsedTime;
            private Delta deltaMode;
            private readonly Ease.Type tweenType;
            private readonly float duration;

            private UnityAction onStart, onComplete;
            protected Transform Transform => transform;


            public Builder(Transform transform, float duration, T end, Ease.Type tweenType)
            {
                  this.transform = transform;
                  this.duration = duration;
                  this.end = end;
                  this.tweenType = tweenType;
            }

            protected abstract void Init(ref T a);
            protected abstract T InterpolateValues(T a, T b, float t);
            protected abstract void OnEase(T value);

            internal void Update()
            {
                  if (completed) return;
                  if (delay > 0F)
                  {
                        delay -= Time.deltaTime;
                        return;
                  }
                  if (!initialised)
                  {
                        Init(ref start);
                        initialised = true;

                        onStart?.Invoke();
                  }
                  if (elapsedTime < 1F)
                  {
                        var deltaTime = deltaMode switch
                        {
                              Delta.Unscaled => Time.unscaledDeltaTime,
                              _ => Time.deltaTime
                        };
                        elapsedTime = Mathf.Clamp01(elapsedTime + deltaTime / duration);
                        T calculatedValue = InterpolateValues(start, end, Ease.EasedRatio(tweenType, elapsedTime, overshoot));
                        OnEase(calculatedValue);
                  }
                  else
                  {
                        onComplete?.Invoke();
                        completed = true;
                  }
            }

            public Builder<T> setDelay(float value) { delay = value; return this; }
            public Builder<T> setOvershoot(float value) { overshoot = value; return this; }
            public Builder<T> setDelta(Delta value) { deltaMode = value; return this; }
            public Builder<T> setOnStart(UnityAction action) { onStart = action; return this; }
            public Builder<T> setOnComplete(UnityAction action) { onComplete = action; return this; }
      }
#pragma warning restore IDE1006
}