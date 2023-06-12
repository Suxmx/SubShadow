using UnityEngine;
using UnityEngine.Events;

namespace MyTimer
{
    //Timer类用于代替部分协程，适合代替规律性强的、需要反复访问的或需要反复启动关闭的协程

    /// <summary>
    /// 描述一段随时间的变化
    /// </summary>
    /// <typeparam name="TValue">变化过程中的返回值类型</typeparam>
    /// <typeparam name="TLerp">计算返回值的方法</typeparam>
    [System.Serializable]
    public class Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        private readonly bool unscaled;
        private readonly GameCycle gameCycle;

        [SerializeField]
        protected bool paused;
        /// <summary>
        /// 是否暂停，弃用Timer前，一定要确保其Paused==true
        /// </summary>
        public bool Paused
        {
            get => paused;
            set
            {
                if (paused != value)
                {
                    paused = value;
                    if (value)
                    {
                        gameCycle.RemoveFromGameCycle(EInvokeMode.Update, Update);
                        OnPause?.Invoke(Current);
                    }
                    else
                    {
                        gameCycle.AttachToGameCycle(EInvokeMode.Update, Update);
                        OnResume?.Invoke(Current);
                    }
                }
            }
        }

        [SerializeField]
        protected bool completed;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Completed
        {
            get => completed;
            protected set
            {
                if (completed != value)
                {
                    completed = value;
                    if (value)
                    {
                        OnComplete?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// 经过的时间
        /// </summary>
        public float Time { get; protected set; }
        /// <summary>
        /// 到达的百分比（0～1)
        /// </summary>
        public float Percent => Mathf.Clamp01(Time / Duration);
        /// <summary>
        /// 总时间
        /// </summary>
        public float Duration { get; protected set; }
        /// <summary>
        /// 初值
        /// </summary>
        public TValue Origin { get; protected set; }
        /// <summary>
        /// 终值
        /// </summary>
        public TValue Target { get; protected set; }

        public ILerp<TValue> Lerp { get; protected set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public TValue Current => Lerp.Value(Origin, Target, Percent, Time, Duration);

        /// <summary>
        /// 暂停时触发
        /// </summary>
        public event UnityAction<TValue> OnPause;
        /// <summary>
        /// 启动/解除暂停时触发
        /// </summary>
        public event UnityAction<TValue> OnResume;
        /// <summary>
        /// 到时间时触发
        /// </summary>
        public event UnityAction OnComplete;
        /// <summary>
        /// 运行时每帧触发
        /// </summary>
        public event UnityAction<TValue> OnTick;

        public Timer(bool unscaled = false)
        {
            Lerp = new TLerp();
            this.unscaled = unscaled;
            gameCycle = GameCycle.Instance;
            paused = true;
        }

        public Timer(Timer<TValue, TLerp> from) : this()
        {
            CopyFrom(from);
        }

        /// <summary>
        /// 为MyTimer设置初始属性及是否立刻启动
        /// </summary>
        public virtual void Initialize(TValue origin, TValue target, float duration, bool start = true)
        {
            Duration = duration;
            Origin = origin;
            Target = target;
            if (start)
                Restart();
        }

        protected void Update()
        {
            if (unscaled) Time += UnityEngine.Time.unscaledDeltaTime;
            else Time += UnityEngine.Time.deltaTime;
            OnTick?.Invoke(Current);
            if (Time >= Duration)
            {
                Paused = true;
                Completed = true;
            }
        }

        /// <param name="fixedTime">设为true可避免累积误差</param>
        public void Restart(bool fixedTime = false)
        {
            if (fixedTime)
                Time -= Duration;
            else
                Time = 0;
            Paused = false;
            Completed = false;
        }

        /// <summary>
        /// 让计时器归零并暂停
        /// </summary>
        public void ReturnToZero()
        {
            Time = 0;
            Paused = true;
            Completed = false;
        }

        /// <summary>
        /// 使计时器立刻到时间
        /// </summary>
        public void ForceComplete()
        {
            Time = Duration;
            OnTick?.Invoke(Current);
            Paused = true;
            Completed = true;
        }

        /// <summary>
        /// 从另一个计时器复制
        /// 可能会意外地触发委托，要注意
        /// </summary>
        /// <param name="from"></param>
        public void CopyFrom(Timer<TValue, TLerp> from)
        {
            Paused = from.Paused;
            Completed = from.Completed;
            Time = from.Time;
            Duration = from.Duration;
            Origin = from.Origin;
            Target = from.Target;
        }

        public void ResetEvents()
        {
            OnPause = null;
            OnResume = null;
            OnTick = null;
            OnComplete = null;
        }

        public override string ToString()
        {
            return $"Paused:{Paused},Completed:{Completed},Origin:{Origin},Target:{Target},Duration:{Duration}";
        }
    }
}