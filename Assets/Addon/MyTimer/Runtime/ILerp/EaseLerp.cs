using UnityEngine;

namespace MyTimer
{
    public enum EaseType
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,
    }

    public static class EaseLerpUtility
    {
        private static readonly float c1 = 1.70158f;
        // c2 = c1 * 1.525
        private static readonly float c2 = 2.59491f;
        // c3 = c1 + 1
        private static readonly float c3 = 2.70158f;
        private static readonly float c4 = 2f * Mathf.PI / 3f;
        private static readonly float c5 = 2f * Mathf.PI / 4.5f;
        private static readonly float n1 = 7.5625f;
        private static readonly float d1 = 2.75f;

        /// <summary>
        /// 根据当前时间百分比获取缓动函数百分比
        /// </summary>
        /// <param name="x">时间百分比</param>
        /// <returns>缓动函数百分比</returns>
        public static float GetPercent(float x, EaseType easeType)
        {
            return easeType switch
            {
                EaseType.Linear => x,
                EaseType.InSine => 1 - Mathf.Cos((x * Mathf.PI) / 2),
                EaseType.OutSine => Mathf.Sin((x * Mathf.PI) / 2),
                EaseType.InOutSine => -(Mathf.Cos(Mathf.PI * x) - 1) / 2,
                EaseType.InQuad => x * x,
                EaseType.OutQuad => 1 - (1 - x) * (1 - x),
                EaseType.InOutQuad => x < 0.5f ? 2 * x * x :
                    1 - Mathf.Pow(-2 * x + 2, 2) / 2,
                EaseType.InCubic => x * x * x,
                EaseType.OutCubic => 1 - Mathf.Pow(1 - x, 3),
                EaseType.InOutCubic => x < 0.5f ? 4 * x * x * x :
                    1 - Mathf.Pow(-2 * x + 2, 3) / 2,
                EaseType.InQuart => x * x * x * x,
                EaseType.OutQuart => 1 - Mathf.Pow(1 - x, 4),
                EaseType.InOutQuart => x < 0.5f ? 8 * x * x * x * x :
                    1 - Mathf.Pow(-2 * x + 2, 4) / 2,
                EaseType.InQuint => x * x * x * x * x,
                EaseType.OutQuint => 1 - Mathf.Pow(1 - x, 5),
                EaseType.InOutQuint => x < 0.5f ? 16 * x * x * x * x * x :
                    1 - Mathf.Pow(-2 * x + 2, 5) / 2,
                EaseType.InExpo => x == 0f ? 0 : Mathf.Pow(2, 10 * x - 10),
                EaseType.OutExpo => x == 1f ? 1 : 1 - Mathf.Pow(2, -10 * x),
                EaseType.InOutExpo => x == 0f ? 0 : x == 1f ? 1 : x < 0.5f ?
                    Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2,
                EaseType.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)),
                EaseType.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)),
                EaseType.InOutCirc => x < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 :
                    (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2,
                EaseType.InBack => c3 * x * x * x - c1 * x * x,
                EaseType.OutBack => 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2),
                EaseType.InOutBack => x < 0.5f ? (Mathf.Pow(2 * x, 2) * (c2 + 1) * 2 * x - c2) / 2 : 
                    (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2,
                EaseType.InElastic => x == 0 ? 0 : x == 1 ? 1 : 
                    -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4),
                EaseType.OutElastic => x == 0 ? 0 : x == 1 ? 1 : 
                    Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1,
                EaseType.InOutElastic => x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? 
                    -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 : 
                    Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5) / 2 + 1,
                EaseType.InBounce => 1 - EaseOutBounce(1 - x),
                EaseType.OutBounce => EaseOutBounce(x),
                EaseType.InOutBounce => x < 0.5f ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : 
                    (1 + EaseOutBounce(2 * x - 1)) / 2,
                _ => 1,
            };
        }

        private static float EaseOutBounce(float x)
        {
            if (x < 1 / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2 / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            else if (x < 2.5 / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            else
            {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
    }

    public class EaseFloatLerp : ILerp<float>
    {
        public EaseType easeType = EaseType.Linear;

        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return Mathf.Lerp(origin, target, EaseLerpUtility.GetPercent(percent, easeType));
        }
    }

    public class EaseVector2Lerp : ILerp<Vector2>
    {
        public EaseType easeType = EaseType.Linear;

        public Vector2 Value(Vector2 origin, Vector2 target, float percent, float time, float duration)
        {
            return Vector2.Lerp(origin, target, EaseLerpUtility.GetPercent(percent, easeType));
        }
    }

    public class EaseVector3Lerp : ILerp<Vector3>
    {
        public EaseType easeType = EaseType.Linear;

        public Vector3 Value(Vector3 origin, Vector3 target, float percent, float time, float duration)
        {
            return Vector3.Lerp(origin, target, EaseLerpUtility.GetPercent(percent, easeType));
        }
    }
}