using Services;
using UnityEngine;
using UnityEngine.UI;

public class ShadowDirEdgeUI : MonoBehaviour
{
    private Image image;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public Sprite edgeSprite1, edgeSprite2;
    public float width1, width2;

    private void Start()
    {
        if (edgeSprite1 == null || edgeSprite2 == null)
        {
            Debug.LogError("ShadowDieEdgeUI的Sprite引用未设置正确！");
        }
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        ShadowManager shadowManager = ServiceLocator.Get<ShadowManager>();
        shadowManager.OnSetShadowDistChange += OnSetShadowDistChange;
        shadowManager.shadowChargeTimer.OnShadowChargedCountChange += OnShadowChargeCountChange;

        GameManager gameManager = ServiceLocator.Get<GameManager>();
        gameManager.BeforeGamePause += BeforeGamePause;
        gameManager.AfterGameResume += AfterGameResume;
    }

    private void OnSetShadowDistChange(float setShadowDist)
    {
        if (Mathf.Approximately(setShadowDist, 2.5f))
        {
            image.sprite = edgeSprite1;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width1);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width1);
        }
        else if (Mathf.Approximately(setShadowDist, 4.5f))
        {
            image.sprite = edgeSprite2;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width2);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width2);
        }
        //transform.localScale = new Vector3(setShadowDist, setShadowDist, 1f);
    }

    private void OnShadowChargeCountChange(int _, int count)
    {
        image.color = count > 0 ? Color.white : Color.gray;
    }

    public void SetDir(Vector3 dirVec)
    {
        if (dirVec.magnitude > 0.01f)
        {
            transform.right = dirVec;
        }
    }
    private void BeforeGamePause()
    {
        canvasGroup.alpha = 0f;
    }

    private void AfterGameResume()
    {
        canvasGroup.alpha = 1f;
    }
}
