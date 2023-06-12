using MyTimer;
using Services;
using System.Collections;
using UnityEngine;

public enum ShadowState
{
    /// <summary>
    /// 放置中
    /// </summary>
    [Label("放置中")]
    Setting,

    /// <summary>
    /// 创造中
    /// </summary>
    [Label("创造中")]
    Creating,

    /// <summary>
    /// 滞留中
    /// </summary>
    [Label("滞留中")]
    Staying,

    /// <summary>
    /// 被踢球中
    /// </summary>
    [Label("被踢球中")]
    Kicking,

    /// <summary>
    /// 召回中
    /// </summary>
    [Label("召回中")]
    Recalling,
}

public class Shadow : MonoBehaviour
{
    private MyObject myObject;
    private Player player;
    private ShadowManager shadowManager;
    private EventSystem eventSystem;
    private AudioManager audioManager;

    private ShadowInfo shadowInfo;
    private ShadowAnimator shadowAnimator;
    private SpriteRenderer sr;
    private ShadowDamager shadowDamager;
    private ShadowLastingProgress shadowLastingProgress;
    private SortingOrderTool sortingOrderTool;

    [Label("中心位置")]
    public Transform centerTransform;
    private Vector3 centerOffset;
    public bool createdByClone;

    private Coroutine co_Recall;
    private FaceTool faceTool;
    public ShadowTransformation setTransformation;
    private ShadowTransformation kickTransformation;
    public ShadowLastingTimer lastingTimer;
    private CountdownPercentTimer fadeOutTimer;

    public Vector3 MoveDir { get; private set; }

    public ShadowState ShadowState { get; private set; }

    public float Size
    {
        get => transform.localScale.x;
        set
        {
            value = Mathf.Min(value, shadowInfo.scaleUpperLimit);
            transform.localScale = new Vector3(value, value, 1f);
            centerOffset = centerTransform.position - transform.position;
        }
    }

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        player = ServiceLocator.Get<Player>();
        shadowManager = ServiceLocator.Get<ShadowManager>();
        eventSystem = ServiceLocator.Get<EventSystem>();
        audioManager = ServiceLocator.Get<AudioManager>();
        shadowInfo = shadowManager.shadowInfo;

        shadowAnimator = GetComponentInChildren<ShadowAnimator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        shadowDamager = GetComponentInChildren<ShadowDamager>();
        shadowLastingProgress = GetComponentInChildren<ShadowLastingProgress>();
        sortingOrderTool = new SortingOrderTool(gameObject);
        faceTool = new FaceTool(shadowAnimator.transform);
        setTransformation = new ShadowTransformation(transform, shadowInfo);
        setTransformation.OnTick += _ => FlyingDistChangeSize();
        setTransformation.OnComplete += CompleteSetting;

        kickTransformation = new ShadowTransformation(transform);
        kickTransformation.OnComplete += OnCompleteKick;

        lastingTimer = new ShadowLastingTimer();
        lastingTimer.OnTick += shadowLastingProgress.UpdateProgress;
        lastingTimer.OnComplete += RecallSelf;

