using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using Sirenix.Utilities;

public class AudioManagerWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/音频配置")]
    private static void OpenWindow()
    {
        var window = GetWindow<AudioManagerWindow>("音频配置");
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    SoundData soundData;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();
        HandleSoundData(tree);
        return tree;
    }

    private void HandleSoundData(OdinMenuTree tree)
    {
        soundData = AssetDatabase.LoadAssetAtPath<SoundData>("Assets/Resources/SoundData.asset");
        foreach (var soundGroup in soundData.soundGroups)
        {
            AudioGroupPanel audioGroupPanel = new AudioGroupPanel(soundGroup.sounds, soundData);
            tree.Add(soundGroup.groupType.ToString(), audioGroupPanel);
        }
    }
}

public class AudioGroupPanel
{
    SoundData soundData;
    List<Sound> soundsInSoundData;
    bool UnAvailability => addSound.UnAvailability;

    public AudioGroupPanel(List<Sound> sounds, SoundData soundData)
    {
        Init(sounds, soundData);
    }

    private void Init(List<Sound> sounds, SoundData soundData)
    {
        this.soundData = soundData;
        this.soundsInSoundData = sounds;

        allSound = new List<SoundInOdin>();
        soundToShow = new List<SoundInOdin>();
        addSound = new AddSoundInOdin(soundData, sounds);

        foreach (var sound in sounds)
        {
            SoundInOdin soundInOdin = new SoundInOdin(sound, DeleteConfirmWindow, Save);
            allSound.Add(soundInOdin);
            soundToShow.Add(soundInOdin);
        }
    }

    private void DeleteConfirmWindow(Sound sound)
    {
        var window =ScriptableObject.CreateInstance<ConfirmDropDown>();
        OdinEditorWindow.CreateOdinEditorWindowInstanceForObject(window);

        Vector2 mouseVec = Event.current.mousePosition;
        Rect mouseRect=new Rect(mouseVec.x,mouseVec.y,1,1);
        mouseRect.position=GUIUtility.GUIToScreenPoint(mouseRect.position);
        window.ShowAsDropDown(mouseRect,new Vector2(350,70));

        window.SetWindow(window);
        window.SetConfirmAction(Delete,sound);
    }

