using Services;
using System.Collections.Generic;
using UnityEngine;

public class ShadowChargeUIController : MonoBehaviour
{
    private GameObject shadowChargeUIPrefab;
    private GameObject shadowWallChargeUIPrefab;

    private List<ShadowChargeUI> shadowChargeUIs;
    private int currentShadowIndex;
    private ShadowWallChargeUI shadowWallChargeUI;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager=ServiceLocator.Get<AudioManager>();
        shadowChargeUIPrefab = Resources.Load<GameObject>("ShadowChargeUI");
        shadowWallChargeUIPrefab = Resources.Load<GameObject>("ShadowWallChargeUI");
        shadowChargeUIs = new List<ShadowChargeUI>();
        currentShadowIndex = 0;

        ShadowManager shadowManager = ServiceLocator.Get<ShadowManager>();
        ShadowChargeTimer shadowChargeTimer = shadowManager.shadowChargeTimer;
        shadowChargeTimer.OnShadowMaxChargedCountChange += UpdateShadowMaxChargedCount;
        shadowChargeTimer.OnShadowChargedCountChange += UpdateShadowChargedCount;
        shadowChargeTimer.OnTick += UpdateShadowChargeProgress;
        shadowManager.shadowWallChargeTimer.OnTick += UpdateShadowWallChargeProgress;
        shadowManager.AfterSetShadowWall += () => UpdateShadowWallChargeProgress(0f);
        shadowManager.OnSetShadowWallActive += SetShadowWallChargeUIActive;
    }

    private void UpdateShadowMaxChargedCount(int before, int after)
    {
        if (shadowChargeUIs.Count != before)
        {
            Debug.LogWarning("影子最大充能数的传递出现问题，将不能正常显示影子信息");
        }

        if (before < after)
        {
            int addCount = after - before;
            while (addCount-- > 0)
            {
                ShadowChargeUI shadowChargeUI = Instantiate(shadowChargeUIPrefab, transform)
                    .GetComponent<ShadowChargeUI>();
                shadowChargeUI.SetChargeProgress(0f);
                shadowChargeUIs.Add(shadowChargeUI);
            }
            if (shadowChargeUIs[currentShadowIndex].Charged)
                currentShadowIndex = FindFirstUnchargedIndex();
        }
        else
        {
            int removeCount = before - after;
            while (removeCount-- > 0)
            {
                int index = FindFirstUnchargedIndex();
                shadowChargeUIs[index].DestroySelf();
                shadowChargeUIs.RemoveAt(index);
            }
            if (currentShadowIndex >= shadowChargeUIs.Count ||
            shadowChargeUIs[currentShadowIndex].Charged)
                currentShadowIndex = FindFirstUnchargedIndex();
        }
    }

    private void UpdateShadowChargedCount(int before, int after)
    {
        if (after > shadowChargeUIs.Count)
        {
            Debug.LogWarning("当前影子充能数的传递出现问题，将不能正常显示影子信息");
        }

        if (before < after)
        {
            audioManager.PlaySound("ShadowChargeFinish");
            shadowChargeUIs[currentShadowIndex].SetChargeProgress(1f);
            shadowChargeUIs[currentShadowIndex].ExpandAndFadeOut();
            for (int i = 1; i < after - before; i++)
            {
                shadowChargeUIs[FindFirstUnchargedIndex()].SetChargeProgress(1f);
            }
            currentShadowIndex = FindFirstUnchargedIndex();
        }
        else if (before > after)
        {
            bool needAdjustCurrentIndex = shadowChargeUIs[currentShadowIndex].Charged;
            int count = Mathf.Min(before, shadowChargeUIs.Count) - after;
            if (count > 0)
            {
                for (int i = 0; i < shadowChargeUIs.Count; i++)
                {
                    if (shadowChargeUIs[i].Charged)
                    {
                        shadowChargeUIs[i].SetChargeProgress(0f);
                        if (--count == 0) break;
                    }
                }
            }
            if (needAdjustCurrentIndex) currentShadowIndex = FindFirstUnchargedIndex();
        }
    }

    private int FindFirstUnchargedIndex()
    {
        for (int i = 0; i < shadowChargeUIs.Count; i++)
        {
            if (!shadowChargeUIs[i].Charged) return i;
        }
        return 0;
    }

    private void UpdateShadowChargeProgress(float progress)
    {
        shadowChargeUIs[currentShadowIndex].SetChargeProgress(progress);
    }

    private void SetShadowWallChargeUIActive(bool active)
    {
        if (active && shadowWallChargeUI == null)
        {
            shadowWallChargeUI = Instantiate(shadowWallChargeUIPrefab, transform)
                .GetComponent<ShadowWallChargeUI>();
            shadowWallChargeUI.transform.SetAsFirstSibling();
            shadowWallChargeUI.SetChargeProgress(1f);
        }
        else if (!active && shadowWallChargeUI != null)
        {
            shadowWallChargeUI.DestroySelf();
            shadowWallChargeUI = null;
        }
    }

    private void UpdateShadowWallChargeProgress(float progress)
    {
        shadowWallChargeUI.SetChargeProgress(progress);
    }
    private void PlayChargeFinishAudio(){

    }
}
