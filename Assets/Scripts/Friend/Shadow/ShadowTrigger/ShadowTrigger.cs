using Services;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    private Shadow shadow;
    private EventSystem eventSystem;

    private void Awake()
    {
        shadow = GetComponentInParent<Shadow>();
        eventSystem = ServiceLocator.Get<EventSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventSystem.Invoke(EEvent.OnShadowTrigger, shadow, collision);
    }
}
