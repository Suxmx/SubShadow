using MyTimer;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Service
{
    [Other]
    private Player player;

    [Other]
    private ShadowManager shadowManager;

    [Other]
    private SkillManager skillManager;

    [Other]
    private EnemyManager enemyManager;

    [Other]
    private AudioManager audioManager;

    [Other]
    private ObjectManager objectManager;
    private CameraController cameraController;
    private PlayerStatusInfo playerStatusInfo;
    private UpgradeUIController upgradeUIController;
    private PauseUIController pauseUIController;
    private DieUIController dieUIController;
    private WinUIController winUIController;

    public event UnityAction BeforeGamePause;
    public event UnityAction AfterGameResume;

    public TimerOnly gameTimer;
    private Lock gamePauseLock;
    private bool gamePaused;
    private int gradeAddCount;
    private CountdownTimer freezeTimer;
    private float getHurtFreezeTime;
    private bool pausable;
    private Coroutine winCoroutine;

    // 测试用
    [HideInInspector]
    public bool enemySpawnOn;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        gameTimer = new TimerOnly(true);
        gamePauseLock = new Lock(PauseGame, ResumeGame);

        freezeTimer = new CountdownTimer(true);
        freezeTimer.OnResume += _ => Time.timeScale = 0f;
        freezeTimer.OnComplete += () =>
        {
            if (gamePauseLock.Unlocked)
                Time.timeScale = 1f;
        };
    }

    protected override void Start()
    {
        base.Start();
        cameraController = Camera.main.GetComponent<CameraController>();
        playerStatusInfo = player.PlayerStatusInfo;
        upgradeUIController = UIControllerLocator.Get<UpgradeUIController>();
        pauseUIController = UIControllerLocator.Get<PauseUIController>();
        dieUIController = UIControllerLocator.Get<DieUIController>();
        winUIController = UIControllerLocator.Get<WinUIController>();
        getHurtFreezeTime = ServiceLocator.Get<SkillManager>().SkillInfoData.getHurtFreezeTime;
        playerStatusInfo.BeforeGetHurt += () => FreezeTime(getHurtFreezeTime);

        EventSystem eventSystem = ServiceLocator.Get<EventSystem>();
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);

        InputUtility.StartCheckInputWord("shadow", skillManager.UpgradeAllSkills);
    }

    private void Initialize()
    {
        gameTimer.OnTick += CheckWin;
        gameTimer.Restart();
        gamePauseLock.Reset();
        gamePaused = false;
        gradeAddCount = 0;
        enemySpawnOn = true;
        pausable = true;
    }

    private void Update()
    {
        #region 按键动态控制参数

        // if (InputUtility.CheckPressAnyNumKey(out var num))
        // {
        //     Skill skill = null;
        //     if (num >= 1 && num <= 8)
        //     {
        //         skill = skillManager.attackSkills[(SkillType)(num - 1)];
        //     }
        //     else if (num >= 11 && num <= 19)
        //     {
        //         skill = skillManager.assistantSkills[(SkillType)(num - 3)];
        //     }
        //     if (skill != null)
        //     {
        //         if (skill.ownedGrade >= 4)
        //             skill.RemoveSkill();
        //         else
        //             skill.UpgradeSkill();
        //     }
        // }
        //
        // InputUtility.CheckValueChangePress(
        //     shadowManager.shadowChargeTimer.MaxShadowChargeCount,
        //     KeyCode.Q,
        //     KeyCode.E,
        //     x => shadowManager.shadowChargeTimer.MaxShadowChargeCount = (int)x
        // );
        // InputUtility.CheckValueChangePress(
        //     shadowManager.shadowChargeTimer.ShadowChargeCD,
        //     KeyCode.R,
        //     KeyCode.T,
        //     x => shadowManager.shadowChargeTimer.ShadowChargeCD = x
        // );
        // InputUtility.CheckValueChangePress(
        //     shadowManager.shadowInfo.stayingDuration,
        //     KeyCode.Y,
        //     KeyCode.U,
        //     x => shadowManager.shadowInfo.stayingDuration = x
        // );
        // InputUtility.CheckValueChangePress(
        //     shadowManager.shadowInfo.basicDamage,
        //     KeyCode.F,
        //     KeyCode.G,
        //     x => shadowManager.shadowInfo.basicDamage = x,
        //     1f,
        //     1f
        // );
        //
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     playerStatusInfo.EXP = playerStatusInfo.maxEXP;
        // }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     enemySpawnOn = !enemySpawnOn;
        //     enemyManager.Paused = !enemySpawnOn;
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     playerStatusInfo.ClearAllDelayDamages();
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     enemyManager.SpawnTestEnemy(EnemyType.Hand);
        // }
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     enemyManager.SpawnTestEnemy(EnemyType.BigEye);
        // }
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     enemyManager.SpawnTestEnemy(EnemyType.SelfExplosionEnemy);
        // }
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     enemyManager.SpawnTestOrbitEnemyGroup();
        // }
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     enemyManager.SpawnTestEllipseEnemyGroup();
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     enemyManager.SpawnTestCorkscrewEnemyGroup();
        // }
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     StartCoroutine(PlayWinPerformance());
        // }
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     playerStatusInfo.GetHealed(1f);
        // }
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     ResetAllParas();
        // }

        #endregion

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneController.InGame)
            {
                if (gamePaused)
                    pauseUIController.ResumeGame();
                else if (pausable)
                    OpenPauseUI();
            }
        }
    }

    /// <summary>
    /// 重置玩家所有状态和参数
    /// </summary>
    public void ResetAllParas()
    {
        skillManager.RemoveAllSkills();
        player.Initialize();
        shadowManager.Initialize();
    }

    public void OpenUpgradeUI()
    {
        if (++gradeAddCount == 1)
        {
            gamePauseLock++;
            upgradeUIController.Upgrade();
        }
    }

    public void CloseUpgradeUI()
    {
        if (--gradeAddCount == 0)
            gamePauseLock--;
        else
            upgradeUIController.Upgrade();
    }

    private void OpenPauseUI()
    {
        pauseUIController.PauseGame();
        gamePauseLock++;
        gamePaused = true;
    }

    public void ClosePauseUI()
    {
        gamePauseLock--;
        gamePaused = false;
    }

    private void PauseGame()
    {
        BeforeGamePause?.Invoke();
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        AfterGameResume?.Invoke();
    }

    public void OpenDieUI()
    {
        audioManager.bgmController.PlayerDie();
        dieUIController.PlayerDie();
        gamePauseLock++;
        pausable = false;
    }

    public void CloseDieUI()
    {
        gamePauseLock--;
        player.Reborn();
        pausable = true;
    }

    public void FreezeTime(float freezeTime)
    {
        freezeTimer.Initialize(freezeTime);
    }

    private void CheckWin(float current)
    {
        if (current >= 1200f)
        {
            gameTimer.Paused = true;
            if (winCoroutine != null) StopCoroutine(winCoroutine);
            winCoroutine = StartCoroutine(PlayWinPerformance());
        }
    }

    private IEnumerator PlayWinPerformance()
    {
        pausable = false;
        player.Active = false;
        skillManager.RemoveAllSkills();
        player.PlayerAnimator.PlayMotion(EPlayerMotion.Idle);
        enemyManager.DestroyAllEnemies();
        shadowManager.RecallAllShadows();
        enemyManager.Paused = true;
        yield return new WaitForSeconds(0.5f);

        audioManager.StopGroup(ESoundGroup.BGM,true);
        // audioManager.bgmController.Success();
        EaseFloatTimer timer = new(EaseType.OutQuad);
        timer.OnTick += x => cameraController.SetOrthoSize(x);
        float closerTime = 1f;
        timer.Initialize(5f, 2f, closerTime);
        yield return new WaitForSeconds(closerTime);

        Shadow newShadow = objectManager
            .Activate(EObject.Shadow, player.transform.position)
            .Transform.GetComponent<Shadow>();
        newShadow.InitializeStaying();
        newShadow.GetKicked(5f, 10f, 0f);
        audioManager.PlaySound("ReleaseShadow");//踢出影子音效
        yield return new WaitForSeconds(1f);
        newShadow.GetComponent<MyObject>().Recycle();

        List<Shadow> shadows = new(8);
        for (int i = 0; i < 8; i++)
        {
            float angle = Mathf.Deg2Rad * 45 * i;
            Vector3 pos =
                player.transform.position + 2.5f * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            Shadow shadow = objectManager
                .Activate(EObject.Shadow, pos)
                .Transform.GetComponent<Shadow>();
            shadow.InitializeCreating();
            shadows.Add(shadow);
            audioManager.PlaySound("ReleaseShadow", AudioPlayMode.Plenty, 0.05f);//播放多个影子释放音效
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1.6f);//影子碰到玩家卡点播放玩家受伤音频
        audioManager.PlaySound("PlayerHurt");
        player.PlayerAnimator.PlayMotion(EPlayerMotion.Die, 0f);
        yield return new WaitForSeconds(0.5f);
        for (int i = 1; i <= 5; i++)//玩家逐渐透明消失
        {
            player.transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color(
                1,
                1,
                1,
                1 - i * 0.2f
            );
            yield return new WaitForSeconds(0.1f);
        }

        audioManager.bgmController.Success();
        winUIController.PlayerWin();
        gamePauseLock++;
    }

    private void BeforeLoadScene(int _)
    {
        gameTimer.Paused = true;
        gameTimer.ResetEvents();
        freezeTimer.Paused = true;
        cameraController.SetOrthoSize(5f);
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        if (winCoroutine != null)
        {
            StopCoroutine(winCoroutine);
            winCoroutine = null;
        }
        if (Mathf.Approximately(Time.timeScale, 0f))
            ResumeGame();
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            Initialize();
        }
    }
}
