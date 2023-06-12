using MyTimer;
using UnityEngine;

public class UIScaleExpander : EaseFloatTimer
{
    private readonly Transform transform;
    private readonly float shrinkScale;
    private readonly float expandScale;
    private readonly float changeTime;

    public UIScaleExpander(Transform transform, float shrinkScale = 1f, float expandScale = 1.2f, 
        float changeTime = 0.4f, EaseType easeType = EaseType.OutCubic) : base(true, easeType)
    {
        this.transform = transform;
        this.shrinkScale = shrinkScale;
        this.expandScale = expandScale;
        this.changeTime = changeTime;
        OnTick += SetScale;
    }

    public void SetScale(float current)
    {
        transform.localScale = new Vector3(current, current, 1f);
    }

    public void ExpandScale()
    {
        Initialize(transform.localScale.x, expandScale, changeTime);
    }

    public void ShrinkScale()
    {
        Initialize(transform.localScale.x, shrinkScale, changeTime);
    }

    public void ResetScale()
    {
        Paused = true;
        SetScale(shrinkScale);
    }
}
