using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShadowManager : Service
{
    [Other] private ObjectManager objectManager;
    [Other] private AudioManager audioManager;
    private PlayerAnimator playerAnimator;
    private Camera mainCamera;
    private SkillInfoData skillInfoData;

    public event UnityAction<float> OnSetShadowDistChange;
    public event UnityAction<Shadow> OnSetShadow;
    public event UnityAction<Shadow> OnCloneShadow;
    public event UnityAction<Shadow> AfterRecallShadow;
    public event UnityAction AfterSetShadowWall;
    public event UnityAction<bool> OnSetShadowWallActive;

    [Label("能够释放影子")]
    public bool ableToSetShadow;
    [Label("能够释放禁军之墙")]
    public bool ableToSetShadowWall;
    [Label("影子信息")]
    public ShadowInfo shadowInfo;
    [Label("暗影之墙信息")]
    public ShadowWallInfo shadowWallInfo;
    private List<Shadow> shadowList;
    private int shadowCount;
    public ShadowChargeTimer shadowChargeTimer;
    public PercentTimer shadowWallChargeTimer;

    private float shadowSetDist;
    public float ShadowSetDist
    {
        get => shadowSetDist;
        set
        {
            shadowSetDist = value;
            OnSetShadowDistChange?.Invoke(shadowSetDist);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        shadowInfo = new ShadowInfo();
        shadowList = new List<Shadow>();
        shadowChargeTimer = new ShadowChargeTimer(shadowList);
        shadowWallChargeTimer = new PercentTimer();
        shadowWallChargeTimer.OnComplete += () => ableToSetShadowWall = true;
    }

    protected override void Start()
    {
        base.Start();
        playerAnimator = ServiceLocator.Get<Player>().PlayerAnimator;
        skillInfoData = ServiceLocator.Get<SkillManager>().SkillInfoData;
        mainCamera = Camera.main;
        shadowWallInfo = new ShadowWallInfo(skillInfoData);
        shadowWallChargeTimer.Initialize(skillInfoData.missileChargeCD_PushOn, false);

        GameManager gameManager = ServiceLocator.Get<GameManager>();
        gameManager.BeforeGamePause += BeforeGamePause;
        gameManager.AfterGameResume += AfterGameResume;

        EventSystem eventSystem = ServiceLocator.Get<EventSystem>();
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    public void Initialize()
    {
        shadowCount = 0;
        ableToSetShadow = true;
        ableToSetShadowWall = false;
        shadowInfo.Initialize(skillInfoData);
        shadowChargeTimer.Initialize(skillInfoData);
        ShadowSetDist = skillInfoData.shadowSetRadius;
    }

    private void Update()
    {
        if (ableToSetShadow && Input.GetMouseButtonDown(0))
        {
            SetShadow();
        }
    }

    private void SetShadow()
    {
        if (ableToSetShadowWall)
        {
            GenerateShadowWall(true);
            if (shadowWallInfo.bidirectional) GenerateShadowWall(false);
            ableToSetShadowWall = false;
            AfterSetShadowWall?.Invoke();
            audioManager.PlaySound("PushOn_Release",AudioPlayMode.Wait);
            
        }
        else if (shadowChargeTimer.ShadowChargedCount > 0)
        {
            GenerateShadow();
            shadowChargeTimer.ShadowChargedCount--;
            playerAnimator.PlaySetShadowMotion();
        }
    }

    private void GenerateShadow()
    {
        Shadow newShadow = objectManager.Activate(
            EObject.Shadow, transform.position).Transform.GetComponent<Shadow>();
        newShadow.Initialize(++shadowCount, GetClampedMouseVec());
        shadowList.Add(newShadow);
        OnSetShadow?.Invoke(newShadow);
    }

    public void CloneShadow(Vector3 shadowPos)
    {
        Shadow newShadow = objectManager.Activate(
            EObject.Shadow, shadowPos).Transform.GetComponent<Shadow>();
        newShadow.InitializeFromClone();
        OnCloneShadow?.Invoke(newShadow);
    }

    private void GenerateShadowWall(bool forward)
    {
        objectManager.Activate(EObject.ShadowWall, transform.position).Transform
            .GetComponent<ShadowWall>().Initialize((forward ? 1f : -1f) * GetClampedMouseVec());
    }

    public void SetShadowWallSetable(bool active)
    {
        ableToSetShadowWall = active;
        if (!active)
        {
            shadowWallChargeTimer.Paused = true;
        }
        OnSetShadowWallActive?.Invoke(active);
    }

    private Vector3 GetClampedMouseVec()
    {
        return Vector2.ClampMagnitude(mainCamera.ScreenToWorldPoint(Input.mousePosition) 
            - transform.position, ShadowSetDist);
    }

    public void RecallAllShadows()
    {
        shadowList.ForEach(x => x.RecallSelf());
    }

    public void RemoveShadow(Shadow shadow)
    {
        if (!shadow.createdByClone)
        {
            shadowList.Remove(shadow);
            shadowChargeTimer.UpdateAfterRecallShadow();
        }
        AfterRecallShadow?.Invoke(shadow);
    }

    public bool GetNearestShadow(out Shadow nearestShadow, out float shortestDist)
    {
        nearestShadow = null;
        shortestDist = float.MaxValue;
        if (shadowList.Count == 0) return false;
        float dist;
        foreach (var shadow in shadowList)
        {
            dist = (shadow.transform.position - transform.position).magnitude;
            if (dist < shortestDist)
            {
                nearestShadow = shadow;
                shortestDist = dist;
            }
        }
        return true;
    }

    private void BeforeGamePause()
    {
        ableToSetShadow = false;
    }

    private void AfterGameResume()
    {
        ableToSetShadow = true;
    }

    private void BeforeLoadScene(int _)
    {
        shadowChargeTimer.Paused = true;
        shadowList.Clear();
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            Initialize();
        }
    }
}
