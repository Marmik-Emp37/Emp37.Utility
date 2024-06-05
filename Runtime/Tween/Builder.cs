using UnityEngine;
using UnityEngine.Events;

namespace Emp37.Utility.Tween
{
      using static Ease;


      internal abstract class Builder<T> where T : struct
      {
            internal enum Delta { Scaled, Unscaled }

            private protected delegate T Fetch();
            private protected delegate void Assign(T value);

            private float elapsed;
            private bool initialised;
            private float delay, overshoot = 1F;
            private Delta delta;
            private UnityAction onComplete;
            private protected T a;
            private readonly T b;
            private readonly float duration;
            private readonly Type type;
            private protected readonly Transform transform;
            internal bool IsComplete { get; private set; }
            internal bool IsValid => transform && duration > 0F && !a.Equals(b);

            internal Builder(Transform transform, T b, float duration, Type type)
            {
                  this.transform = transform;
                  this.b = b;
                  this.duration = duration;
                  this.type = type;
                  onComplete = delegate { IsComplete = true; };
            }

            private protected abstract void Initialize();
            private protected abstract void OnEase(float ratio);

            internal void Update()
            {
                  float deltaTime = delta switch
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
                        Initialize();
                        initialised = true;
                  }

                  elapsed = Mathf.Clamp01(elapsed + deltaTime / duration);
                  float T = EasedRatio(elapsed, type, overshoot);
                  OnEase(ratio: T);

                  if (elapsed == 1F) onComplete.Invoke();
            }
      }
}