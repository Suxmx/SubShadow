using MyTimer;
using UnityEngine;

public class HandHighLight : MonoBehaviour
{
    private SpriteRenderer highLightSR;
    private SpriteRenderer parentSR;
    private CountdownTimer flickerTimer;

    private void Awake()
    {
        highLightSR = GetComponent<SpriteRenderer>();
        parentSR = transform.parent.GetComponent<SpriteRenderer>();
        flickerTimer = GetComponentInParent<CharacterStatusInfo>().flickerTimer;
        flickerTimer.OnResume += _ => highLightSR.enabled = false;
        flickerTimer.OnPause += _ => highLightSR.enabled = true;
    }

    private void Update()
    {
        highLightSR.sprite = parentSR.sprite;
    }
}
