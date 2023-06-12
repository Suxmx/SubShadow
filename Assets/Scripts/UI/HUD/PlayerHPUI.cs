using MyTimer;
using Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerHPUIType
{
    /// <summary>
    /// 空生命值
    /// </summary>
    [Label("空生命值")]
    Blank,
    /// <summary>
    /// 有生命值
    /// </summary>
    [Label("有生命值")]
    Fill,
    /// <summary>
    /// 不稳定生命值
    /// </summary>
    [Label("不稳定生命值")]
    Unstable,
    /// <summary>
    /// 空的延迟伤害待扣生命值
    /// </summary>
    [Label("空的延迟伤害待扣生命值")]
    BlankDelayed,
    /// <summary>
    /// 延迟伤害待扣生命值
    /// </summary>
    [Label("延迟伤害待扣生命值")]
    Delayed,
    /// <summary>
    /// 延迟伤害待扣的不稳定生命值
    /// </summary>
    [Label("延迟伤害待扣的不稳定生命值")]
    DelayedUnstable,
}

public class PlayerHPUI : MonoBehaviour
{
    private Image backgroundImage, image;
    private MyObject myObject;
    private Dictionary<PlayerHPUIType, Sprite> sprites;
    public CountdownPercentTimer delayTimer;

    private PlayerHPUIType playerHPUIType;
    public PlayerHPUIType PlayerHPUIType
    {
        get => playerHPUIType;
        set
        {
            playerHPUIType = value;
            image.sprite = sprites[playerHPUIType];
            SetFillAmount(1f);
            delayTimer = null;
            backgroundImage.sprite = playerHPUIType switch
            {
                PlayerHPUIType.BlankDelayed or PlayerHPUIType.Delayed or PlayerHPUIType.DelayedUnstable => sprites[PlayerHPUIType.BlankDelayed],
                _ => sprites[PlayerHPUIType.Blank],
            };
        }
    }

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        image = GetComponentsInChildren<Image>().First(x => x.transform != transform);
        myObject = GetComponent<MyObject>();
        sprites = UIControllerLocator.Get<HUD>().PlayerHPUIController.sprites;
    }

    public void SetFillAmount(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }

    public void DestroySelf()
    {
        myObject.Recycle();
    }
}
