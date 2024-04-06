using UnityEngine;

namespace Emp37.Utility.Tween
{
      public class Builder
      {
            private readonly Transform transform;
            private readonly float duration;
            private readonly Ease.Type type;

            private float delay;
            private TimeMode delta;

            private bool init;


            public Builder(Transform transform, float duration, Ease.Type type)
            {
                  this.transform = transform;
                  this.duration = duration;
                  this.type = type;
            }

            public void Update()
            {
                  float deltaTime = delta switch
                  {
                        TimeMode.Unscaled => Time.unscaledDeltaTime,
                        _ => Time.deltaTime,
                  };

                  if (delay > 0F)
                  {
                        delay -= deltaTime;

                        return;
                  }

            }


            public Builder Delay(float value)
            {
                  delay = value;

                  return this;
            }
      }
}