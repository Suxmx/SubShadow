using System.Collections;
using UnityEngine;

public class EXPParticle : Prop
{
    [SerializeField]
    int upFrames = 10,
        downFrames = 10;

    [SerializeField]
    float upa = 1,
        downa = 2;

    [SerializeField]
    Color generateColor,
        normalColor;
    private SpriteRenderer sr;
    private bool genAniEnd = false;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void ChangeValue()
    {
        playerStatusInfo.ChangeExp(1);
    }

    public void Init(Vector2 endPos)
    {
        Collectable = true;
        genAniEnd = false;
        trail.Clear();
        trail.time=0.15f;
        sr.color = generateColor;
        StartCoroutine(GenerateAnimation(endPos));
    }

    public override void FollowAndRecycle(Transform targetTrans)
    {
        Collectable = false;
        StartCoroutine(WaitGenAniEnd(targetTrans));
    }

    private IEnumerator WaitGenAniEnd(Transform targetTrans)//防止经验值一生成就在拾取范围内而无法拾取
    {
        while (!genAniEnd)
            yield return new WaitForFixedUpdate();
        trail.Clear();
        StartCoroutine(Follow(targetTrans));
    }

    private IEnumerator GenerateAnimation(Vector2 endPos)//生成动画
    {
        //-----动画部分-----
        Vector2 xSpeed = (endPos - (Vector2)transform.position) / (upFrames + downFrames);
        float initVelocity = upFrames * 0.02f * upa;
        for (int i = 1; i <= upFrames; i++)
        {
            transform.Translate(xSpeed);
            transform.Translate(Vector2.up * (initVelocity - i * upa * 0.02f));
            yield return new WaitForFixedUpdate();
        }
        for (int i = 1; i <= downFrames; i++)
        {
            transform.Translate(xSpeed);
            transform.Translate(Vector2.down * downa * i * 0.02f);
            yield return new WaitForFixedUpdate();
        }
        sr.color = normalColor;
        //-----动画部分-----
        genAniEnd = true;
    }
}
