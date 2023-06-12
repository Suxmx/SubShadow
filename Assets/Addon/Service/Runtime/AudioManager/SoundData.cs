using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "shadow/SoundSO", order = 0)]
public class SoundData : ScriptableObject
{
    public List<SoundGroup> soundGroups;
}

[System.Serializable]
public class SoundGroup
{
    [Label("音频组")]
    public ESoundGroup groupType;
    public List<Sound> sounds;
}

[System.Serializable]
public class Sound
{
    [Label("音频名称")]
    public string name;

    [Label("音频文件")]
    public AudioClip audioClip;

    [Range(0, 1)]
    public float initVolume=1;

    [Label("是否在游戏开始时播放")]
    public bool playOnAwake;

    [Label("是否循环")]
    public bool loop;
    [Label("音调")]
    
    public float pitch=1;

    [NonSerialized]
    public List<AudioSource> audioSources = null;
    [NonSerialized]
    public Transform sourcesTrans=null;
    [NonSerialized]
    public float lastPlayTime;
}

public enum ESoundGroup
{
    BGM,
    SFX
}
