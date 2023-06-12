using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.Audio;

public enum AudioPlayMode
{
    /// <summary>
    /// 当前正在播放时取消播放
    /// </summary>
    Wait,

    /// <summary>
    /// 当前正在播放时打断并重新播放
    /// </summary>
    Interrupt,

    /// <summary>
    /// 多个该音频同时播放
    /// </summary>
    Plenty
}

public class AudioManager : Service
{
    public BGMController bgmController;
    private SoundData soundData;
    private Dictionary<string, Sound> soundDics;
    private Dictionary<ESoundGroup, float> groupVolumeBeforeMuted;
    private Dictionary<ESoundGroup, List<Sound>> soundGroupDics;

    protected override void Awake()
    {
        base.Awake();
        soundDics = new Dictionary<string, Sound>();
        groupVolumeBeforeMuted = new Dictionary<ESoundGroup, float>();
        soundGroupDics = new Dictionary<ESoundGroup, List<Sound>>();
        soundData = Resources.Load<SoundData>("SoundData");
    }

    protected override void Start()
    {
        base.Start();
        Transform managerTrans = new GameObject("AudioManager").transform;
        bgmController = managerTrans.gameObject.AddComponent<BGMController>();
        DontDestroyOnLoad(managerTrans.gameObject);
        //managerTrans.SetParent(transform);

        foreach (var group in soundData.soundGroups)
        {
            Transform groupTrans = new GameObject(group.groupType.ToString()).transform;
            groupTrans.SetParent(managerTrans);

            ESoundGroup groupType = group.groupType;
            soundGroupDics.Add(groupType, new List<Sound>());
            groupVolumeBeforeMuted.Add(groupType, 1);

            foreach (var sound in group.sounds)
            {
                GameObject sourcesObj = new GameObject(sound.name);
                Transform soundObj1Trans = new GameObject($"{sound.name}1").transform;
                sourcesObj.transform.SetParent(groupTrans);
                soundObj1Trans.SetParent(sourcesObj.transform);

                AudioSource soundAudioSource =
                    soundObj1Trans.gameObject.AddComponent<AudioSource>();
                sound.audioSources = new List<AudioSource>();
                sound.audioSources.Add(soundAudioSource);
                sound.sourcesTrans = sourcesObj.transform;

                soundAudioSource.clip = sound.audioClip;
                soundAudioSource.volume = sound.initVolume; //TODO:似乎要与分贝转换,之后再看
                soundAudioSource.pitch = sound.pitch;
                soundAudioSource.playOnAwake = sound.playOnAwake; //这个在脚本中设置似乎并不能playOnAwake
                soundAudioSource.loop = sound.loop;
                if (sound.playOnAwake)
                    soundAudioSource.Play();

                soundDics.Add(sound.name, sound);
                soundGroupDics[groupType].Add(sound);
            }
        }
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="mode">播放模式</param>
    /// <param name="plentyWaitTime">Plenty播放模式下等待的时间，默认为0.1</param>
    public void PlaySound(
        string name,
        AudioPlayMode mode = AudioPlayMode.Plenty,
        float plentyWaitTime = 0.1f
    )
    {
        if (!soundDics.ContainsKey(name))
        {
            Debug.LogWarning($"音频{name}不存在!");
            return;
        }
        Sound curSound = soundDics[name];
        List<AudioSource> curSources = soundDics[name].audioSources;
        switch (mode)
        {
            case AudioPlayMode.Wait:
                if (!curSources[0].isPlaying)
                    FixedPlay(curSources[0]);
                break;
            case AudioPlayMode.Interrupt:
                FixedPlay(curSources[0]);
                break;
            case AudioPlayMode.Plenty:
                if (plentyWaitTime > 0 && Time.time - curSound.lastPlayTime < plentyWaitTime)
                {
                    return;
                }
                curSound.lastPlayTime = Time.time;
                foreach (var source in curSources)
                    if (!source.isPlaying)
                    {
                        FixedPlay(source);
                        return;
                    }
                //若遍历完还没有空余的AudioSource则新建一个AudioSource
                GameObject newSourceObj = new GameObject($"{curSound.name}{curSources.Count + 1}");
                AudioSource newSource = newSourceObj.AddComponent<AudioSource>();
                CopyAudioSource(curSources[0], newSource);
                curSources.Add(newSource);
                newSourceObj.transform.SetParent(curSound.sourcesTrans);

                newSource.Play();
                break;
        }
    }

    /// <summary>
    /// 若声音正在播放,停止该声音的播放
    /// </summary>
    /// <param name="name">音频名称</param>
    public void StopSound(string name, bool smoothly = true)
    {
        if (!soundDics.ContainsKey(name))
        {
            Debug.LogWarning($"音频{name}不存在!");
            return;
        }
        foreach (var audioSource in soundDics[name].audioSources)
            if (audioSource.isPlaying)
                if (!smoothly)
                    audioSource.Stop();
                else
                    StartCoroutine(StopSmoothly(audioSource));
    }

    public void MuteGroup(ESoundGroup groupType)
    {
        if (!soundGroupDics.ContainsKey(groupType))
        {
            Debug.LogError($"不存在该音频组!");
            return;
        }
        foreach (var sound in soundGroupDics[groupType])
        {
            foreach (var audioSource in sound.audioSources)
                audioSource.volume = 0;
        }
    }

    private IEnumerator StopSmoothly(AudioSource audioSource)
    {
        
        float originVolume = audioSource.volume;
        if (originVolume <= 0)
        {
            //Debug.Log(audioSource.name+"exit");
            yield break;
        }
        for (int i = 1; i <= 10; i++)
        {
            //Debug.Log(audioSource.name+":"+audioSource.volume.ToString());
            audioSource.volume -= originVolume / 10f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        audioSource.Stop();
        audioSource.volume = originVolume;
    }

    public void UnmuteGroup(ESoundGroup groupType)
    {
        if (!soundGroupDics.ContainsKey(groupType))
        {
            Debug.LogError($"不存在该音频组!");
            return;
        }
        foreach (var sound in soundGroupDics[groupType])
        {
            float final =
                1f / Mathf.Pow(10, 2 * (1 - groupVolumeBeforeMuted[groupType] * sound.initVolume));
            ;
            foreach (var audioSource in sound.audioSources)
                audioSource.volume = final;
        }
    }

    public void SetGroupVolume(ESoundGroup groupType, float value)
    {
        if (!soundGroupDics.ContainsKey(groupType))
        {
            Debug.LogError($"不存在该音频组!");
            return;
        }
        if (value > 1 || value < 0)
            Debug.LogError($"设置音量{value}不合法!");
        foreach (var sound in soundGroupDics[groupType])
        {
            float final;
            if (value * sound.initVolume == 0)
                final = 0;
            else
                final = 1f / Mathf.Pow(10, 2 * (1 - value * sound.initVolume)); //大概推导了一下，试出来的公式
            foreach (var audioSource in sound.audioSources)
                audioSource.volume = final;
        }
        groupVolumeBeforeMuted[groupType] = value;
    }

    public void StopGroup(ESoundGroup groupType, bool smoothly = false)
    {
        if (!soundGroupDics.ContainsKey(groupType))
        {
            Debug.LogError($"不存在该音频组!");
            return;
        }
        foreach (var sound in soundGroupDics[groupType])
        {
            foreach (var audioSource in sound.audioSources)
                if (audioSource.isPlaying)
                    if (!smoothly)
                        audioSource.Stop();
                    else
                        StartCoroutine(StopSmoothly(audioSource));
        }
    }

    private void CopyAudioSource(AudioSource oldSource, AudioSource newSource)
    {
        newSource.clip = oldSource.clip;
        newSource.volume = oldSource.volume;
        newSource.pitch = oldSource.pitch;
        newSource.loop = oldSource.loop;
    }

    /// <summary>
    /// 支持倒放的audioSource.Play
    /// </summary>
    /// <param name="source"></param>
    private void FixedPlay(AudioSource source)
    {
        if (source.pitch >= 0)
            source.Play();
        else
        {
            source.Play();
            source.time = source.clip.length - 0.001f;
        }
    }
}
