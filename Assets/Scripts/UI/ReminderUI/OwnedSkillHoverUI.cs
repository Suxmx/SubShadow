using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedSkillHoverUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public List<Text> skillDescriptions;
    public Text skillName;
    private readonly string[] skillDesc = new string[] { "һ�׼���", "���׼���", "���׼���", "�Ľ׼���" };

    private void Awake()
    {
        if (skillDescriptions == null || skillDescriptions.Count != 4 || skillName == null)
        {
            Debug.LogError("OwnedSkillHoverUI������δ������ȷ");
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(Skill skill, Vector3 pos)
    {
        canvasGroup.alpha = 1f;
        skillName.text = skill.skillInfo.name;
        for (int i = 0; i < 4; i++)
        {
            skillDescriptions[i].text = (skill.ownedGrade > i ? "<color='#FF4365'>" : "<color='#323232'>") + 
                skillDesc[i] + "\n" + skill.skillInfo.desc[i] + "</color>";
        }
        transform.position = pos;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
