using UnityEngine;
using UnityEngine.UI;

public class ShadowChargeUI : MonoBehaviour
{
    public Image progressImage;
    private ExpandFadeOutUI expandFadeOutUI;

    public bool Charged => progressImage.fillAmount >= 1f;

    private void Awake()
    {
        if (progressImage == null)
            Debug.LogError("ShadowChargeUIµƒprogressImageŒ¥…Ë÷√");
        expandFadeOutUI = GetComponentInChildren<ExpandFadeOutUI>();
    }

    public void SetChargeProgress(float progress)
    {
        progressImage.fillAmount = progress;
    }

    public void ExpandAndFadeOut()
    {
        expandFadeOutUI.ExpandAndFadeOut();
    }

    public void DestroySelf()
    {
        expandFadeOutUI.ResetUI();
        Destroy(gameObject);
    }
}
