using UnityEngine;
using UnityEngine.UI;

public class ShadowWallChargeUI : MonoBehaviour
{
    public Image progressImage;

    private void Awake()
    {
        if (progressImage == null)
            Debug.LogError("ShadowChargeUI��progressImageδ����");
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
