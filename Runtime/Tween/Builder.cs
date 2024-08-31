using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
#pragma warning disable IDE1006
      public abstract class Builder<T> where T : struct
      {
            private bool initialised, _isComplete;

            private T start;
            private readonly T end;

            private readonly Transform _transform;
            private readonly float durationMultiplier;
            private readonly Ease.Type tweenType;
            private Delta deltaMode;
            private float delay, overshoot = 1F, linearRatio;
            private UnityAction<float> onUpdate;
            private UnityAction onStart, onComplete;

            protected Transform Transform => _transform;
            public bool IsComplete => _isComplete;


            public Builder(Transform transform, float duration, T end, Ease.Type tweenType)
            {
                  _transform = transform;
                  durationMultiplier = 1F / duration;
                  this.end = end;
                  this.tweenType = tweenType;
            }

            protected abstract void Initialisation(ref T a);
            protected abstract void OnEase(T value);
            protected abstract T Interpolation(T a, T b, float t);

            internal void Update()
            {
                  if (_isComplete) return;

                  var deltaTime = deltaMode switch
                  {
                        Delta.Unscaled => Time.unscaledDeltaTime,
                        _ => Time.deltaTime
                  };
                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }
                  if (!initialised)
                  {
                        Initialisation(ref start);
                        initialised = true;

                        onStart?.Invoke();
                  }
                  if (linearRatio < 1F)
                  {
                        linearRatio = Mathf.Clamp01(linearRatio + deltaTime * durationMultiplier);
                        float progress = Ease.EasedRatio(tweenType, linearRatio, overshoot);
                        OnEase(value: Interpolation(start, end, progress));

                        onUpdate?.Invoke(progress);
                  }
                  else
                  {
                        onComplete?.Invoke();
                        _isComplete = true;
                  }
            }

            public Builder<T> setDelta(Delta value) { deltaMode = value; return this; }
            public Builder<T> setDelay(float value) { delay = value; return this; }
            public Builder<T> setOvershoot(float value) { overshoot = value; return this; }
            public Builder<T> setOnStart(UnityAction action) { onStart = action; return this; }
            public Builder<T> setOnUpdate(UnityAction<float> action) { onUpdate = action; return this; }
            public Builder<T> setOnComplete(UnityAction action) { onComplete = action; return this; }
      }
#pragma warning restore IDE1006
}