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

[System.Serializable]
public class SkillDescInfo
{
    SkillInfoData skillData;
    SkillInfo skillInfo;

    public SkillDescInfo(SkillInfoData skillData, SkillInfo skillInfo)
    {
        this.skillData = skillData;
        this.skillInfo = skillInfo;
    }

    [VerticalGroup("图标", 0)]
    [TableColumnWidth(10)]
    [PreviewField(80, Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
    [HideLabel]
    [OnValueChanged(nameof(ChangeIcon))]
    public Sprite icon;

    [VerticalGroup("等级一描述", 1)]
    [TextArea(5, 10)]
    [HideLabel]
    [OnValueChanged(nameof(ChangeGrade1))]
    public string grade1Descs;

    [VerticalGroup("等级二描述", 2)]
    [TextArea(5, 10)]
    [HideLabel]
    [OnValueChanged(nameof(ChangeGrade2))]
    public string grade2Descs;

    [VerticalGroup("等级三描述", 3)]
    [TextArea(5, 10)]
    [HideLabel]
    [OnValueChanged(nameof(ChangeGrade3))]
    public string grade3Descs;

    [VerticalGroup("等级四描述", 4)]
    [TextArea(5, 10)]
    [HideLabel]
    [OnValueChanged(nameof(ChangeGrade4))]
    public string grade4Descs;

    private void ChangeIcon(Sprite newIcon)
    {
        skillInfo.icon = newIcon;
        Save();
    }

    private void ChangeGrade1(string newDesc)
    {
        skillInfo.desc[0] = newDesc;
        Save();
    }

    private void ChangeGrade2(string newDesc)
    {
        skillInfo.desc[1] = newDesc;
        Save();
    }

    private void ChangeGrade3(string newDesc)
    {
        skillInfo.desc[2] = newDesc;
        Save();
    }

    private void ChangeGrade4(string newDesc)
    {
        skillInfo.desc[3] = newDesc;
        Save();
    }

    private void Save()
    {
        EditorUtility.SetDirty(skillData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
