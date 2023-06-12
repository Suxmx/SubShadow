using MyTimer;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Player : Service
{
    [Other] private ShadowManager shadowManager;
    [Other] private GameManager gameManager;
    private SkillInfoData skillInfoData;
    private Rigidbody2D rb;
    private List<Collider2D> colliders;
    private Renderer bodyRenderer;
    private List<Light2D> lights;
    private CollectTrigger collectTrigger;
    private PlayerHeal playerHeal;

    public event UnityAction<bool> OnPlayerActiveChange;
    public event UnityAction<float> OnPlayerMove;

    public FaceTool faceTool;
    public float speed;
    private bool movable;
    private float hMove, vMove;
    //private PlayerSpeedMultiplier speedMultiplier;
    private CountdownTimer invincibleTimer;
    private BeatInfo beatInfo;

    private bool active;
    public bool Active
    {
        get => active;
        set
        {
            if (active != value)
            {
                active = value;
                colliders.ForEach(c => c.enabled = value);
                movable = value;
                shadowManager.ableToSetShadow = value;
                OnPlayerActiveChange?.Invoke(active);
            }
        }
    }

    private float collectRadius;
    public float CollectRadis
    {
        get => collectRadius;
        set
        {
            collectRadius = value;
            collectTrigger.SetScale(collectRadius);
        }
    }

    public float CurrentSpeed { get; private set; }
    public Vector3 CurrentVelocity { get; private set; }
    public PlayerStatusInfo PlayerStatusInfo { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>().ToList();
        lights = GetComponentsInChildren<Light2D>().ToList();
        collectTrigger = GetComponentInChildren<CollectTrigger>();
        PlayerStatusInfo = GetComponent<PlayerStatusInfo>();
        PlayerAnimator = GetComponentInChildren<PlayerAnimator>();
        bodyRenderer = PlayerAnimator.GetComponent<Renderer>();
        playerHeal = GetComponentInChildren<PlayerHeal>();

        faceTool = new FaceTool(PlayerAnimator.transform);
        hMove = vMove = 0f;
        movable = active = true;
        PlayerStatusInfo.BeforeGetHurt += CauseBeat;
        PlayerStatusInfo.OnDie += () => StartCoroutine(Die());
    }

    protected override void Start()
    {
        base.Start();

        skillInfoData = ServiceLocator.Get<SkillManager>().SkillInfoData;
        beatInfo = new BeatInfo(skillInfoData.getHurtBeatEnemyDist, skillInfoData.getHurtBeatEnemySpeed, true);
        invincibleTimer = new CountdownTimer();
        invincibleTimer.OnResume += _ => colliders[0].enabled = false;
        invincibleTimer.OnPause += _ => colliders[0].enabled = true;
        invincibleTimer.Initialize(skillInfoData.getHurtInvincibleDuration, false);
        PlayerStatusInfo.OnGetHurt += () => invincibleTimer.Restart();
        //speedMultiplier = new PlayerSpeedMultiplier(skillInfoData);

        GameManager gameManager = ServiceLocator.Get<GameManager>();
        gameManager.BeforeGamePause += BeforeGamePause;
        gameManager.AfterGameResume += AfterGameResume;

        EventSystem eventSystem = ServiceLocator.Get<EventSystem>();
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    public void Initialize()
    {
        faceTool.Reset();
        Active = true;
        CollectRadis = skillInfoData.collectRadius;
        speed = skillInfoData.playerSpeed;
        PlayerStatusInfo.Initialize(skillInfoData);
    }

    private void Update()
    {
        CheckMoveInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 检测输入并转向
    /// </summary>
    private void CheckMoveInput()
    {
        if (movable)
        {
            hMove = Input.GetAxis("Horizontal");
            faceTool.SetFace(hMove);
            vMove = Input.GetAxis("Vertical");
        }
    }

    private void Move()
    {
        CurrentVelocity = rb.velocity;
        CurrentSpeed = CurrentVelocity.magnitude;
        OnPlayerMove?.Invoke(CurrentSpeed * Time.fixedDeltaTime);
        if (movable)
        {
            //rb.velocity = defaultSpeed * speedMultiplier.multiplier * 
            //    new Vector2(hMove, vMove).normalized;
            rb.velocity = speed * new Vector2(hMove, vMove).normalized;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    //public void AdjustSpeed(PlayerSpeedFactorType factorType, bool active = true)
    //{
    //    speedMultiplier.SetSpeedFactorActive(factorType, active);
    //}

    private void CauseBeat()
    {
        Collider2D[] targetEnemies = Physics2D.OverlapCircleAll(transform.position,
            skillInfoData.getHurtBeatRadius, EnemyManager.enemyLayerMask);
        foreach (var enemyCollider in targetEnemies)
        {
            Vector3 beatDir = enemyCollider.transform.position - transform.position;
            beatInfo.beatVec = beatDir.normalized * beatInfo.beatDist;
            enemyCollider.GetComponentInParent<Enemy>().GetBeaten(beatInfo);
        }
    }

    private IEnumerator Die()
    {
        rb.velocity = Vector2.zero;
        PlayerStatusInfo.ResetStatusInfo();
        PlayerAnimator.ResetAnimator();
        Active = false;
        PlayerAnimator.PlayMotion(EPlayerMotion.Die, 0f);
        yield return new WaitForSeconds(0.7f);
        int dieTime = Mathf.FloorToInt(gameManager.gameTimer.Current);
        Debug.Log($"玩家死亡，时间为{dieTime / 60}:{dieTime % 60:00}");
        gameManager.OpenDieUI();
    }

    public void Reborn()
    {
        PlayerStatusInfo.ResetHP();
        PlayerAnimator.ResetAnimator();
        Active = true;
    }

    private void BeforeGamePause()
    {
        movable = false;
    }

    private void AfterGameResume()
    {
        movable = true;
    }

    private void BeforeLoadScene(int _)
    {
        //speedMultiplier.Reset();
        invincibleTimer.Paused = true;
        PlayerStatusInfo.ResetStatusInfo();
        PlayerAnimator.ResetAnimator();
        Active = false;
        bodyRenderer.enabled = false;
        lights.ForEach(x => x.enabled = false);
        playerHeal.ResetHealAnimator();
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            Initialize();
            bodyRenderer.enabled = true;
            lights.ForEach(x => x.enabled = true);
        }
    }
}
