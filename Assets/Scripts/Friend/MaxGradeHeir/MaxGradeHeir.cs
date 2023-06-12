using MyTimer;
using Services;
using UnityEngine;

public class MaxGradeHeir : MonoBehaviour
{
    private Player player;
    private ObjectManager objectManager;
    private EnemyManager enemyManager;
    private ShadowInfo shadowInfo;
    private SpriteRenderer sr;

    private float currentRoundAngle;
    private RepeatTimer shootTimer;
    [SerializeField, Label("环绕速度")]
    private float roundSpeed;
    [SerializeField, Label("环绕半径")]
    private float roundRadius;

    private float attackDamageMultiplier;
    private float attackDist;
    private float bulletSpeed;

    private void Awake()
    {
        player = ServiceLocator.Get<Player>();
        objectManager = ServiceLocator.Get<ObjectManager>();
        enemyManager = ServiceLocator.Get<EnemyManager>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        sr = GetComponent<SpriteRenderer>();

        Transform heir = new GameObject("Heir").transform;
        heir.SetParent(player.transform);
        heir.localPosition = Vector3.zero;
        transform.SetParent(heir);
        transform.localPosition = Vector3.up * roundRadius;
        currentRoundAngle = 0f;

        shootTimer = new RepeatTimer();
        shootTimer.OnComplete += Shoot;

        player.OnPlayerActiveChange += SetActive;
    }

    /// <summary>
    /// 初始化，之后不再变化
    /// </summary>
    /// <param name="attackInterval">攻击间隔</param>
    /// <param name="attackDist">攻击距离</param>
    /// <param name="attackDamageMultiplier">攻击伤害相对影子基础伤害倍率</param>
    /// <param name="bulletSpeed">子弹速度</param>
    public void Initialize(float attackInterval, float attackDist, 
        float attackDamageMultiplier, float bulletSpeed)
    {
        this.attackDamageMultiplier = attackDamageMultiplier;
        this.attackDist = attackDist;
        this.bulletSpeed = bulletSpeed;
        shootTimer.Initialize(attackInterval);
    }

    private void Update()
    {
        Round();
    }

    private void Round()
    {
        currentRoundAngle = (currentRoundAngle + roundSpeed * Time.deltaTime) % 360;
        transform.localPosition = Quaternion.AngleAxis(currentRoundAngle, Vector3.forward)
            * Vector3.up * roundRadius;
    }

    private void Shoot()
    {
        Enemy nearestEnemy = enemyManager.FindNearestEnemyToPlayer(attackDist);
        if (nearestEnemy != null)
        {
            objectManager.Activate(EObject.HeirBullet, transform.position)
              .Transform.GetComponent<NormalBullet>().Initialize(
                nearestEnemy.transform.position - transform.position, 
                shadowInfo.Damage * attackDamageMultiplier, bulletSpeed, 10f);
        }
    }

    private void SetActive(bool active)
    {
        sr.enabled = active;
        shootTimer.Paused = !active;
    }

    public void DestroySelf()
    {
        shootTimer.Paused = true;
        player.OnPlayerActiveChange -= SetActive;
        Destroy(gameObject);
    }
}