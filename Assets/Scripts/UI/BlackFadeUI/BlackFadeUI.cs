using MyTimer;
using Services;
using UnityEngine;

// �����л�����ʱ���뵭���ĺ�ɫ����UI��ע����GameLauncher��needKeyConfirm���ܹ���
public class BlackFadeUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private SceneController sceneController;
    private EventSystem eventSystem;
    private EaseFloatTimer fadeOutTimer;
    private EaseFloatTimer fadeInTimer;
    private AsyncOperation asyncOperation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        canvasGroup = GetComponent<CanvasGroup>();
        sceneController = ServiceLocator.Get<SceneController>();
        eventSystem = ServiceLocator.Get<EventSystem>();

        canvasGroup.alpha = 1.0f;

        fadeInTimer = new EaseFloatTimer();
        fadeInTimer.OnTick += x => canvasGroup.alpha = x;
        fadeInTimer.OnComplete += () => asyncOperation.allowSceneActivation = true;
        fadeOutTimer = new EaseFloatTimer();
        fadeOutTimer.OnTick += x => canvasGroup.alpha = x;

        sceneController.AsyncLoadScene += x => asyncOperation = x;

        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, _ => fadeInTimer.Initialize(canvasGroup.alpha, 1f, 0.5f));
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, _ => fadeInTimer.Initialize(canvasGroup.alpha, 0f, 0.5f));
    }
}
