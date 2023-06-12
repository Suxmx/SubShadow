using MyTimer;
using UnityEngine;

public class SelfExplosionDamager : MonoBehaviour
{
    private CountdownTimer damageTimer;

    public bool Active
    {
        get => gameObject.activeSelf;
        set
        {
            if (gameObject.activeSelf != value)
            {
                gameObject.SetActive(value);
                if (value)
                {
                    damageTimer.Restart();
                }
            }
        }
    }

    public void Init(SelfExplosionEnemyInfo selfExplosionEnemyInfo)
    {
        GetComponentInChildren<SelfExplosionDamagerToPlayer>().Damage = selfExplosionEnemyInfo.damageToPlayer;
        GetComponentInChildren<SelfExplosionDamagerToEnemy>().Damage = selfExplosionEnemyInfo.damageToEnemy;
        transform.localScale = new Vector3(selfExplosionEnemyInfo.explosionRadius * 2, 
            selfExplosionEnemyInfo.explosionRadius * 2, 1f);
        damageTimer = new CountdownTimer();
        damageTimer.OnComplete += () => Active = false;
        damageTimer.Initialize(0.4f, false);
    }

    public void ResetDamager()
    {
        Active = false;
        damageTimer.Paused = true;
    }
}
