using Services;
using UnityEngine;

// ����EnemyCollider����ײ���仯��Ӱ���˺��Ĵ����ͳ��ܣ��ʵ������ô����������˺�
public class EnemyDamageTrigger : MonoBehaviour
{
    protected Enemy enemy;
    protected EnemyStatusInfo enemyStatusInfo;
    protected BeatInfo beatInfo;
    protected EventSystem eventSystem;
    protected PlayerStatusInfo playerStatusInfo;

    protected virtual void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyStatusInfo = GetComponentInParent<EnemyStatusInfo>();
        EnemyInfoData enemyInfoData = ServiceLocator.Get<EnemyManager>().EnemyInfoData;
        beatInfo = new BeatInfo(enemyInfoData.hitBeatDist, enemyInfoData.hitBeatSpeed, false, true);
        eventSystem = ServiceLocator.Get<EventSystem>();
        playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Friend"))
        {
            if (collision.TryGetComponent<IDestroySelf>(out var destroyer))
            {
                destroyer.DestroySelf();
            }
            if (collision.TryGetComponent<IDamager>(out var damager))
            {
                float damage = damager.Damage;
                if (damage <= 0f) return;
                eventSystem.Invoke(EEvent.OnHurtEnemy, enemyStatusInfo, damager);

                // ��IDamagerΪӰ���ٻ��˺�ʱ�����˺�ֵ��������
                ShadowDamager shadowDamager = damager as ShadowDamager;
                if (shadowDamager != null && shadowDamager.shadow.ShadowState == ShadowState.Recalling)
                {
                    if (shadowDamager.shadowInfo.scaleDamageMultiplier > 0)
                    {
                        damage *= shadowDamager.shadow.Size * shadowDamager.shadowInfo.scaleDamageMultiplier;
                    }
                    if (shadowDamager.shadowInfo.gradeMultiplier > 0)
                    {
                        damage += playerStatusInfo.PlayerGrade * shadowDamager.shadowInfo.gradeMultiplier * enemyStatusInfo.MaxHP;
                    }
                }

                enemyStatusInfo.GetHurt(damage);

                if (enemyStatusInfo.IsAlive)
                {
                    // �ܵ��˺�ʱ������һС�ξ���
                    // ����IMovable���У���Ӧ������MoveDir��ֱ�ķ�����ˣ�����ֱ����Զ����ײ�������
                    Vector3 beatDir = transform.position - collision.transform.position;
                    if (collision.TryGetComponent<IMovable>(out var mover))
                    {
                        beatDir -= Vector3.Project(beatDir, mover.MoveDir);
                    }
                    beatInfo.beatVec = beatDir.normalized * beatInfo.beatDist;
                    enemy.GetBeaten(beatInfo);
                }
            }
        }
    }
}
