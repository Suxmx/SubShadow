using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    Whirl,
    /// <summary>
    /// �ո�
    /// </summary>
    [Label("�ո�")]
    Reap,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    Sunspot,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    TurnWeaponAround,
    /// <summary>
    /// ج��
    /// </summary>
    [Label("ج��")]
    NightmarishFlame,
    /// <summary>
    /// ��ʸ
    /// </summary>
    [Label("��ʸ")]
    FlyingArrow,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    RoughSea,
    /// <summary>
    /// �ƽ�
    /// </summary>
    [Label("�ƽ�")]
    PushOn,
    /// <summary>
    /// �ۼ�
    /// </summary>
    [Label("�ۼ�")]
    Gather,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    ConfusedNoise,
    /// <summary>
    /// ʵ��
    /// </summary>
    [Label("ʵ��")]
    RealImage,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    Ghost,
    /// <summary>
    /// ���
    /// </summary>
    [Label("���")]
    Unbridled,
    /// <summary>
    /// ��¡
    /// </summary>
    [Label("��¡")]
    Clone,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    Reborn,
    /// <summary>
    /// ����
    /// </summary>
    [Label("����")]
    AllRiversRunIntoSea,
}

[System.Serializable]
public class SkillInfo
{
    [Label("��������")]
    public SkillType skillType;
    [Label("��������")]
    public string name;
    [Label("����ͼ��")]
    public Sprite icon;
    [Header("��������")]
    [TextArea]
    public List<string> desc;
}
