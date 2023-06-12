using UnityEngine;

public class VortexTrigger : PlayerTrigger
{
    private float multiplier;
    private bool isAttracter;
    //private bool isDisarmer;

    //public void Initialize(float multiplier, float radius, bool isDisarmer)
    //{
    //    this.multiplier = multiplier;
    //    SetRadius(radius);
    //    this.isDisarmer = isDisarmer;
    //}

    public void Initialize(float multiplier, float radius, bool isAttracter)
    {
        this.multiplier = multiplier;
        SetRadius(radius);
        this.isAttracter = isAttracter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.ActivateSpeedFactor(EnemySpeedFactorType.Vortex, multiplier);
            if (isAttracter) enemy.AddAttracter(transform);
            //if (isDisarmer) enemy.AddDisarmer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.InactivateSpeedFactor(EnemySpeedFactorType.Vortex);
            if (isAttracter) enemy.RemoveAttracter(transform);
            //if (isDisarmer) enemy.RemoveDisarmer();
        }
    }
}
