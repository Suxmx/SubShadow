using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkillHoverUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Text skillDescription;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        skillDescription = GetComponentInChildren<Text>();
    }

    public void Show(string description, float posX)
    {
        canvasGroup.alpha = 1f;
        skillDescription.text = description;
        transform.position = new Vector3(posX, transform.position.y, 0f);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
