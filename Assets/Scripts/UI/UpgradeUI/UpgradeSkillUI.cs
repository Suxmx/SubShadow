using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image frameImage;
    public Image skillIcon;
    public Text skillName;
    public Text skillDescription;
    public List<UpgradeSkillGradeUI> skillGrades;
    // 两个Sprite分别代表空技能和拥有技能的标志
    public List<Sprite> skillGradeSprites;

    public Skill skill;
    private UIScaleExpander scaleExpander;
    private UpgradeUIController upgradeUIController;

    private void Awake()
    {
        if (skillIcon == null || skillName == null || skillDescription == null ||
            skillGrades == null || skillGrades.Count < 4 || 
            skillGradeSprites == null || skillGradeSprites.Count < 2)
            Debug.LogError("UpgradeSkillUI的外部引用未设置正确");
        frameImage = GetComponent<Image>();
        frameImage.enabled = false;
        upgradeUIController = GetComponentInParent<UpgradeUIController>();
        scaleExpander = new UIScaleExpander(transform);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(SelectSkill);
    }

    public void Initialize(Skill skill)
    {
        this.skill = skill;
        SkillInfo skillInfo = skill.skillInfo;
        skillIcon.sprite = skillInfo.icon;
        skillName.text = skillInfo.name;
        skillDescription.text = skillInfo.desc[Mathf.Min(skill.ownedGrade, 3)];
        for (int i = 0; i < 4; i++)
        {
            skillGrades[i].Initialize(skillInfo.desc[i], 
                skillGradeSprites[i < skill.ownedGrade ? 1 : 0]);
        }
    }

    private void SelectSkill()
    {
        if (upgradeUIController.currentSkillUI == this)
        {
            frameImage.enabled = false;
            upgradeUIController.currentSkillUI = null;
        }
        else
        {
            frameImage.enabled = true;
            if (upgradeUIController.currentSkillUI != null)
            {
                upgradeUIController.currentSkillUI.DeselectSkill();
            }
            upgradeUIController.currentSkillUI = this;
        }
    }

    public void DeselectSkill()
    {
        frameImage.enabled = false;
        scaleExpander.ShrinkScale();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleExpander.ExpandScale();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (upgradeUIController.currentSkillUI != this)
            scaleExpander.ShrinkScale();
    }

    public void ResetUI()
    {
        scaleExpander.ResetScale();
        skillGrades.ForEach(x => x.ResetUI());
        frameImage.enabled = false;
    }
}
