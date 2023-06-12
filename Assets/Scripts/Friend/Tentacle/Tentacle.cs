using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class Tentacle : MonoBehaviour
{
    private MyObject myObject;
    private SpriteShapeRenderer ssr;
    private ShadowInfo shadowInfo;

    private TentacleInfo tentacleInfo;
    private float currentAttackInterval;
    private float damageToEnemy;
    private RepeatTimer attackTimer;
    private List<CharacterStatusInfo> targets;

    private event UnityAction OnAttack;

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        ssr = GetComponent<SpriteShapeRenderer>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;

        attackTimer = new RepeatTimer();
        targets = new List<CharacterStatusInfo>();

        attackTimer.OnComplete += Attack;
        // float angle = 300 * Mathf.PI / 180;
        // float cos = Mathf.Cos(angle);
        // float sin = Mathf.Sin(angle);
        // Debug.Log(cos + " " + sin);
        // Debug.Log(GainDir(new Vector2(cos, sin)));
    }

    /// <summary>
    /// ��ʼ����֮���ٱ仯
    /// </summary>
    /// <param name="tentacleInfo">������Ϣ</param>
    /// <param name="grade">���ֵȼ�</param>
    public void Initialize(TentacleInfo tentacleInfo, int grade)
    {
        //������������
        this.tentacleInfo = tentacleInfo;
        damageToEnemy = tentacleInfo.damageToEnemyMultiplier * shadowInfo.Damage;
        transform.localScale = new Vector3(tentacleInfo.size, tentacleInfo.size, 1f);

        //���ݵȼ��仯�ļ��ܼ�����
        currentAttackInterval = grade == 1 ? tentacleInfo.attackInterval : 
            tentacleInfo.shorterAttackInterval;
        if (grade >= 3) OnAttack += DecreaseAttackInterval;
        // ����д�ڶ�������ϴ�����
        if (grade >= 4) OnAttack += VertigoJudge;

        attackTimer.Initialize(currentAttackInterval);
    }

    private void Attack()
    {
        DetectAttackTargets();
        foreach (var target in targets)
        {
            Vector2 attackVector = target.GetComponent<Transform>().position - transform.position;
            int direction = GainDir(attackVector);
            //���Ŷ���(Ŀǰ����ÿ�ι���ֻ����һ��Ŀ�� �Ȳ����ǹ������Ŀ��Ķ�������)

            // �����ã��������������˺�������˲�䴥����Ŀǰ���ַ����޷������������˺�ί��
            if (target as EnemyStatusInfo != null) target.GetHurt(damageToEnemy);
            else target.GetHurt(tentacleInfo.damageToPlayer);
        }
        if (targets.Count > 0)
        {
            ssr.color = Color.red;
            Invoke(nameof(TurnWhite), 0.1f);
            //Debug.Log("OnAttack");
            OnAttack?.Invoke();
        }

    }

    private void DetectAttackTargets()
    {
        List<CharacterStatusInfo> targetStatusInfos = new List<CharacterStatusInfo>();

        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, 
            tentacleInfo.attackDist, EnemyManager.enemyLayerMask);
        //��ȡ������Χ�ڵ�CharacterStatusInfo
        foreach (var targetCollider in targetColliders)
        {
            CharacterStatusInfo targetStatusInfo = targetCollider.GetComponentInParent<CharacterStatusInfo>();
            if (targetStatusInfo != null) targetStatusInfos.Add(targetStatusInfo);
        }
        targets = targetStatusInfos.RandomPick(tentacleInfo.attackNum);
    }

    //���ڻ�ȡ���ֹ�������İ���
    //4    3    2
    //�I   ��    �J

    //5��  �I   ��1

    //6    7    8
    //�L   ��    �K
    private int GainDir(Vector2 vector)
    {
        float angle = Vector2.SignedAngle(Vector2.right, vector);
        angle = angle < 0 ? (385.5f + angle) % 360 : angle + 22.5f;
        int dir = (int)(angle / 45 + 1);
        return dir;
    }

    private void DecreaseAttackInterval()
    {
        currentAttackInterval = Mathf.Max(tentacleInfo.minAttackInterval,
            currentAttackInterval * tentacleInfo.decreaseAttackIntervalMultiplier);
        //Debug.Log(currentAttackInterval);
        attackTimer.Initialize(currentAttackInterval, false);
    }

    private void VertigoJudge()
    {
        foreach (var target in targets)
        {
            EnemyStatusInfo enemyTarget = target as EnemyStatusInfo;
            if (enemyTarget != null)
            {
                float randomFloat = Random.Range(0f, 1f);
                if ((enemyTarget.IsBoss && randomFloat <= tentacleInfo.vertigoProbability_Boss) || 
                    (!enemyTarget.IsBoss && randomFloat <= tentacleInfo.vertigoProbability_NormalEnemy))
                {
                    target.GetComponent<Enemy>().GetVertigo(tentacleInfo.vertigoTime);
                }
            }
        }
    }

    public void DestroySelf()
    {
        attackTimer.Paused = true;
        OnAttack = null;
        myObject.Recycle();
    }

    private void TurnWhite()
    {
        ssr.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (tentacleInfo != null)
            Gizmos.DrawWireSphere(transform.position, tentacleInfo.attackDist);
        Gizmos.color = Color.red;

        if (targets != null && targets.Count > 0)
        {
            foreach (var target in targets)
            {
                Gizmos.DrawSphere(target.transform.position, 0.1f);
            }
        }
    }
}
