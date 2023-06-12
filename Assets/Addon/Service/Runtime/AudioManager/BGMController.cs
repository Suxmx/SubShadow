using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;

public class BGMController : MonoBehaviour
{
    private EventSystem eventSystem;
    private AudioManager audioManager;

    private void Start()
    {
        eventSystem = ServiceLocator.Get<EventSystem>();
        audioManager = ServiceLocator.Get<AudioManager>();

        eventSystem.AddListener<int>(EEvent.AfterLoadScene, PlayBGMAfterLoadingScene);
        audioManager.SetGroupVolume(ESoundGroup.BGM,0.6f);
    }

    private void PlayBGMAfterLoadingScene(int index)
    {
        audioManager.StopGroup(ESoundGroup.BGM,true);
        switch (index)
        {
            case 1:
                audioManager.PlaySound("MainMenu");
                break;
            case 2:
                audioManager.PlaySound("FightBGM");
                break;
            default:
                Debug.LogError($"BGMController中加载场景序号{index}未知");
                break;
        }
    }

    public void PlayerDie()
    {
        //Debug.Log("here");
        audioManager.StopGroup(ESoundGroup.BGM,true);
        audioManager.PlaySound("PlayerDieBGM");
    }
    public void Success(){
        audioManager.StopGroup(ESoundGroup.BGM,true);
        audioManager.PlaySound("SuccessBGM");
    }
}
