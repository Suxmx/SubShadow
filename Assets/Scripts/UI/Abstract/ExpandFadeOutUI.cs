using MyTimer;
using UnityEngine;
using UnityEngine.UI;

public class ExpandFadeOutUI : MonoBehaviour
{
    private Image image;
    private EaseFloatTimer fadeOutTimer;
    private UIScaleExpander scaleExpander;
    [SerializeField]
    private float fadeDuration = 0.2f;
    [SerializeField]
    private float expandScale = 1.5f;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        fadeOutTimer = new EaseFloatTimer();
        fadeOutTimer.Initialize(1f, 0f, fadeDuration, false);
        fadeOutTimer.OnTick += FadeOut;
        scaleExpander = new UIScaleExpander(transform, 1f, expandScale, fadeDuration, EaseType.Linear);
    }

    private void FadeOut(float current)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, current);
    }

    public void ExpandAndFadeOut()
    {
        FadeOut(1f);
        fadeOutTimer.Restart();
        scaleExpander.ExpandScale();
    }

    public void ResetUI()
    {
        fadeOutTimer.Paused = true;
        scaleExpander.ResetScale();
    }
}
