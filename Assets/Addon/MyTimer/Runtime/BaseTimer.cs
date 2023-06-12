using UnityEngine.Events;
using static UnityEngine.UI.Image;

namespace MyTimer
{
    /// <summary>
    /// 基本的往复变化
    /// </summary>
    public class Circulation<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Circulation()
        {
            OnComplete += MyOnComplete;
        }

        private void MyOnComplete()
        {
            (Target, Origin) = (Origin, Target);
            Restart(true);
        }
    }

    /// <summary>
    /// 基本的反复变化
    /// </summary>
    public class Repeation<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Repeation()
        {
            OnComplete += MyOnComplete;
        }

        private void MyOnComplete()
        {
            Restart(true);
        }
    }

    /// <summary>
    /// 固定时长的周期计时器
    /// </summary>
    public class FixedDurationRepeation : Timer<float, CurrentTime>
    {
        private float judgeInterval;
        private float nextJudgeTime;

        public event UnityAction OnJudge;

        public FixedDurationRepeation()
        {
            Origin = 0;
            OnTick += MyOnTick;
        }

        private void MyOnTick(float current)
        {
            if (current >= nextJudgeTime)
            {
                OnJudge?.Invoke();
                nextJudgeTime += judgeInterval;
            }
        }

        /// <summary>
        /// 刷新持续时间的初始化
        /// </summary>
        public virtual void Initialize(float duration, float judgeInterval)
        {
            this.judgeInterval = judgeInterval;
            if (Paused)
            {
                nextJudgeTime = judgeInterval;
                Target = Duration = duration;
                Restart();
            }
            else
            {
                Target = Duration = Current + duration;
            }
        }
    }

    /// <summary>
    /// 不使用值，仅周期性调用方法
    /// </summary>
    public class Metronome : Repeation<float, DefaultValue<float>>
    {
        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, 0f, duration, start);
        }
    }

    /// <summary>
    /// 周期计时器
    /// </summary>
    public class RepeatTimer : Repeation<float, CurrentTime>
    {
        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }

    /// <summary>
    /// 仅计时
    /// </summary>
    public class TimerOnly : Timer<float, CurrentTime>
    {
        public TimerOnly(bool infinity = false)
        {
            if (infinity) Initialize(float.MaxValue, false);
        }

        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }

    /// <summary>
    /// 仅倒计时
    /// </summary>
    public class CountdownTimer : Timer<float, LeftTime>
    {
        public CountdownTimer(bool unscaled = false) : base(unscaled) { }

        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(duration, 0f, duration, start);
        }
    }

    /// <summary>
    /// 利用百分比的计时器
    /// </summary>
    public class PercentTimer : Timer<float, CurrentPercent>
    {
        public PercentTimer(bool unscaled = false) : base(unscaled) { }

        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }

    /// <summary>
    /// 倒计时百分比计时器
    /// </summary>
    public class CountdownPercentTimer : Timer<float, LeftPercent>
    {
        public CountdownPercentTimer(bool unscaled = false) : base(unscaled) { }

        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(duration, 0f, duration, start);
        }
    }
}