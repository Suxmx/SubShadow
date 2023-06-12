using MyTimer;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowWall : MonoBehaviour
{
    private MyObject myObject;
    private Rigidbody2D rb;
    private ShadowWallDamager damageTrigger;
    private List<BoxCollider2D> cds;
    private List<Animator> animators;
    private Transform playerTransform;
    private ShadowManager shadowManager;
    private ShadowWallInfo shadowWallInfo;

    [Label("中心位置")]
    public Transform centerTransform;
    private Vector3 centerOffset;
    private float defaultSizeY;
    private Coroutine co_Recall;
    private PercentTimer colliderTimer;
    private PercentTimer lastingTimer;
    private float destroySelfDuration;

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        rb = GetComponent<Rigidbody2D>();
        damageTrigger = GetComponentInChildren<ShadowWallDamager>();
        cds = new List<BoxCollider2D>();
        animators = new List<Animator>();
        playerTransform = ServiceLocator.Get<Player>().transform;
        shadowManager = ServiceLocator.Get<ShadowManager>();
        shadowWallInfo = shadowManager.shadowWallInfo;

        centerOffset = centerTransform.position - transform.position;

        colliderTimer = new PercentTimer();
        colliderTimer.OnTick += x => cds.ForEach(cd => cd.size = new Vector2(cd.size.x, defaultSizeY * x));

        lastingTimer = new PercentTimer();
        lastingTimer.OnComplete += RecallSelf;
        lastingTimer.Initialize(shadowWallInfo.stayingDuration, false);

        float scale = transform.localScale.x;
        float startX = shadowWallInfo.missileJoinDist * (0.5f - shadowWallInfo.missileJoinCount / 2f) / scale;
        GameObject bodyGO = GetComponentInChildren<SpriteRenderer>().gameObject;
        bodyGO.transform.localPosition = new Vector3(startX, 0f);
        cds.Add(bodyGO.GetComponent<BoxCollider2D>());
        animators.Add(bodyGO.GetComponent<Animator>());
        defaultSizeY = cds[0].size.y;
        for (int i = 1; i < shadowWallInfo.missileJoinCount; i++)
        {
            GameObject newBody = Instantiate(bodyGO, transform);
            newBody.transform.localPosition = new Vector3(startX + shadowWallInfo.missileJoinDist / scale * i, 0f);
            cds.Add(newBody.GetComponent<BoxCollider2D>());
            animators.Add(newBody.GetComponent<Animator>());
        }
        damageTrigger.Initialize(shadowWallInfo.missileJoinDist * shadowWallInfo.missileJoinCount / scale);
        destroySelfDuration = animators[0].runtimeAnimatorController.animationClips.First(
            x => x.name.Equals("DestroySelf")).length;

        myObject.OnRecycle += OnRecycle;
    }

    public void Initialize(Vector3 setVec) => StartCoroutine(DoSetFromVec(setVec));

    private IEnumerator DoSetFromVec(Vector3 setVec)
    {
        // 这块写的有点乱，凑合能用
        transform.position -= centerOffset;
        damageTrigger.SetDamageOn(shadowManager.shadowInfo.Damage * shadowWallInfo.missileDamageMultiplier);
        cds.ForEach(x => x.enabled = true);
        Vector2 originalPos = transform.position;
        Vector2 endPos = transform.position + setVec;
        float setDuration = setVec.magnitude / shadowManager.shadowInfo.flyingSpeed;
        colliderTimer.Initialize(setDuration);
        float deltaT = Time.fixedDeltaTime / setDuration;
        float t = 0f;
        yield return new WaitForFixedUpdate();
        while (t < 1f)
        {
            t += deltaT;
            Vector2 aimPos = Vector2.Lerp(originalPos, endPos, t);
            rb.MovePosition(aimPos);
            yield return new WaitForFixedUpdate();
        }
        lastingTimer.Restart();
        damageTrigger.SetDamageOff();
    }

    private void RecallSelf()
    {
        if (co_Recall != null) StopCoroutine(co_Recall);
        co_Recall = StartCoroutine(DoRecallSelf());
    }

    private IEnumerator DoRecallSelf()
    {
        cds.ForEach(x => x.enabled = false);
        animators.ForEach(x => x.Play("DestroySelf", 0, 0f));
        yield return new WaitForSeconds(destroySelfDuration);
        //while (true)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position,
        //        playerTransform.position - centerOffset,
        //        shadowManager.shadowInfo.flyingSpeed * Time.deltaTime);
        //    if ((playerTransform.position - centerTransform.position).sqrMagnitude < 0.01f) break;
        //    yield return null;
        //}
        shadowManager.shadowWallChargeTimer.Restart();
        DestroySelf();
    }

    public void DestroySelf()
    {
        myObject.Recycle();
    }

    public void OnRecycle()
    {
        StopAllCoroutines();
        co_Recall = null;
        
        // 确保计时器被弃用时Paused为true
        lastingTimer.Paused = true;
        colliderTimer.Paused = true;
    }
}
