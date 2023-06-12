using Cinemachine;
using Services;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    public CinemachineVirtualCamera VirtualCamera { get; private set; }

    private void Awake()
    {
        VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Player player = ServiceLocator.Get<Player>();

        player.PlayerStatusInfo.BeforeGetHurt += ShakeScreen;
        VirtualCamera.Follow = player.transform;

        EventSystem eventSystem = ServiceLocator.Get<EventSystem>();
    }

    public void ShakeScreen()
    {
        impulseSource.GenerateImpulse();
    }

    public void SetOrthoSize(float size)
    {
        VirtualCamera.m_Lens.OrthographicSize = size;
    }
}
