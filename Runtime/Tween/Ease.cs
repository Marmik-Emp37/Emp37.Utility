using NUnit;

using static System.MathF;

namespace Emp37.Utility.Tweening
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
                  InElastic,
                  OutElastic,
                  InOutElastic,
            }

            #region U S I N G   P E N N E R ' S   E A S I N G   N O T A T I O N S
            // Linear
            public static float Linear(float t) => t;

            // Sine
            public static float InSine(float t) => 1F - Cos(t * PI * 0.5F);
            public static float OutSine(float t) => Sin(t * PI * 0.5F);
            public static float InOutSine(float t) => -0.5F * (Cos(PI * t) - 1F);

            // Cubic
            public static float InCubic(float t) => t * t * t;
            public static float OutCubic(float t) => (--t * t * t) + 1F;
            public static float InOutCubic(float t) => t < 0.5F ? (4F * t * t * t) : 1F - (Pow((-2F * t) + 2F, 3F) / 2F);

            // Quint
            public static float InQuint(float t) => t * t * t * t * t;
            public static float OutQuint(float t) => 1F - Pow(1F - t, 5F);
            public static float InOutQuint(float t) => t < 0.5F ? (16F * t * t * t * t * t) : 1F - (Pow((-2F * t) + 2F, 5F) / 2F);

            // Circ
            public static float InCirc(float t) => 1F - Sqrt(1F - (t * t));
            public static float OutCirc(float t) => Sqrt(1F - Pow(t - 1F, 2F));
            public static float InOutCirc(float t) => t < 0.5F ? (1F - Sqrt(1F - (4F * t * t))) / 2F : (Sqrt(1F - Pow((-2F * t) + 2F, 2F)) + 1F) / 2F;

            // Quad
            public static float InQuad(float t) => t * t;
            public static float OutQuad(float t) => 1F - (1F - t) * (1F - t);
            public static float InOutQuad(float t) => t < 0.5F ? (2F * t * t) : 1F - (Pow((-2F * t) + 2F, 2F) / 2F);

            // Quart
            public static float InQuart(float t) => t * t * t * t;
            public static float OutQuart(float t) => 1F - Pow(1F - t, 4F);
            public static float InOutQuart(float t) => t < 0.5F ? (8F * t * t * t * t) : 1F - (Pow((-2F * t) + 2F, 4F) / 2F);

            // Expo
            public static float InExpo(float t) => t == 0F ? 0F : Pow(2F, (10F * t) - 10F);
            public static float OutExpo(float t) => t == 1F ? 1F : 1F - Pow(2F, (-10F * t));
            public static float InOutExpo(float t) => t == 0F ? 0F : t == 1F ? 1F : t < 0.5F ? Pow(2F, (20F * t) - 10F) / 2F : (2F - Pow(2F, (-20F * t) + 10F)) / 2F;

            // Back
            private const float s = 1.70158F;
            public static float InBack(float t, float overshoot = 1F)
            {
                  float a = s * overshoot;
                  return t * t * (((a + 1F) * t) - a);
            }
            public static float OutBack(float t, float overshoot = 1F)
            {
                  float a = s * overshoot;
                  return (--t) * t * (((a + 1F) * t) + a) + 1F;
            }
            public static float InOutBack(float t, float overshoot = 1F)
            {
                  float a = s * overshoot * 1.525F;
                  return t < 0.5F ? Pow((2F * t), 2F) * (((a + 1F) * 2F * t) - a) / 2F : (Pow((2F * t) - 2F, 2F) * ((a + 1F) * ((2F * t) - 2F) + a) + 2F) / 2F;
            }

            // Bounce
            public static float InBounce(float t) => 1F - OutBounce(1F - t);
            public static float OutBounce(float t)
            {
                  const float n1 = 7.5625F, d1 = 2.75F;
                  if (t < 1F / d1) return n1 * t * t;
                  if (t < 2F / d1) return (n1 * (t -= 1.5F / d1) * t) + 0.75F;
                  if (t < 2.5F / d1) return (n1 * (t -= 2.25F / d1) * t) + 0.9375F;
                  return (n1 * (t -= 2.625F / d1) * t) + 0.984375F;
            }
            public static float InOutBounce(float t) => t < 0.5F ? (1F - OutBounce(1F - (2F * t))) / 2F : (1F + OutBounce((2F * t) - 1F)) / 2F;

            // Elastic
            public static float InElastic(float t, float overshoot = 1F, float period = 0.3F)
            {
                  if (t == 0F || t == 1F) return t;
                  float s = period / (2F * PI) * Asin(1F / overshoot);
                  return -(overshoot * Pow(2F, 10F * (t - 1F)) * Sin((t - 1F - s) * (2F * PI) / period));
            }
            public static float OutElastic(float t, float overshoot = 1F, float period = 0.3F)
            {
                  if (t == 0F || t == 1F) return t;
                  float s = period / (2F * PI) * Asin(1F / overshoot);
                  return overshoot * Pow(2F, -10F * t) * Sin((t - s) * (2F * PI) / period) + 1F;
            }
            public static float InOutElastic(float t, float overshoot = 1F, float period = 0.45F)
            {
                  if (t == 0F) return 0F; if (t == 1F) return 1F;
                  float s = period / 4F;
                  t *= 2F;
                  if (overshoot > 1F) overshoot = t < 0.2F ? 1F + (t / 0.2F * (overshoot - 1F)) : t > 0.8F ? 1F + ((1F - t) / 0.2F * (overshoot - 1F)) : overshoot;
                  return t-- < 1F ? -0.5F * (Pow(2F, 10F * t) * Sin((t - s) * (2F * PI) / period)) * overshoot : 1F + (Pow(2F, -10F * t) * Sin((t - s) * (2F * PI) / period) * 0.5F * overshoot);
            }
            #endregion
      }
}