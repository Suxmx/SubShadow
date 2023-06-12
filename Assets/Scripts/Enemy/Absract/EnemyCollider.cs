using MyTimer;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    protected Enemy enemy;
    protected CircleCollider2D bodyCollider;

    protected EaseFloatTimer colliderTimer;

    protected virtual void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        bodyCollider = GetComponent<CircleCollider2D>();

        colliderTimer = new EaseFloatTimer();
        colliderTimer.OnTick += x => bodyCollider.radius = x;
        colliderTimer.Initialize(0f, bodyCollider.radius, 1f, false);
    }

    public void EnableCollider(bool forceChange = false)
    {
        if (!forceChange)
        {
            if (bodyCollider.enabled) return;
            // 若当前位置有其他敌人，其他敌人也应该变化碰撞体大小以避免突然卡位
            EnemyManager.CheckAndChangeEnemyCollider(transform.position);
        }
        // 碰撞体大小从0开始缓慢变化以避免突然卡位
        bodyCollider.enabled = true;
        colliderTimer.Restart();
    }

    public void DisenableCollider()
    {
        bodyCollider.enabled = false;
        colliderTimer.Paused = true;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy otherEnemy = collision.collider.GetComponentInParent<Enemy>();
        if (otherEnemy != null) enemy.AddLinkedEnemy(otherEnemy);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        Enemy otherEnemy = collision.collider.GetComponentInParent<Enemy>();
        if (otherEnemy != null) enemy.RemoveLinkedEnemy(otherEnemy);
    }
}
