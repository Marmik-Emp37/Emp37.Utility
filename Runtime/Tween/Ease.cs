using static System.MathF;

namespace Emp37.Utility.Tween
{
      public static class Ease
      {
            public enum Type
            {
                  Linear,
                  InSine,
                  OutSine,
                  InOutSine,
                  InCubic,
                  OutCubic,
                  InOutCubic,
                  InQuint,
                  OutQuint,
                  InOutQuint,
                  InCirc,
                  OutCirc,
                  InOutCirc,
                  InQuad,
                  OutQuad,
                  InOutQuad,
                  InQuart,
                  OutQuart,
                  InOutQuart,
                  InExpo,
                  OutExpo,
                  InOutExpo,
                  InBack,
                  OutBack,
                  InOutBack,
                  InBounce,
                  OutBounce,
                  InOutBounce,
                  BreakOutBounce,
                  InElastic,
                  OutElastic,
                  InOutElastic,
            }


            #region E A S I N G   F U N C T I O N S
            public static float Linear(float progress)
            {
                  return progress;
            }

            // S I N E
            public static float InSine(float progress)
            {
                  return 1F - Cos(progress * PI * 0.5F);
            }
            public static float OutSine(float progress)
            {
                  return Sin(progress * PI * 0.5F);
            }
            public static float InOutSine(float progress)
            {
                  return -0.5F * (Cos(progress * PI) - 1F);
            }

            // C U B I C
            public static float InCubic(float progress)
            {
                  return progress * progress * progress;
            }
            public static float OutCubic(float progress)
            {
                  progress--;
                  return progress * progress * progress + 1F;
            }
            public static float InOutCubic(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return 4F * progress * progress * progress;
                  }
                  else
                  {
                        progress = -2F * progress + 2F;
                        return 1F - (progress * progress * progress * 0.5F);
                  }
            }

