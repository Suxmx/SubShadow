using MyTimer;
using Services;
using UnityEngine;

/// <summary>
/// 代表了一个UI的功能模块
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIController : MonoBehaviour
{
    protected EventSystem eventSystem;

    protected CanvasGroup canvasGroup;
    protected float fadeTime = 0.5f;
    protected EaseFloatTimer alphaTimer;
    protected float alpha_default;

    protected bool visible;
    public virtual bool Visible
    {
        get => visible;
        set
        {
            if (value != visible)
            {
                visible = value;
                canvasGroup.interactable = visible;
                canvasGroup.blocksRaycasts = visible;
                float target = visible ? alpha_default : 0f;
                alphaTimer.Initialize(canvasGroup.alpha, target, fadeTime);
            }
        }
    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UIControllerLocator.Register(this);
        DontDestroyOnLoad(gameObject);

        alphaTimer = new EaseFloatTimer(true, EaseType.OutCubic);
        alphaTimer.OnTick += x => canvasGroup.alpha = x;
        alpha_default = canvasGroup.alpha;
        visible = true;
    }

    protected virtual void Start()
    {
        eventSystem = ServiceLocator.Get<EventSystem>();
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
    }

    protected virtual void BeforeLoadScene(int _)
    {
        Visible = false;
    }
}
