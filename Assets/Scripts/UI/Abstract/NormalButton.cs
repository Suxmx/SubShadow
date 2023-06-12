using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NormalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Button button;
    protected UIScaleExpander scaleExpander;
    protected Text text;
    public Color normalColor = new(251f / 255f, 80f / 255f, 96f / 255f);
    public Color enterColor = new(243f / 255f, 212f / 255f, 191f / 255f);

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        scaleExpander = new UIScaleExpander(transform, 1f, 1.1f);
        text = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleExpander.ExpandScale();
        text.color = enterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleExpander.ShrinkScale();
        text.color = normalColor;
    }

    public void ResetButton()
    {
        scaleExpander.ResetScale();
        text.color = normalColor;
    }
}
