using Services;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    protected MyObject myObject;

    protected virtual void Awake()
    {
        myObject = GetComponent<MyObject>();

        myObject.OnActivate += OnActivate;
        myObject.OnRecycle += OnRecycle;
    }

    protected virtual void OnActivate() { }

    public virtual void SetRadius(float radius)
    {
        transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
    }

    public virtual void DestroySelf()
    {
        myObject.Recycle();
    }

    protected virtual void OnRecycle() { }
}
