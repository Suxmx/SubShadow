using MyTimer;
using Services;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeSeed : MonoBehaviour
{
    private MyObject myObject;
    private CountdownTimer stayTimer;
    private UnityAction<Vector3> CauseExplode;

    private void Awake()
    {
        myObject = GetComponent<MyObject>();
        stayTimer = new CountdownTimer();
        stayTimer.OnComplete += myObject.Recycle;

        myObject.OnRecycle += () => stayTimer.Paused = true;
    }

    public void Initialize(float stayingTime, UnityAction<Vector3> CauseExplode)
    {
        stayTimer.Initialize(stayingTime);
        this.CauseExplode = CauseExplode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CauseExplode(transform.position);
            myObject.Recycle();
        }
    }
}
