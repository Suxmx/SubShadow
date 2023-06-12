using UnityEngine;

public class VertigoTrigger : InstantaneousTrigger
{
    private float vertigoDuration;
    private bool effectiveForBoss;

    public void Initialize(float vertigoDuration, float vertigoRadius, bool effectiveForBoss = false)
    {
        this.vertigoDuration = vertigoDuration;
        this.effectiveForBoss = effectiveForBoss;
        SetRadius(vertigoRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null && enemy.IsAlive && (!enemy.IsBoss || effectiveForBoss))
        {
            enemy.GetVertigo(vertigoDuration);
        }
    }
}
