using MyTimer;
using UnityEngine;

public class ShadowTransformation : EaseTransformation
{
    private readonly ShadowInfo shadowInfo;

    public ShadowTransformation(Transform transform) : base(transform) { }

    public ShadowTransformation(Transform transform, ShadowInfo shadowInfo)
        : base(transform)
    {
        this.shadowInfo = shadowInfo;
    }

    public Vector3 InitializeByMoveVec(Vector3 moveVec, float moveSpeed)
    {
        base.InitializeByMoveVec(moveVec, moveVec.magnitude / moveSpeed);
        return Target - Origin;
    }

    public Vector3 InitializeByMoveVec(Vector3 moveVec) => InitializeByMoveVec(moveVec, shadowInfo.flyingSpeed);

    public void ExpandTarget(float increaseDist)
    {
        Vector3 aimTarget = Vector3.MoveTowards(Target, Origin, -increaseDist);
        InitializeByTarget(aimTarget, Vector3.Distance(transform.position, aimTarget) / shadowInfo.flyingSpeed);
    }
}