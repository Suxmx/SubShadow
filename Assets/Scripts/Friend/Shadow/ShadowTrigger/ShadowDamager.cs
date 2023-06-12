using Services;
using UnityEngine;

public class ShadowDamager : MonoBehaviour, IDamager, IMovable
{
    public Shadow shadow;
    public ShadowInfo shadowInfo;
    private Collider2D damageCollider;

    private float damageMultiplier;
    public float Damage => shadowInfo.Damage * damageMultiplier;

    public Vector3 MoveDir => shadow.MoveDir;

    private void Awake()
    {
        shadow = GetComponentInParent<Shadow>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        damageCollider = GetComponent<Collider2D>();
    }

    public void SetDamageOn(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
        damageCollider.enabled = true;
    }

    public void SetDamageOff()
    {
        damageCollider.enabled = false;
    }
}
