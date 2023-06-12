using Services;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamager
{
    protected MyObject myObject;
    protected EnemyManager enemyManager;

    protected float speed;
    protected bool destroyed;
    protected float moveDist;
    protected float maxFlyingDist;

    public float Damage { get; protected set; }

    protected virtual void Awake()
    {
        myObject = GetComponent<MyObject>();
        enemyManager = ServiceLocator.Get<EnemyManager>();

        myObject.OnActivate += OnActivate;
    }

    protected virtual void OnActivate()
    {
        destroyed = false;
        moveDist = 0f;
    }

    public virtual void Initialize(Vector3 shootDir, float bulletDamage, float bulletSpeed, float maxFlyingDist)
    {
        transform.right = shootDir;
        Damage = bulletDamage;
        speed = bulletSpeed;
        this.maxFlyingDist = maxFlyingDist;
    }

    protected virtual void Update()
    {
        if (!destroyed)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        moveDist += speed * Time.deltaTime;
        if (moveDist > maxFlyingDist) myObject.Recycle();
    }

    public void DestroySelf()
    {
        if (destroyed) return;
        destroyed = true;
        OnDestroySelf();
    }

    protected virtual void OnDestroySelf()
    {
        myObject.Recycle();
    }
}