            // Q U I N T
            public static float InQuint(float progress)
            {
                  return progress * progress * progress * progress * progress;
            }
            public static float OutQuint(float progress)
            {
                  progress = 1F - progress;
                  return 1F - progress * progress * progress * progress * progress;
            }
            public static float InOutQuint(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return 16F * progress * progress * progress * progress * progress;
                  }
                  else
                  {
                        progress = (-2F * progress) + 2F;
                        return 1F - (progress * progress * progress * progress * progress * 0.5F);
                  }
            }

            // C I R C
            public static float InCirc(float progress)
            {
                  if (progress <= 1F)
                  {
                        return 1F - Sqrt(1F - progress * progress);
                  }
                  else
                  {
                        return 1F;
                  }
            }
            public static float OutCirc(float progress)
            {
                  progress--;
                  return Sqrt(1F - progress * progress);
            }
            public static float InOutCirc(float progress)
            {
                  if (progress < 0.5F)
                  {
                        progress *= 2F;
                        return 0.5F * (1F - Sqrt(1F - (progress * progress)));
                  }
                  else
                  {
                        progress = -2F * progress + 2F;
                        return 0.5F * (Sqrt(1F - (progress * progress)) + 1F);
                  }
            }

            // Q U A D
            public static float InQuad(float progress)
            {
                  return progress * progress;
            }
            public static float OutQuad(float progress)
            {
                  progress = 1F - progress;
                  return 1F - progress * progress;
            }
            public static float InOutQuad(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return progress * progress * 2F;
                  }
                  else
                  {
                        progress = (progress * -2F) + 2F;
                        return 1F - (progress * progress * 0.5F);
                  }
            }

            // Q U A R T
            public static float InQuart(float progress)
            {
                  return progress * progress * progress * progress;
            }
            public static float OutQuart(float progress)
            {
                  progress--;
                  return -(progress * progress * progress * progress - 1F);
            }
            public static float InOutQuart(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return 8F * progress * progress * progress * progress;
                  }
                  else
                  {
                        progress = (-2F * progress) + 2F;
                        return 1F - (progress * progress * progress * progress * 0.5F);
                  }
            }

            // E X P O
            public static float InExpo(float progress)
            {
                  return Pow(2F, 10F * progress - 10F);
            }
            public static float OutExpo(float progress)
            {
                  if (progress == 1F)
                  {
                        return 1F;
                  }
                  else
                  {
                        return 1F - Pow(2F, -10F * progress);
                  }
            }
            public static float InOutExpo(float progress)
            {
                  if (progress == 0F) return 0F;
                  else if (progress == 1F) return 1F;

                  float e = 20F * progress - 10F;

                  if (progress < 0.5F)
                  {
                        return Pow(2F, e) * 0.5F;
                  }
                  else
                  {
                        return (2F - Pow(2F, -e)) * 0.5F;
                  }
            }

            // B A C K
            private const float easeBackSpringConstant = 1.70158F;
            public static float InBack(float progress, float overshoot)
            {
                  float spring = easeBackSpringConstant * overshoot;

                  return progress * progress * ((spring + 1F) * progress - spring);
            }
            public static float OutBack(float progress, float overshoot)
            {
                  float spring = easeBackSpringConstant * overshoot;

                  progress--;
                  return progress * progress * ((spring + 1F) * progress + spring) + 1F;
            }
            public static float InOutBack(float progress, float overshoot)
            {
                  const float midpointTension = 1.525F;
                  float spring = easeBackSpringConstant * overshoot * midpointTension;

                  if (progress < 0.5F)
                  {
                        progress *= 2F;
                        return progress * progress * ((spring + 1F) * progress - spring) * 0.5F;
                  }
                  else
                  {
                        progress = (progress * 2F) - 2F;
                        return (progress * progress * ((spring + 1F) * progress + spring) + 2F) * 0.5F;
                  }
            }

            // B O U N C E
            public static float InBounce(float progress)
            {
                  return 1F - OutBounce(1F - progress);
            }
            public static float OutBounce(float progress)
            {
                  const float effect = 2.75F, strength = 7.5625F;
                  const float c1 = 1F / effect, c2 = 2F / effect, c3 = 2.5F / effect;
                  const float exitOffset = 2.625F / effect;

                  if (progress < c1)
                  {
                        return strength * progress * progress;
                  }
                  else if (progress < c2)
                  {
                        progress -= 1.5F / effect;
                        return strength * progress * progress + 0.75F;
                  }
                  else if (progress < c3)
                  {
                        progress -= 2.25F / effect;
                        return strength * progress * progress + 0.9375F;
                  }
                  else
                  {
                        progress -= exitOffset;
                        return strength * progress * progress + 0.984375F;
                  }
            }
            public static float InOutBounce(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return (1F - OutBounce(1F - (progress * 2F))) * 0.5F;
                  }
                  else
                  {
                        return (1F + OutBounce((2F * progress) - 1F)) * 0.5F;
                  }
            }
            public static float BreakOutBounce(float progress)
            {
                  return (progress < 0.5F ? 1F - InBounce(1F - (2F * progress)) : 1F + OutBounce((2F * progress) - 1F)) * 0.5F;
            }

            // E L A S T I C
            public static float InElastic(float progress, float overshoot, float period)
            {
                  if (progress == 0F) return 0F;
                  else if (progress == 1F) return 1F;

                  float shift = period / 4F;

                  if (overshoot > 1F && progress > 0.6F)
                  {
                        overshoot = 1F + ((1F - progress) / 0.4F * (overshoot - 1F));
                  }
                  progress--;
                  return -(Pow(2F, 10F * progress) * Sin((progress - shift) * (2F * PI) / period)) * overshoot;
            }
            public static float OutElastic(float progress, float overshoot, float period)
            {
                  if (progress == 0F) return 0F;
                  else if (progress == 1F) return 1F;

                  float shift = period / 4F;

                  if (overshoot > 1F && progress < 0.4F)
                  {
                        overshoot = 1F + (progress / 0.4F * (overshoot - 1F));
                  }
                  return 1F + Pow(2F, -10F * progress) * Sin((progress - shift) * (2F * PI) / period) * overshoot;
            }
            public static float InOutElastic(float progress, float overshoot, float period)
            {
                  if (progress == 0F) return 0F; else if (progress == 1F) return 1F;

                  progress = (progress * 2F) - 1F;
                  if (overshoot > 1F)
                  {
                        float relativeProgress = progress + 1F;
                        if (relativeProgress < 0.2F)
                        {
                              overshoot = 1F + (relativeProgress / 0.2F * (overshoot - 1F));
                        }
                        if (relativeProgress > 0.8F)
                        {
                              overshoot = 1F + ((1F - relativeProgress) / 0.2F * (overshoot - 1F));
                        }
                  }

                  float shift = period / 4F;
                  float scaledProgress = 10F * progress, sineFactor = Sin((progress - shift) * (2F * PI) / period);

                  if (progress < 0F)
                  {
                        return -0.5F * (Pow(2F, scaledProgress) * sineFactor) * overshoot;
                  }
                  else
                  {
                        return 1F + 0.5F * Pow(2F, -scaledProgress) * sineFactor * overshoot;
                  }
            }
            #endregion
      }
}