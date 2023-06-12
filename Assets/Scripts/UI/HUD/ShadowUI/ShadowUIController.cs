using Services;
using System.Collections;
using UnityEngine;

public class ShadowUIController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Transform playerTransform;
    private ShadowManager shadowManager;
    private Camera mainCamera;
    private Coroutine co_Follow;

    public ShadowChargeUIController ShadowChargeUIController {get; private set;}
    public ShadowDirUIController ShadowDirUIController { get; private set; }
    public ShadowDirEdgeUI ShadowDirEdgeUI { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ShadowChargeUIController = GetComponentInChildren<ShadowChargeUIController>();
        ShadowDirUIController = GetComponentInChildren<ShadowDirUIController>();
        ShadowDirEdgeUI = GetComponentInChildren<ShadowDirEdgeUI>();
    }

    private void Start()
    {
        Player player = ServiceLocator.Get<Player>();
        playerTransform = player.transform;
        shadowManager = ServiceLocator.Get<ShadowManager>();
        mainCamera = Camera.main;

        player.OnPlayerActiveChange += SetActive;
    }

    public void Initialize()
    {
        if (co_Follow != null) StopCoroutine(co_Follow);
        co_Follow = StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        for (; ; )
        {
            yield return new WaitForEndOfFrame(); 
            transform.position = mainCamera.WorldToScreenPoint(playerTransform.position);

            ShadowDirEdgeUI.SetDir(Vector2.ClampMagnitude(
                mainCamera.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position,
                shadowManager.ShadowSetDist));
        }
    }

    private void SetActive(bool active)
    {
        canvasGroup.alpha = active ? 1f : 0f;
    }

    public void ResetUI()
    {
        if (co_Follow != null)
        {
            StopCoroutine(co_Follow);
            co_Follow = null;
        }
        ShadowDirUIController.ResetUI();
    }
}
