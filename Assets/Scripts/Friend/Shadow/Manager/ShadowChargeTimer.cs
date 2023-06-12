using MyTimer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShadowChargeTimer : Repeation<float, CurrentPercent>
{
    public event UnityAction<int, int> OnShadowMaxChargedCountChange;
    public event UnityAction<int, int> OnShadowChargedCountChange;
    public bool completeImmediatelyNextTime;

    private readonly List<Shadow> shadowList;
    private readonly ShadowPreChargeTimer prechargeTimer;

    public float ShadowChargeCD { get => Duration; set => Duration = value; }

    public bool IsChargeCountMax => ShadowChargedCount + shadowList.Count >= MaxShadowChargeCount;

    private int maxShadowChargeCount;
    public int MaxShadowChargeCount
    {
        get => maxShadowChargeCount;
        set
        {
            OnShadowMaxChargedCountChange?.Invoke(maxShadowChargeCount, value);
            maxShadowChargeCount = value;
            if (IsChargeCountMax) ShadowChargedCount = maxShadowChargeCount - shadowList.Count;
            else
            {
                Paused = false;
                prechargeTimer.ReturnToZero();
            }
        }
    }

    private int shadowChargedCount;
    public int ShadowChargedCount
    {
        get => shadowChargedCount;
        set
        {
            int previousCount = shadowChargedCount;
            shadowChargedCount = value;
            if (IsChargeCountMax)
            {
                shadowChargedCount = Mathf.Max(MaxShadowChargeCount - shadowList.Count, 0);
                if (prechargeTimer.Paused) Paused = true;
            }
            else
            {
                Paused = false;
                prechargeTimer.ReturnToZero();
            }
            OnShadowChargedCountChange?.Invoke(previousCount, shadowChargedCount);
        }
    }

    public ShadowChargeTimer(List<Shadow> shadowList)
    {
        Origin = 0f;
        Target = 1f;
        this.shadowList = shadowList;
        prechargeTimer = new ShadowPreChargeTimer();
        prechargeTimer.OnTick += PrechargeOnTick;
        //prechargeTimer.OnCompletePrecharge += () => Paused = true;
        prechargeTimer.OnComplete += () => Paused = true;
        OnComplete += () => ShadowChargedCount++;
        shadowChargedCount = maxShadowChargeCount = 0;
    }

    public void Initialize(SkillInfoData skillInfoData)
    {
        completeImmediatelyNextTime = false;
        prechargeTimer.Initialize(skillInfoData.maxPrecharge_Gather, skillInfoData.prechargeDuration_Gather);
        ShadowChargeCD = skillInfoData.shadowChargeCD;
        MaxShadowChargeCount = skillInfoData.shadowChargeCount;
        ShadowChargedCount = MaxShadowChargeCount;
    }

    public void UpdateAfterRecallShadow()
    {
        if (!IsChargeCountMax)
        {
            Paused = false;
            prechargeTimer.ReturnToZero();
        }
    }

    public void AddProgress(float addProgress)
    {
        if (!Paused) Time += addProgress * Duration;
    }

    public void CompleteImmediately(bool nextTime)
    {
        if (completeImmediatelyNextTime) return;
        if (Paused)
        {
            if (nextTime)
            {
                OnResume += MyOnResume;
                completeImmediatelyNextTime = true;
            }
        }
        else
        {
            ForceComplete();
        }
    }

    private void MyOnResume(float _)
    {
        OnResume -= MyOnResume;
        if (completeImmediatelyNextTime)
        {
            completeImmediatelyNextTime = false;
            ForceComplete();
        }
    }

    public void SetShadowPrecharge(bool on)
    {
        if (on)
        {
            OnShadowChargedCountChange += StartPrechargeShadow;
        }
        else
        {
            OnShadowChargedCountChange -= StartPrechargeShadow;
            prechargeTimer.ReturnToZero();
        }
    }

    private void StartPrechargeShadow(int before, int after)
    {
        if (Paused && ShadowChargedCount < MaxShadowChargeCount 
            && prechargeTimer.Paused && !prechargeTimer.Completed)
        {
            Paused = false;
            prechargeTimer.Restart();
        }
    }

    private void PrechargeOnTick(float current)
    {
        Time = current * Duration;
    }
    
    public void IncreaseChargeEfficiency(float increaseDuration, float increaseEfficiency)
    {
        CountdownTimer increaseChargeTimer = new CountdownTimer();
        increaseChargeTimer.OnTick += _ => 
        {
            if (!Paused) Time += increaseEfficiency * UnityEngine.Time.deltaTime;
        };
        increaseChargeTimer.Initialize(increaseDuration);
    }
}

public class ShadowPreChargeTimer : PercentTimer
{
    private float prechargePercent;
    //public event UnityAction OnCompletePrecharge;

    public ShadowPreChargeTimer()
    {
        OnTick += CheckPreChargeEnd;
    }

    public void Initialize(float prechargePercent, float prechargeDuration)
    {
        this.prechargePercent = prechargePercent;
        Initialize(prechargeDuration, false);
    }

    private void CheckPreChargeEnd(float current)
    {
        if (current >= prechargePercent)
        {
            Paused = true;
            Completed = true;
            //OnCompletePrecharge?.Invoke();
        }
    }
}
