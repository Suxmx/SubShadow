using UnityEngine;

public class ExtendTimeTrigger : PlayerTrigger
{
    private ShadowLastingTimer attachTimer;
    private float extendTime;
    private bool added;
    private bool playerIn;

    public void Initialize(ShadowLastingTimer timer, float extendTime, float radius)
    {
        attachTimer = timer;
        this.extendTime = extendTime;
        SetRadius(radius);
        added = false;
        playerIn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCollider>() != null)
        {
            playerIn = true;
            if (!added)
            {
                attachTimer.AddConditionalLastingTime(extendTime, () => playerIn);
                added = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCollider>() != null)
        {
            playerIn = false;
        }
    }
}
