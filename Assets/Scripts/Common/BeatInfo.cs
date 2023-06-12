using UnityEngine;

public class BeatInfo
{
    /// <summary>
    /// 击退的距离
    /// </summary>
    public float beatDist;

    /// <summary>
    /// 击退的向量（长度为BeatDist）
    /// </summary>
    public Vector3 beatVec;

    /// <summary>
    /// 击退的平均速度
    /// </summary>
    public float beatSpeed;

    /// <summary>
    /// 是否取消碰撞体
    /// </summary>
    public bool disenableCollider;

    /// <summary>
    /// 是否是伤害附带的击退效果
    /// </summary>
    public bool isHitBeat;

    /// <summary>
    /// 使用这个构造函数需要后续设置beatVec
    /// </summary>
    public BeatInfo(float beatDist, float beatSpeed, bool disenableCollider, 
        bool isHitBeat = false)
    {
        this.beatDist = beatDist;
        this.beatSpeed = beatSpeed;
        this.disenableCollider = disenableCollider;
        this.isHitBeat = isHitBeat;
    }

    public BeatInfo(Vector3 beatVec, float beatSpeed, bool disenableCollider, 
        bool isHitBeat = false) 
        : this(beatVec.magnitude, beatSpeed, disenableCollider, isHitBeat)
    {
        this.beatVec = beatVec;
    }
}