        fadeOutTimer = new CountdownPercentTimer();
        fadeOutTimer.OnTick += x => sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, x);
        fadeOutTimer.Initialize(0.5f, false);

        myObject.OnActivate += OnActivate;
        myObject.OnRecycle += OnRecycle;
    }

    private void OnActivate()
    {
        faceTool.Reset();
        shadowDamager.SetDamageOff();
        shadowLastingProgress.Visible = false;
        Size = shadowInfo.basicScale;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        if (!shadowLastingProgress.gameObject.activeSelf)
            shadowLastingProgress.gameObject.SetActive(true);
    }

    public void Initialize(int order, Vector3 setVec)
    {
        name = $"Shadow{order}";
        shadowLastingProgress.SetSortingOrder(sortingOrderTool.SetSortingOrders(order));

        createdByClone = false;
        faceTool.FaceRight = player.faceTool.FaceRight;
        lastingTimer.SetLastingTime(shadowInfo.stayingDuration);
        StartSetting(setVec);
    }

    public void InitializeFromClone()
    {
        createdByClone = true;
        faceTool.FaceRight = player.faceTool.FaceRight;
        lastingTimer.SetLastingTime(shadowInfo.stayingDuration);
        CompleteSetting();
    }

    public void InitializeStaying()
    {
        createdByClone = true;
        faceTool.FaceRight = player.faceTool.FaceRight;
        transform.position -= centerOffset;
        ShadowState = ShadowState.Staying;
        shadowAnimator.PlayMotion(EShadowMotion.Stay);
    }

    public void InitializeCreating()
    {
        createdByClone = true;
        faceTool.FaceRight = player.faceTool.FaceRight;
        lastingTimer.SetLastingTime(1f);
        CompleteSetting();
        shadowLastingProgress.gameObject.SetActive(false);
    }

    private void StartSetting(Vector3 setVec)
    {
        ShadowState = ShadowState.Setting;
        shadowAnimator.PlayMotion(EShadowMotion.Set);
        MoveDir = setTransformation.InitializeByMoveVec(setVec);
        audioManager.PlaySound("ReleaseShadow");
    }

    public void CompleteSetting()
    {
        setTransformation.Paused = true;
        shadowDamager.SetDamageOff();
        ShadowState = ShadowState.Creating;
        shadowAnimator.PlayMotion(EShadowMotion.Create);
    }

    public void CompleteCreating()
    {
        ShadowState = ShadowState.Staying;
        shadowLastingProgress.Visible = true;
        lastingTimer.Restart();
        eventSystem.Invoke(EEvent.AfterCreateShadow, this);
    }

    public void GetKicked(float kickDist, float kickSpeed, float damageMultiplier)
    {
        ShadowState = ShadowState.Kicking;
        MoveDir = kickTransformation.InitializeByMoveVec(
            (
                Mathf.Approximately(player.CurrentSpeed, 0f)
                    ? Vector3.right
                    : player.CurrentVelocity.normalized
            ) * kickDist,
            kickSpeed
        );
        SetDamageOn(damageMultiplier);
    }

    private void OnCompleteKick()
    {
        shadowDamager.SetDamageOff();
        ShadowState = ShadowState.Staying;
    }

    public void SetDamageOn(float damageMultiplier)
    {
        shadowDamager.SetDamageOn(damageMultiplier);
    }

    private void FlyingDistChangeSize()
    {
        Size += shadowInfo.flyingSpeed * Time.deltaTime * shadowInfo.flyingDistScaleMultiplier;
    }

    public void RecallSelf()
    {
        if (ShadowState != ShadowState.Recalling)
        {
            lastingTimer.Paused = true;
            shadowLastingProgress.Visible = false;
            if (co_Recall != null)
                StopCoroutine(co_Recall);
            co_Recall = StartCoroutine(DoRecallSelf());
            audioManager.PlaySound("GetBackShadow", AudioPlayMode.Plenty, 0.05f);
        }
    }

    private IEnumerator DoRecallSelf()
    {
        yield return new WaitUntil(() => ShadowState == ShadowState.Staying);
        ShadowState = ShadowState.Recalling;
        eventSystem.Invoke(EEvent.BeforeRecallShadow, this);
        MoveDir = player.transform.position - centerTransform.position;
        SetDamageOn(shadowInfo.recallDamageMultiplier);
        yield return new WaitForFixedUpdate();

        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position - centerOffset,
                shadowInfo.flyingSpeed * Time.deltaTime
            );
            FlyingDistChangeSize();
            if ((player.transform.position - centerTransform.position).sqrMagnitude < 0.01f)
                break;
            yield return null;
        }

        shadowManager.RemoveShadow(this);

        if (shadowInfo.reflectFlyingDist > 0)
        {
            Vector3 finalTarget =
                transform.position + MoveDir.normalized * shadowInfo.reflectFlyingDist;
            while (true)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    finalTarget,
                    shadowInfo.flyingSpeed * Time.deltaTime
                );
                if ((finalTarget - transform.position).sqrMagnitude < 0.01f)
                    break;
                yield return null;
            }
            fadeOutTimer.Restart();
            yield return new WaitForSeconds(fadeOutTimer.Duration);
        }

        myObject.Recycle();
    }

    private void OnRecycle()
    {
        StopAllCoroutines();
        co_Recall = null;

        // 确保计时器被弃用时Paused为true
        lastingTimer.ResetTimer();
        setTransformation.Paused = true;
        kickTransformation.Paused = true;
        lastingTimer.Paused = true;
        fadeOutTimer.Paused = true;
    }
}
