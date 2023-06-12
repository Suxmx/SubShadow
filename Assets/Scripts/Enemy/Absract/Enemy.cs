using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    /// <summary>
    /// ��
    /// </summary>
    [Label("��")]
    Hand,
    /// <summary>
    /// ���۹�
    /// </summary>
    [Label("���۹�")]
    BigEye,
    /// <summary>
    /// �Ա���
    /// </summary>
    [Label("�Ա���")]
    SelfExplosionEnemy,
}

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected MyObject myObject;
    protected EnemyStatusInfo enemyStatusInfo;
    protected EnemyCollider enemyCollider;
    protected EnemyAnimator enemyAnimator;
    protected GameObject enemyDamageTrigger;
    protected GameObject vertigoIcon;
    protected GameObject disarmIcon;
    protected Transform playerTransform;
    protected EnemyManager enemyManager;
    protected AudioManager audioManager;

    protected float speed;
    protected float followSpeed;
    protected EaseFloatTimer speedTimer;
    protected Vector3 dirToPlayer;

    public EnemyGroupTracker attachGroupTracker;
    protected FaceTool faceTool;
    protected EnemySpeedMultiplier enemySpeedMultiplier;
    protected List<Transform> attracters;

    public EnemyBeatenTimer beatenTimer;
    protected CountdownTimer vertigoTimer;
    protected Lock disarmLock;
    protected Lock loseControlLock;
    protected bool IsArmed => disarmLock.Unlocked;
    protected bool IsControllable => enemyStatusInfo.IsAlive && loseControlLock.Unlocked;

    public bool IsBoss => enemyStatusInfo.IsBoss;

    public bool IsAlive => enemyStatusInfo.IsAlive;

    public List<Enemy> LinkedEnemies { get; protected set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myObject = GetComponent<MyObject>();
        enemyStatusInfo = GetComponent<EnemyStatusInfo>();
        enemyCollider = GetComponentInChildren<EnemyCollider>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        enemyDamageTrigger = GetComponentInChildren<EnemyDamageTrigger>().gameObject;
        vertigoIcon = transform.Find("VertigoIcon").gameObject;
        vertigoIcon.SetActive(false);
        disarmIcon = transform.Find("DisarmIcon").gameObject;
        disarmIcon.SetActive(false);
        playerTransform = ServiceLocator.Get<Player>().transform;
        enemyManager = ServiceLocator.Get<EnemyManager>();
        audioManager=ServiceLocator.Get<AudioManager>();

        speedTimer = new EaseFloatTimer(EaseType.OutQuad);
        speedTimer.OnTick += x => speed = x;

        faceTool = new FaceTool(enemyAnimator.transform);
        enemySpeedMultiplier = new EnemySpeedMultiplier();
        attracters = new List<Transform>();

        beatenTimer = new EnemyBeatenTimer(rb);
        beatenTimer.OnResume += _ => loseControlLock++;
        beatenTimer.OnPause += OnRecoverFromGettingBeaten;

        vertigoTimer = new CountdownTimer();
        vertigoTimer.OnComplete += RecoverFromVertigo;

        disarmLock = new Lock(DisarmSelf, ArmSelf);
        loseControlLock = new Lock(LoseControl, GainControl);
        LinkedEnemies = new List<Enemy>();

        myObject.OnActivate += OnActivate;
        myObject.OnRecycle += OnRecycle;
    }

    protected virtual void OnActivate()
    {
        disarmLock.Reset();
        loseControlLock.Reset();
        faceTool.Reset();
        enemyCollider.EnableCollider(true);
        enemyDamageTrigger.SetActive(true);
    }

    public virtual void Initialize(EnemyInfo enemyInfo, EnemyGroupTracker enemyGroupTracker)
    {
        enemyStatusInfo.Initialize(enemyInfo.HP, enemyInfo.isBoss, enemyInfo.dropEXP * 
            (enemyGroupTracker == null ? 1 : enemyGroupTracker.expMultiplier));
        followSpeed = enemyInfo.followSpeed;
        attachGroupTracker = enemyGroupTracker;
        GainControl();
        ArmSelf();
    }

    protected virtual void FixedUpdate()
    {
        if (!IsControllable)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        dirToPlayer = (playerTransform.position - transform.position).normalized;
        faceTool.SetFace(dirToPlayer.x);
        if (attracters.Count > 0)
        {
            rb.velocity = enemySpeedMultiplier.multiplier * speed * 
                (attracters[0].position - transform.position).normalized;
        }
        else
        {
            EnemyFixedUpdate();
        }
    }

    protected abstract void EnemyFixedUpdate();

    protected virtual void MoveToPlayer()
    {
        rb.velocity = enemySpeedMultiplier.multiplier * speed * dirToPlayer;
    }

    protected void ResetSpeedTimer()
    {
        speed = 0f;
        speedTimer.Paused = true;
    }

    /// <summary>
    /// �ܵ�����Ч���������������ڵĵ��ˣ��������ⲿ����
    /// ����������д�˷�������д��LoseControl��
    /// </summary>
    public void GetBeaten(BeatInfo beatInfo)
    {
        if (IsBoss) return;
        if (beatInfo.disenableCollider) DisenableCollider();
        beatenTimer.Initialize(beatInfo);

        // ������������������ͨ������������Ҳ�ܵ�ͬ����beatenVec����Ч��
        // �������BFS�ķ�������������ͨ�ĵ��ˣ����Ƶ���֤�������������DFS����ܻ����
        if (LinkedEnemies.Count == 0) return;
        HashSet<Enemy> enemySet = new HashSet<Enemy> { this };
        Queue<Enemy> queue = new Queue<Enemy>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            Enemy enemy = queue.Dequeue();
            foreach (var linkedEnemy in enemy.LinkedEnemies)
            {
                if (!enemySet.Contains(linkedEnemy))
                {
                    if (beatInfo.beatVec.IsSimilarDir(linkedEnemy.transform.position - 
                        enemy.transform.position))
                    {
                        linkedEnemy.beatenTimer.Initialize(beatInfo);
                        queue.Enqueue(linkedEnemy);
                    }
                    enemySet.Add(linkedEnemy);
                }
            }
        }
    }

    /// <summary>
    /// ȡ����ײ��
    /// </summary>
    protected void DisenableCollider()
    {
        ClearLinkedEnemies();
        enemyCollider.DisenableCollider();
    }

    /// <summary>
    /// �ӻ����лָ�ʱ
    /// ����������д�˷�������д��GainControl��
    /// </summary>
    protected void OnRecoverFromGettingBeaten(Vector2 _)
    {
        enemyCollider.EnableCollider();
        loseControlLock--;
    }

    public void AddLinkedEnemy(Enemy otherEnemy)
    {
        if (!LinkedEnemies.Contains(otherEnemy))
        {
            if (!otherEnemy.beatenTimer.Paused)
            {
                Vector3 beatVec = otherEnemy.beatenTimer.BeatenVec;
                if (beatVec.IsSimilarDir(transform.position - otherEnemy.transform.position))
                {
                    GetBeaten(new BeatInfo(beatVec, otherEnemy.beatenTimer.beatenSpeed, false, true));
                }
            }
            LinkedEnemies.Add(otherEnemy);
        }
    }

    public void RemoveLinkedEnemy(Enemy otherEnemy)
    {
        LinkedEnemies.Remove(otherEnemy);
    }

    protected void ClearLinkedEnemies()
    {
        foreach (var linkedEnemy in LinkedEnemies)
        {
            linkedEnemy.RemoveLinkedEnemy(this);
        }
        LinkedEnemies.Clear();
    }

    /// <summary>
    /// �����ٶ�Ӱ������
    /// </summary>
    public void ActivateSpeedFactor(EnemySpeedFactorType factorType, float multiplier)
        => enemySpeedMultiplier.ActivateSpeedFactor(factorType, multiplier);

    /// <summary>
    /// ȡ�������ٶ�Ӱ������
    /// </summary>
    public void InactivateSpeedFactor(EnemySpeedFactorType factorType)
        => enemySpeedMultiplier.InactivateSpeedFactor(factorType);

    public void AddAttracter(Transform transform)
    {
        attracters.Add(transform);
    }

    public void RemoveAttracter(Transform transform)
    {
        attracters.Remove(transform);
    }

    public void AddDisarmer()
    {
        disarmLock++;
    }

    public void RemoveDisarmer()
    {
        disarmLock--;
    }

    protected virtual void DisarmSelf()
    {
        disarmIcon.SetActive(true);
    }

    protected virtual void ArmSelf()
    {
        disarmIcon.SetActive(false);
    }

    protected virtual void LoseControl()
    {
        ResetSpeedTimer();
        enemyAnimator.AddAnimatorLock();
    }

    protected virtual void GainControl()
    {
        enemyAnimator.RemoveAnimatorLock();
    }

    /// <summary>
    /// �ܵ�ѣ�Σ�����������д�˷�������д��LoseControl��
    /// </summary>
    public void GetVertigo(float vertigoDuration)
    {
        if (vertigoTimer.Paused)
        {
            rb.velocity = Vector2.zero;
            loseControlLock++;
            disarmLock++;
            vertigoIcon.SetActive(true);
            disarmIcon.SetActive(false);
        }
        vertigoTimer.Initialize(vertigoDuration);
    }

    /// <summary>
    /// ��ѣ���лָ�������������д�˷�������д��GainControl��
    /// </summary>
    private void RecoverFromVertigo()
    {
        loseControlLock--;
        disarmLock--;
        vertigoIcon.SetActive(false);
        if (!IsArmed) disarmIcon.SetActive(true);
    }

    public void DestroySelf()
    {
        OnDestroySelf();
        enemyAnimator.PlayDestroySelfMotion();
    }

    protected virtual void OnDestroySelf()
    {
        // ȷ��Timer������ʱΪPaused
        speedTimer.Paused = true;
        beatenTimer.Paused = true;
        vertigoTimer.Paused = true;

        vertigoIcon.SetActive(false);
        disarmIcon.SetActive(false);
        enemyStatusInfo.ResetStatusInfo();
        enemyCollider.DisenableCollider();
        enemyDamageTrigger.SetActive(false);
        enemySpeedMultiplier.Reset();
        attracters.Clear();
        ClearLinkedEnemies();

        enemyManager.RemoveEnemy(this);
        attachGroupTracker?.RemoveEnemy(this);
        attachGroupTracker = null;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// ��֤ǿ�ƻ��յ���ת����
    /// </summary>
    protected virtual void OnRecycle()
    {
        if (enemyStatusInfo.IsAlive) OnDestroySelf();
    }
}
