using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Lock
{
    [SerializeField, Label("锁的值")]
    private int value;

    public bool Unlocked => value <= 0;

    public event UnityAction OnLock;
    public event UnityAction OnUnlock;

    public Lock(int value, UnityAction OnLock = null, UnityAction OnUnlock = null)
    {
        this.value = value;
        this.OnLock = OnLock;
        this.OnUnlock = OnUnlock;
    }

    public Lock(UnityAction OnLock = null, UnityAction OnUnlock = null) : this(0, OnLock, OnUnlock) { }

    public void Reset(bool clearEvent = false)
    {
        value = 0;
        if (clearEvent)
        {
            OnLock = null;
            OnUnlock = null;
        }
    }

    public static Lock operator ++(Lock _lock)
    {
        _lock.value++;
        if (_lock.value == 1)
            _lock.OnLock?.Invoke();
        return _lock;
    }

    public static Lock operator --(Lock _lock)
    {
        _lock.value--;
        if (_lock.value == 0)
            _lock.OnUnlock?.Invoke();
        else if (_lock.value < 0)
            Debug.LogWarning("锁的值不应降低到0以下");
        return _lock;
    }
}
