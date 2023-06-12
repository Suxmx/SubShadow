namespace MyTimer
{
    public class CurrentTime : ILerp<float>
    {
        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return time;
        }
    }

    public class LeftTime : ILerp<float>
    {
        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return duration - time;
        }
    }

    public class CurrentPercent : ILerp<float>
    {
        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return percent;
        }
    }

    public class LeftPercent : ILerp<float>
    {
        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return 1 - percent;
        }
    }
}