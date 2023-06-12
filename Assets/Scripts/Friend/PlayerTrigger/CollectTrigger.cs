using Services;
using UnityEngine;


public class CollectTrigger : MonoBehaviour
{
    private EventSystem eventSystem;
    private UnityEngine.Rendering.Universal.Light2D collectLight;

    private void Start()
    {
        eventSystem = ServiceLocator.Get<EventSystem>();
        collectLight=transform.Find("CollectAreaLight").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale * 2, scale * 2, 1f);
        collectLight.pointLightOuterRadius=scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Prop>(out var prop))
        {
            if (prop.Collectable)
            {
                prop.FollowAndRecycle(transform);
            }
        }
    }
}
