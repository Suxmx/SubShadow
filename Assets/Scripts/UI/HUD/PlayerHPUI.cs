using MyTimer;
using Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerHPUIType
{
    /// <summary>
    /// ������ֵ
    /// </summary>
    [Label("������ֵ")]
    Blank,
    /// <summary>
    /// ������ֵ
    /// </summary>
    [Label("������ֵ")]
    Fill,
    /// <summary>
    /// ���ȶ�����ֵ
    /// </summary>
    [Label("���ȶ�����ֵ")]
    Unstable,
    /// <summary>
    /// �յ��ӳ��˺���������ֵ
    /// </summary>
    [Label("�յ��ӳ��˺���������ֵ")]
    BlankDelayed,
    /// <summary>
    /// �ӳ��˺���������ֵ
    /// </summary>
    [Label("�ӳ��˺���������ֵ")]
    Delayed,
    /// <summary>
    /// �ӳ��˺����۵Ĳ��ȶ�����ֵ
    /// </summary>
    [Label("�ӳ��˺����۵Ĳ��ȶ�����ֵ")]
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
