using UnityEngine;

public class BeatInfo
{
    /// <summary>
    /// ���˵ľ���
    /// </summary>
    public float beatDist;

    /// <summary>
    /// ���˵�����������ΪBeatDist��
    /// </summary>
    public Vector3 beatVec;

    /// <summary>
    /// ���˵�ƽ���ٶ�
    /// </summary>
    public float beatSpeed;

    /// <summary>
    /// �Ƿ�ȡ����ײ��
    /// </summary>
    public bool disenableCollider;

    /// <summary>
    /// �Ƿ����˺������Ļ���Ч��
    /// </summary>
    public bool isHitBeat;

    /// <summary>
    /// ʹ��������캯����Ҫ��������beatVec
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
