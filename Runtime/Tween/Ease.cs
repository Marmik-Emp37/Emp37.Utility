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
                  InBounce,
                  OutBounce,
                  InOutBounce,
                  InBack,
                  OutBack,
                  InOutBack,
                  InElastic,
                  OutElastic,
                  InOutElastic,
                  BreakOutBounce,
            }


            #region E A S I N G   F U N C T I O N S
            private static float Linear(float progress)
            {
                  return progress;
            }

            // S I N E
            private static float EaseInSine(float progress)
            {
                  return 1F - Cos(progress * PI * 0.5F);
            }
            private static float EaseOutSine(float progress)
            {
                  return Sin(progress * PI * 0.5F);
            }
            private static float EaseInOutSine(float progress)
            {
                  return -0.5F * (Cos(progress * PI) - 1F);
            }

            // C U B I C
            private static float EaseInCubic(float progress)
            {
                  return progress * progress * progress;
            }
            private static float EaseOutCubic(float progress)
            {
                  progress--;
                  return progress * progress * progress + 1F;
            }
            private static float EaseInOutCubic(float progress)
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
            private static float EaseInQuint(float progress)
            {
                  return progress * progress * progress * progress * progress;
            }
            private static float EaseOutQuint(float progress)
            {
                  progress = 1F - progress;
                  return 1F - progress * progress * progress * progress * progress;
            }
            private static float EaseInOutQuint(float progress)
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
            private static float EaseInCirc(float progress)
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
            private static float EaseOutCirc(float progress)
            {
                  progress--;
                  return Sqrt(1F - progress * progress);
            }
            private static float EaseInOutCirc(float progress)
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
            private static float EaseInQuad(float progress)
            {
                  return progress * progress;
            }
            private static float EaseOutQuad(float progress)
            {
                  progress = 1F - progress;
                  return 1F - progress * progress;
            }
            private static float EaseInOutQuad(float progress)
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
            private static float EaseInQuart(float progress)
            {
                  return progress * progress * progress * progress;
            }
            private static float EaseOutQuart(float progress)
            {
                  progress--;
                  return -(progress * progress * progress * progress - 1F);
            }
            private static float EaseInOutQuart(float progress)
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
            private static float EaseInExpo(float progress)
            {
                  return Pow(2F, 10F * progress - 10F);
            }
            private static float EaseOutExpo(float progress)
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
            private static float EaseInOutExpo(float progress)
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
            private const float BackSpringConstant = 1.70158F;

            private static float EaseInBack(float progress, float overshoot)
            {
                  float spring = BackSpringConstant * overshoot;

                  return progress * progress * ((spring + 1F) * progress - spring);
            }
            private static float EaseOutBack(float progress, float overshoot)
            {
                  float spring = BackSpringConstant * overshoot;

                  progress--;
                  return progress * progress * ((spring + 1F) * progress + spring) + 1F;
            }
            private static float EaseInOutBack(float progress, float overshoot)
            {
                  const float midpointTension = 1.525F;
                  float spring = BackSpringConstant * overshoot * midpointTension;

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
            private static float EaseInBounce(float progress)
            {
                  return 1F - EaseOutBounce(1F - progress);
            }
            private static float EaseOutBounce(float progress)
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
            private static float EaseInOutBounce(float progress)
            {
                  if (progress < 0.5F)
                  {
                        return (1F - EaseOutBounce(1F - (progress * 2F))) * 0.5F;
                  }
                  else
                  {
                        return (1F + EaseOutBounce((2F * progress) - 1F)) * 0.5F;
                  }
            }

            // E L A S T I C
            private static float EaseInElastic(float progress, float overshoot, float period)
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
            private static float EaseOutElastic(float progress, float overshoot, float period)
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
            private static float EaseInOutElastic(float progress, float overshoot, float period)
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

            // C U S T O M
            private static float BreakOutBounce(float progress)
            {
                  return (progress < 0.5F ? 1F - EaseInBounce(1F - (2F * progress)) : 1F + EaseOutBounce((2F * progress) - 1F)) * 0.5F;
            }
            #endregion

            /// <summary>
            /// Simulates transition of a value on a non-linear path.
            /// </summary>
            /// <param name="type">Type of curve to be simulated.</param>
            /// <param name="value">Normalized point on a linear path.</param>
            /// <returns>Corresponding point on a selected type path.</returns>
            public static float EasedRatio(float value, Type type, float overshoot = 1F, float period = 0.4F) => type switch
            {
                  Type.Linear => Linear(value),
                  Type.InSine => EaseInSine(value),
                  Type.OutSine => EaseOutSine(value),
                  Type.InOutSine => EaseInOutSine(value),
                  Type.InCubic => EaseInCubic(value),
                  Type.OutCubic => EaseOutCubic(value),
                  Type.InOutCubic => EaseInOutCubic(value),
                  Type.InQuint => EaseInQuint(value),
                  Type.OutQuint => EaseOutQuint(value),
                  Type.InOutQuint => EaseInOutQuint(value),
                  Type.InCirc => EaseInCirc(value),
                  Type.OutCirc => EaseOutCirc(value),
                  Type.InOutCirc => EaseInOutCirc(value),
                  Type.InQuad => EaseInQuad(value),
                  Type.OutQuad => EaseOutQuad(value),
                  Type.InOutQuad => EaseInOutQuad(value),
                  Type.InQuart => EaseInQuart(value),
                  Type.OutQuart => EaseOutQuart(value),
                  Type.InOutQuart => EaseInOutQuart(value),
                  Type.InExpo => EaseInExpo(value),
                  Type.OutExpo => EaseOutExpo(value),
                  Type.InOutExpo => EaseInOutExpo(value),
                  Type.InBack => EaseInBack(value, overshoot),
                  Type.OutBack => EaseOutBack(value, overshoot),
                  Type.InOutBack => EaseInOutBack(value, overshoot),
                  Type.InElastic => EaseInElastic(value, overshoot, period),
                  Type.OutElastic => EaseOutElastic(value, overshoot, period),
                  Type.InOutElastic => EaseInOutElastic(value, overshoot, period),
                  Type.InBounce => EaseInBounce(value),
                  Type.OutBounce => EaseOutBounce(value),
                  Type.InOutBounce => EaseInOutBounce(value),
                  Type.BreakOutBounce => BreakOutBounce(value),
                  _ => 1F
            };
      }
}