    private void Delete(Sound sound)
    {
        Debug.Log("Done");
        soundsInSoundData.Remove(sound);
        EditorUtility.SetDirty(soundData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Init(soundsInSoundData, soundData);
    }

    [Button(ButtonSizes.Large), PropertyOrder(-1)]
    [LabelText("添加")]
    [DisableIf("$UnAvailability")]
    [HideLabel]
    public void AddSound()
    {
        Sound sound = new Sound();
        sound.audioClip = addSound.audioClip;
        sound.initVolume = addSound.volume;
        sound.name = addSound.name;
        sound.loop = addSound.loop;
        sound.pitch = addSound.pitch;
        soundsInSoundData.Add(sound);
        Save();
        Init(soundsInSoundData, soundData);
        if (searchWords != null)
            Search(searchWords);
    }

    private void Save()
    {
        EditorUtility.SetDirty(soundData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void Search(string searchWords)
    {
        soundToShow.RemoveRange(0, soundToShow.Count);
        Dictionary<SoundInOdin, bool> searchResult = new Dictionary<SoundInOdin, bool>(); //foreach方法遍历途中不能改变遍历对象的值，因此先用字典存下符合是否搜索结果
        foreach (var sound in allSound)
            searchResult.Add(sound, true);

        for (int i = 0; i < searchWords.Length; i++)
            foreach (var sound in allSound)
                if (
                    sound.name.IndexOf(searchWords[i]) == -1
                    && sound.clipName.IndexOf(searchWords[i]) == -1
                )
                    searchResult[sound] = false;
        foreach (var sound in searchResult.Keys)
            if (searchResult[sound])
                soundToShow.Add(sound);
    }

    //[Title("添加", TitleAlignment = TitleAlignments.Centered)]
    [HideLabel]
    public AddSoundInOdin addSound; //用于添加新的音频

    [Title("查询", TitleAlignment = TitleAlignments.Centered)]
    [OnValueChanged(nameof(Search))]
    [HideLabel]
    public string searchWords; //搜索词

    [Title("音频配置", TitleAlignment = TitleAlignments.Centered)]
    [TableList(ShowIndexLabels = true)]
    [HideLabel]
    public List<SoundInOdin> soundToShow; //用于显示搜索结果

    private List<SoundInOdin> allSound; //用于保存全部的音频
}

[System.Serializable]
public class SoundInOdin
{
    private Color red = Color.red;
    private Sound soundInSoundData;
    private Action<Sound> deleteAction;
    private Action saveAction;

    public SoundInOdin(Sound sound, Action<Sound> deleteAction, Action saveAction)
    {
        clipName = sound.audioClip.name;
        name = sound.name;
        audioClip = sound.audioClip;
        volume = sound.initVolume;
        pitch = sound.pitch;
        loop = sound.loop;
        this.soundInSoundData = sound;
        this.deleteAction = deleteAction;
        this.saveAction = saveAction;
        // Debug.Log(AssetDatabase.GetAssetPath(sound.audioClip));
    }

    private void ChangeName(string changeName)
    {
        soundInSoundData.name = changeName;
        saveAction();
    }

    private void ChangeAudioClip(AudioClip changeClip)
    {
        soundInSoundData.audioClip = changeClip;
        saveAction();
    }

    private void ChangeVolume(float changeVolume)
    {
        soundInSoundData.initVolume = changeVolume;
        saveAction();
    }

    private void ChangePitch(float changePitch)
    {
        soundInSoundData.pitch = changePitch;
        saveAction();
    }

    private void ChangeLoop(bool changeLoop)
    {
        soundInSoundData.loop = changeLoop;
        saveAction();
    }

    // [VerticalGroup("音频属性")]
    // [LabelText("音频名称")]
    // [ReadOnly]
    // [HideLabel]
    // [SuffixLabel("只读名称")]
    [NonSerialized]
    public string clipName;

    [VerticalGroup("音频属性")]
    [LabelText("引用名称")]
    [HideLabel]
    [OnValueChanged(nameof(ChangeName))]
    [SuffixLabel("回车保存"), Delayed]
    public string name;

    [VerticalGroup("音频属性")]
    [LabelText("音频文件")]
    [HideLabel]
    [OnValueChanged(nameof(ChangeAudioClip))]
    public AudioClip audioClip;

    [VerticalGroup("音频属性")]
    [HideLabel]
    [GUIColor("$red")]
    [LabelText("删除")]
    [Button(ButtonSizes.Small)]
    public void DeleteButton()
    {
        deleteAction(soundInSoundData);
    }

    [VerticalGroup("配置")]
    [LabelText("初始音量")]
    [HideLabel]
    [OnValueChanged(nameof(ChangeVolume))]
    [Range(0, 1), Delayed, SuffixLabel("回车保存")]
    public float volume;

    [VerticalGroup("配置")]
    [LabelText("初始音调")]
    [HideLabel]
    [OnValueChanged(nameof(ChangePitch))]
    [Delayed, SuffixLabel("回车保存")]
    public float pitch;

    [VerticalGroup("配置")]
    [LabelText("是否循环")]
    [HideLabel]
    [OnValueChanged(nameof(ChangeLoop))]
    public bool loop;
}

[System.Serializable]
public class AddSoundInOdin
{
    public AddSoundInOdin(SoundData soundData, List<Sound> soundsInSoundData)
    {
        this.soundData = soundData;
        this.soundsInSoundData = soundsInSoundData;
    }

    SoundData soundData;
    Sound soundInSoundData;
    List<Sound> soundsInSoundData;
    public bool UnAvailability => !(audioClip != null && name != null && name.Length > 0);
    bool ClipNull => audioClip == null;

    [HorizontalGroup("1"), LabelWidth(100)]
    [LabelText("引用名称")]
    [SuffixLabel("必须")]
    [HideLabel]
    public string name;

    [HorizontalGroup("1"), LabelWidth(100)]
    [LabelText("音频文件")]
    [SuffixLabel("必须")]
    [HideLabel]
    public AudioClip audioClip;

    [LabelText("初始音量"), LabelWidth(100)]
    [HideLabel]
    public float volume = 1;

    [LabelText("初始音调"), LabelWidth(100)]
    [HideLabel]
    public float pitch = 1;

    [LabelText("是否循环")]
    [HideLabel]
    public bool loop = false;
}
