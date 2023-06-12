using MyTimer;
using UnityEngine;

public class EnemyBeatenTimer : EaseTransformationForRb2D
{
    public float beatenSpeed;
    private bool beatenFromHit;

    public Vector3 BeatenVec => Target - Current;

    public EnemyBeatenTimer(Rigidbody2D rb) : base(rb, EaseType.OutCubic) { }

    public void Initialize(BeatInfo beatInfo)
    {
        if (!Paused && beatInfo.isHitBeat && !beatenFromHit) return;
        beatenSpeed = beatInfo.beatSpeed;
        beatenFromHit = beatInfo.isHitBeat;
        base.InitializeByMoveVec(beatInfo.beatVec, beatInfo.beatDist / beatenSpeed, true);
    }
}