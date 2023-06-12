using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPUIController : MonoBehaviour
{
    // 六个Sprite分别代表：
    // 空生命值、有生命值、不稳定生命值、
    // 延迟伤害待扣的生命值、延迟伤害待扣的不稳定生命值、空的延迟伤害待扣的生命值
    public Dictionary<PlayerHPUIType, Sprite> sprites;
    private List<PlayerHPUI> playerHPUIs;
    private ObjectManager objectManager;

    private void Awake()
    {
        sprites = new Dictionary<PlayerHPUIType, Sprite>();
        PlayerHPUITypeSpritePair[] playerHPUITypeSpritePairs =
            Resources.Load<PlayerHPUIData>("PlayerHPUIData").playerHPUITypeSpritePairs;
        foreach (var pair in playerHPUITypeSpritePairs)
        {
            sprites.Add(pair.playerHPUIType, pair.sprite);
        }
        playerHPUIs = new List<PlayerHPUI>();
    }

    private void Start()
    {
        objectManager = ServiceLocator.Get<ObjectManager>();

        PlayerStatusInfo playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo;
        playerStatusInfo.OnChangeMaxHP += OnChangeMaxHP;
        playerStatusInfo.OnChangeHP += OnChangeHP;
        playerStatusInfo.OnGetDelayedHurt += OnGetDelayedHurt;
        playerStatusInfo.OnRemoveAllDelayedHurt += OnRemoveAllDelayedHurt;
        playerStatusInfo.OnChangeUnstableHP += OnChangeUnstableHP;
    }

    private void OnChangeMaxHP(float before, float after)
    {
        if (before < after)
        {
            int addCount = Mathf.RoundToInt(after - before);
            for (int i = 0; i < addCount; i++)
            {
                Transform playerHPUITransform = objectManager.Activate(EObject.PlayerHPUI,
                    Vector3.zero, transform).Transform;
                playerHPUITransform.localScale = Vector3.one;
                playerHPUITransform.SetSiblingIndex(Mathf.RoundToInt(before) + i);
                PlayerHPUI playerHPUI = playerHPUITransform.GetComponent<PlayerHPUI>();
                playerHPUI.PlayerHPUIType = PlayerHPUIType.Blank;
                playerHPUIs.Insert(Mathf.RoundToInt(before) + i, playerHPUI);
            }
        }
        else
        {
            int destroyIndex = Mathf.RoundToInt(after);
            int removeCount = Mathf.RoundToInt(before - after);
            while (removeCount-- > 0)
            {
                playerHPUIs[destroyIndex].DestroySelf();
                playerHPUIs.RemoveAt(destroyIndex);
            }
        }
    }

    private void OnChangeHP(float before, float after)
    {
        if (before < after)
        {
            int beforeIndex = Mathf.RoundToInt(before) - 1;
            int afterIndex = Mathf.RoundToInt(after) - 1;
            int i;
            for (i = beforeIndex; i >= 0; i--)
            {
                if (playerHPUIs[i].PlayerHPUIType != PlayerHPUIType.Delayed) break;
            }
            int delayCount = beforeIndex - i;
            for (++i; i <= afterIndex - delayCount; i++)
            {
                playerHPUIs[i].PlayerHPUIType = PlayerHPUIType.Fill;
            }
            for (; i <= afterIndex; i++)
            {
                playerHPUIs[i].PlayerHPUIType = PlayerHPUIType.Delayed;
            }
        }
        else
        {
            int maxHPCount = Mathf.Min(Mathf.RoundToInt(before), playerHPUIs.Count);
            for (int i = Mathf.RoundToInt(after); i < maxHPCount; i++)
            {
                if (playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Fill ||
                    playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Delayed)
                    playerHPUIs[i].PlayerHPUIType = PlayerHPUIType.Blank;
            }
        }
    }

    private void OnGetDelayedHurt(DelayDamage delayDamage)
    {
        int delayCount = Mathf.RoundToInt(delayDamage.damage);
        if (delayCount == 0) return;
        for (int i = playerHPUIs.Count - 1; i >= 0; i--)
        {
            if (playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Fill || 
                playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Unstable)
            {
                playerHPUIs[i].PlayerHPUIType = 
                    playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Fill ?
                    PlayerHPUIType.Delayed : PlayerHPUIType.DelayedUnstable;
                delayDamage.delayTimer.OnTick += playerHPUIs[i].SetFillAmount;
                playerHPUIs[i].delayTimer = delayDamage.delayTimer;
                if (--delayCount == 0) break;
            }
        }
    }

    private void OnRemoveAllDelayedHurt()
    {
        for (int i = playerHPUIs.Count - 1; i >= 0; i--)
        {
            if (playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Fill) break;
            if (playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.Delayed)
                playerHPUIs[i].PlayerHPUIType = PlayerHPUIType.Fill;
            else if (playerHPUIs[i].PlayerHPUIType == PlayerHPUIType.DelayedUnstable)
                playerHPUIs[i].PlayerHPUIType = PlayerHPUIType.Unstable;
        }
    }

    private void OnChangeUnstableHP(float before, float after)
    {
        if (before < after)
        {
            int addCount = Mathf.RoundToInt(after - before);
            int addIndex = playerHPUIs.Count - Mathf.RoundToInt(before);
            int getDelayedIndex = 0;
            while (addCount-- > 0)
            {
                Transform playerHPUITransform = objectManager.Activate(EObject.PlayerHPUI,
                    Vector3.zero, transform).Transform;
                playerHPUITransform.localScale = Vector3.one;
                playerHPUITransform.SetSiblingIndex(addIndex);
                PlayerHPUI playerHPUI = playerHPUITransform.GetComponent<PlayerHPUI>();
                playerHPUI.PlayerHPUIType = PlayerHPUIType.Unstable;

                // 若存在延迟伤害待扣的生命值，将其转化为蓝色的延迟伤害待扣的不稳定生命值
                while (getDelayedIndex < addIndex)
                {
                    if (playerHPUIs[getDelayedIndex].PlayerHPUIType == PlayerHPUIType.Delayed)
                    {
                        // 将delayTimer.OnTick控制的对象进行转移
                        CountdownPercentTimer delayTimer = playerHPUIs[getDelayedIndex].delayTimer;
                        delayTimer.OnTick -= playerHPUIs[getDelayedIndex].SetFillAmount;

                        playerHPUIs[getDelayedIndex].PlayerHPUIType = PlayerHPUIType.Fill;
                        playerHPUI.PlayerHPUIType = PlayerHPUIType.DelayedUnstable;

                        delayTimer.OnTick += playerHPUI.SetFillAmount;
                        playerHPUI.delayTimer = delayTimer;

                        getDelayedIndex++;
                        break;
                    }
                    getDelayedIndex++;
                }
                playerHPUIs.Insert(addIndex, playerHPUI);
            }
        }
        else
        {
            int removeCount = Mathf.RoundToInt(before - after);
            while (removeCount-- > 0)
            {
                playerHPUIs[playerHPUIs.Count - 1].DestroySelf();
                playerHPUIs.RemoveAt(playerHPUIs.Count - 1);
            }
        }
    }
}
