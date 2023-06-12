using UnityEngine;
using UnityEngine.UI;

public class ShadowLastingProgress : MonoBehaviour
{
    private Image lastingProgress;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private bool visible;
    public bool Visible
    {
        get => visible;
        set
        {
            if (visible != value)
            {
                visible = value;
                canvasGroup.alpha = value ? 1f : 0f;
            }
        }
    }

    private void Awake()
    {
        lastingProgress = GetComponent<Image>();
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        visible = true;
    }

    public void UpdateProgress(float progress)
    {
        lastingProgress.fillAmount = 1 - progress;
    }

    public void SetSortingOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}
