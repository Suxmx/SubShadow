using UnityEngine;
using UnityEngine.UI;

public class ShadowWallChargeUI : MonoBehaviour
{
    public Image progressImage;

    private void Awake()
    {
        if (progressImage == null)
            Debug.LogError("ShadowChargeUIµƒprogressImageŒ¥…Ë÷√");
    }

    public void SetChargeProgress(float progress)
    {
        progressImage.fillAmount = progress;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
