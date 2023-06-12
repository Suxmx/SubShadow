using System.Collections;
using Services;
using UnityEngine;

public abstract class Prop : MonoBehaviour
{
    [SerializeField]
    protected float followSpeed = 1f; //跟随速度

    [SerializeField]
    protected int backFrame = 16; //协程动画后退的fixed帧数
    protected MyObject myObject;
    protected TrailRenderer trail;
    protected PlayerStatusInfo playerStatusInfo;
    protected AudioManager audioManager;
    public bool Collectable { get; set; } //防止重复被拾取

    protected virtual void Awake()
    {
        myObject = GetComponent<MyObject>();
        trail = GetComponentInChildren<TrailRenderer>();
        playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo;
        audioManager = ServiceLocator.Get<AudioManager>();
        if (trail == null)
            Debug.LogError("该物体尚未添加拖尾");

        myObject.OnActivate += OnActivate;
    }

    protected virtual void OnActivate()
    {
        Collectable = true;
    }

    public virtual void DestroySelf()
    {
        myObject.Recycle();
    }

    public virtual void FollowAndRecycle(Transform targetTrans)
    {
        Collectable = false;
        StartCoroutine(Follow(targetTrans));
    }

    protected virtual IEnumerator Follow(Transform targetTrans)
    {
        Vector2 targetDir = (targetTrans.position - transform.position).normalized;
        Vector2 jitter = Random.Range(-1f, 1f) * new Vector2(-targetDir.y, targetDir.x);
        targetDir = (targetDir + jitter).normalized;
        float backv = followSpeed / 3.5f;
        float backa = backv / backFrame;
        trail.time = 0.35f;
        for (int i = 1; i <= backFrame; i++)
        {
            transform.Translate(backv * Time.deltaTime * -targetDir);
            backv -= backa;
            yield return new WaitForFixedUpdate();
        }
        Vector2 stepVec = -backv * targetDir;

        trail.time = 0.2f;
        Vector2 tarVec = targetTrans.position - transform.position;
        Vector2 tarDir;
        while (tarVec.sqrMagnitude > 0.1f) //随后持续追随目标
        {
            tarVec = targetTrans.position - transform.position;
            tarDir = tarVec.normalized;
            stepVec = Vector2.Lerp(stepVec, tarDir * followSpeed, 10f * Time.deltaTime);
            transform.Translate(stepVec * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        PlaySound();
        ChangeValue();
        DestroySelf();
    }

    protected abstract void ChangeValue();

    protected virtual void PlaySound()
    {
        audioManager.PlaySound("PlayerPickUp",AudioPlayMode.Plenty);
    }
}
