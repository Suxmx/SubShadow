using MyTimer;
using Services;
using UnityEngine;

public class Whirl : MonoBehaviour, IDamager
{
    private WhirlInfo whirlInfo;
    private MyObject myObject;
    private Vector3 defaultScale;
    private Vector3 targetScale;
    private EaseFloatTimer generateTimer;

    public float Damage => whirlInfo.whirlDamage;

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        defaultScale = transform.localScale;

        generateTimer = new EaseFloatTimer(EaseType.OutQuad);
        generateTimer.OnTick += x => transform.localScale = new Vector3(x * targetScale.x, targetScale.y, 1f);

        myObject.OnRecycle += OnRecycle;
    }

    private void Update()
    {
        if (generateTimer.Completed)
        {
            transform.Rotate(0f, 0f, whirlInfo.whirlAngularVelocity * Time.deltaTime);
        }
    }

    public void Initialize(WhirlInfo whirlInfo, float angle, Shadow shadow)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        targetScale = defaultScale / shadow.Size;
        transform.localScale = new Vector3(targetScale.x, 0f, 1f);
        generateTimer.Initialize(0f, whirlInfo.whirlLength, 0.2f);
        this.whirlInfo = whirlInfo;
    }

    private void OnRecycle()
    {
        generateTimer.Paused = true;
    }

    public void DestroySelf()
    {
        myObject.Recycle();
    }
}
