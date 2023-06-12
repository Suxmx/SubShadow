using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.UI;

public class AudioTest : MonoBehaviour
{
    private AudioManager manager;

    private void Start()
    {
        manager = ServiceLocator.Get<AudioManager>();
    }

    public void MuteTestGroup()
    {
        manager.MuteGroup(ESoundGroup.BGM);
    }

    public void UnMuteTestGroup()
    {
        manager.UnmuteGroup(ESoundGroup.BGM);
    }

    public void SetGroupVolume()
    {
        float value = GameObject.Find("Slider").GetComponent<Slider>().value;
        manager.SetGroupVolume(ESoundGroup.BGM, value);
    }
}
