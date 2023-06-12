using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSkillGradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private UpgradeSkillHoverUI skillHoverUI;
    private string gradeDescription;
    private UIScaleExpander scaleExpander;

    private void Start()
    {
        image = GetComponent<Image>();
        UpgradeUIController upgradeUIController = GetComponentInParent<UpgradeUIController>();
        skillHoverUI = upgradeUIController.UpgradeSkillHoverUI;
        scaleExpander = new UIScaleExpander(transform);
    }

    public void Initialize(string gradeDescription, Sprite sprite)
    {
        this.gradeDescription = gradeDescription;
        image.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleExpander.ExpandScale();
        skillHoverUI.Show(gradeDescription, transform.position.x);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleExpander.ShrinkScale();
        skillHoverUI.Hide();
    }

    public void ResetUI() => scaleExpander.ResetScale();
}
