using UnityEngine;

namespace MyTimer
{
    public class EaseFloatTimer : Timer<float, EaseFloatLerp>
    {
        public EaseFloatTimer(bool unscaled, EaseType easeType = EaseType.Linear) : base(unscaled)
        {
            SetEaseType(easeType);
        }

        public EaseFloatTimer(EaseType easeType = EaseType.Linear) : this(false, easeType) { }

        public void SetEaseType(EaseType easeType)
        {
            ((EaseFloatLerp)Lerp).easeType = easeType;
        }
    }

    public class EaseVector2Timer : Timer<Vector2, EaseVector2Lerp>
    {
        public EaseVector2Timer(bool unscaled, EaseType easeType = EaseType.Linear) : base(unscaled)
        {
            SetEaseType(easeType);
        }

        public EaseVector2Timer(EaseType easeType = EaseType.Linear) : this(false, easeType) { }

        public void SetEaseType(EaseType easeType)
        {
            ((EaseVector2Lerp)Lerp).easeType = easeType;
        }
    }

    public class EaseVector3Timer : Timer<Vector3, EaseVector3Lerp>
    {
        public EaseVector3Timer(bool unscaled, EaseType easeType = EaseType.Linear) : base(unscaled)
        {
            SetEaseType(easeType);
        }

        public EaseVector3Timer(EaseType easeType = EaseType.Linear) : this(false, easeType) { }

        public void SetEaseType(EaseType easeType)
        {
            ((EaseVector3Lerp)Lerp).easeType = easeType;
        }
    }

    /// <summary>
    /// 用于缓动移动至目标点，期间其他对Transform.position的设置无效
    /// </summary>
    public class EaseTransformation : EaseVector3Timer
    {
        protected Transform transform;

        public EaseTransformation(Transform transform, bool unscaled, EaseType easeType = EaseType.Linear) 
            : base(unscaled, easeType)
        {
            this.transform = transform;
            OnTick += SetTransformPosition;
        }

        public EaseTransformation(Transform transform, EaseType easeType = EaseType.Linear) 
            : this(transform, false, easeType) { }

        public virtual void InitializeByTarget(Vector3 target, float duration, bool start = true)
        {
            Initialize(transform.position, target, duration, start);
        }

        public virtual void InitializeByMoveVec(Vector3 moveVec, float duration, bool start = true)
        {
            Initialize(transform.position, transform.position + moveVec, duration, start);
        }

        private void SetTransformPosition(Vector3 current)
        {
            transform.position = current;
        }
    }

    /// <summary>
    /// 用于使2D刚体缓动移动至目标点
    /// </summary>
    public class EaseTransformationForRb2D : EaseVector2Timer
    {
        protected Rigidbody2D rb;

        public EaseTransformationForRb2D(Rigidbody2D rb, EaseType easeType = EaseType.Linear) : 
            base(easeType)
        {
            this.rb = rb;
            OnTick += MovePosition;
        }

        public virtual void InitializeByTarget(Vector2 target, float duration, bool start = true)
        {
            rb.velocity = Vector2.zero;
            Initialize(rb.position, target, duration, start);
        }

        public virtual void InitializeByMoveVec(Vector2 moveVec, float duration, bool start = true)
        {
            rb.velocity = Vector2.zero;
            Initialize(rb.position, rb.position + moveVec, duration, start);
        }

        private void MovePosition(Vector2 current)
        {
            rb.MovePosition(current);
        }
    }
}