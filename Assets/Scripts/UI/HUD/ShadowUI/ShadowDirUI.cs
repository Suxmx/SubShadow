using Services;
using UnityEngine;
using UnityEngine.UI;

public class ShadowDirUI : MonoBehaviour
{
    public Image progressImage;
    private MyObject myObject;
    private ExpandFadeOutUI expandFadeOutUI;
    private Transform playerTransform;
    [SerializeField, Label("æ‡ÕÊº“œÒÀÿæ‡¿Î")]
    private float distToPlayer;

    private Transform shadowTransform;
    private ShadowLastingTimer shadowTimer;
    private Vector3 dirToShadow;

    private void Awake()
    {
        if (progressImage == null)
            Debug.LogError("ShadowDirUIµƒprogressImageŒ¥…Ë÷√");
        myObject = GetComponent<MyObject>();
        expandFadeOutUI = GetComponentInChildren<ExpandFadeOutUI>();
        playerTransform = ServiceLocator.Get<Player>().transform;
    }

    public void Initialize(Shadow shadow)
    {
        shadowTransform = shadow.centerTransform.transform;
        shadowTimer = shadow.lastingTimer;
        shadowTimer.OnComplete += ExpandAndFadeOut;
    }

    private void LateUpdate()
    {
        SetPosAndRot();
        SetLastingProgress();
    }

    private void SetPosAndRot()
    {
        dirToShadow = shadowTransform.position - playerTransform.position;
        if (dirToShadow.sqrMagnitude > 0.01f)
        {
            transform.localPosition = dirToShadow.normalized * distToPlayer;
            transform.right = dirToShadow;
        }
    }

    private void SetLastingProgress()
    {
        progressImage.fillAmount = shadowTimer.Current;
    }

    private void ExpandAndFadeOut()
    {
        expandFadeOutUI.ExpandAndFadeOut();
    }

    public void DestroySelf()
    {
        shadowTransform = null;
        shadowTimer.OnComplete -= ExpandAndFadeOut;
        shadowTimer = null;
        expandFadeOutUI.ResetUI();
        myObject.Recycle();
    }
}
