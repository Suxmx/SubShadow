using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class ShadowLine : MonoBehaviour
{
    private MyObject myObject;
    private SpriteRenderer sr;
    private BoxCollider2D cd;
    private ShadowInfo shadowInfo;

    public Transform endpoint1;
    public Transform endpoint2;
    private Vector3 endpointDir;
    private float damageMultiplier;
    private RepeatTimer damageTimer;
    private ContactFilter2D contactFilter;
    private List<Collider2D> enemies;
    private bool affectSpeed;

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<BoxCollider2D>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;

        damageTimer = new RepeatTimer();
        damageTimer.OnComplete += CauseDamage;
        contactFilter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = EnemyManager.enemyLayerMask,
        };
        enemies = new List<Collider2D>();
    }

    public void Initialize(Transform endpoint1, Transform endpoint2, 
        float damageJudgeInterval, float damageMultiplier, bool affectSpeed)
    {
        this.endpoint1 = endpoint1;
        this.endpoint2 = endpoint2;
        this.damageMultiplier = damageMultiplier;
        this.affectSpeed = affectSpeed;
        UpdatePos();
        damageTimer.Initialize(damageJudgeInterval);
    }

    private void LateUpdate()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        transform.position = (endpoint1.position + endpoint2.position) / 2;
        endpointDir = endpoint1.position - endpoint2.position;
        sr.size = cd.size = new Vector2(endpointDir.magnitude, 1f);
        transform.right = endpointDir;
    }

    private void CauseDamage()
    {
        cd.OverlapCollider(contactFilter, enemies);
        enemies.ForEach(x => x.GetComponentInParent<EnemyStatusInfo>().GetHurt(
            damageMultiplier * shadowInfo.Damage));
    }

    public void DestroySelf()
    {
        damageTimer.Paused = true;
        myObject.Recycle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (affectSpeed)
        {
            Player player = collision.GetComponentInParent<Player>();
            //if (player != null) player.AdjustSpeed(PlayerSpeedFactorType.AfterCrossShadowLine);
        }
    }
}
