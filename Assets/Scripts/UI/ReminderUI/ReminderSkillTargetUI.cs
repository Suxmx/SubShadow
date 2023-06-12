using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReminderSkillTargetUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public OwnedSkillHoverUI ownedSkillHoverUI;
    public Skill skill;
    private Vector3 offset;
    //private float offset;
    //private StringBuilder stringBuilder;

    private void Awake()
    {
        offset = new Vector3(transform.parent.GetComponent<RectTransform>().offsetMax.x, 0);
        //offset = transform.parent.GetComponent<RectTransform>().offsetMax.x;
        //stringBuilder = new StringBuilder();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //stringBuilder.Clear();
        //stringBuilder.Append(skill.skillInfo.name);
        //for (int i = 0; i < 4; i++)
        //{
        //    stringBuilder.Append(skill.ownedGrade > i ? "\n<color='#FF4365'>" : "\n<color='#323232'>");
        //    stringBuilder.Append(skillDesc[i]);
        //    stringBuilder.Append("\n");
        //    stringBuilder.Append(skill.skillInfo.desc[i]);
        //    stringBuilder.Append("</color>");
        //}
        ownedSkillHoverUI.Show(skill, transform.position + offset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ownedSkillHoverUI.Hide();
    }
}
