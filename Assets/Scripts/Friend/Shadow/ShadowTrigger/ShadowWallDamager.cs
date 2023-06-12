using UnityEngine;

public class ShadowWallDamager : MonoBehaviour, IDamager
{
    private BoxCollider2D damageCollider;

    public float Damage { get; private set; }

    public void Initialize(float sizeX)
    {
        damageCollider = GetComponent<BoxCollider2D>();
        damageCollider.size = new Vector2(sizeX, damageCollider.size.y);
    }

    public void SetDamageOn(float damage)
    {
        damageCollider.enabled = true;
        Damage = damage;
    }

    public void SetDamageOff()
    {
        damageCollider.enabled = false;
    }
}
