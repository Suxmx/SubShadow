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
    [SerializeField, Label("�����ٶ�")]
    private float roundSpeed;
    [SerializeField, Label("���ư뾶")]
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
    /// ��ʼ����֮���ٱ仯
    /// </summary>
    /// <param name="attackInterval">�������</param>
    /// <param name="attackDist">��������</param>
    /// <param name="attackDamageMultiplier">�����˺����Ӱ�ӻ����˺�����</param>
    /// <param name="bulletSpeed">�ӵ��ٶ�</param>
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