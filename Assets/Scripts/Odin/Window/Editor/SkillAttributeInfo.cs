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
public class SkillAttributeInfo
{
    string name;
    bool ifFloat = true;
    FieldInfo fieldInfo;
    SkillInfoData skillData = null;

    public SkillAttributeInfo(
        SkillInfoData skillData,
        FieldInfo fieldInfo,
        string _name,
        float _value
    ) //新的自找麻烦出现了
    {
        name = _name;
        floatValue = _value;
        ifFloat = true;
        this.fieldInfo = fieldInfo;
        this.skillData = skillData;
    }

    public SkillAttributeInfo(
        SkillInfoData skillData,
        FieldInfo fieldInfo,
        string _name,
        int _value
    ) //新的自找麻烦出现了
    {
        name = _name;
        intValue = _value;
        ifFloat = false;
        this.fieldInfo = fieldInfo;
        this.skillData = skillData;
    }

    private void OnFloatValueChange(float value)
    {
        fieldInfo.SetValue(skillData, value);
        EditorUtility.SetDirty(skillData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnIntValueChange(int value)
    {
        fieldInfo.SetValue(skillData, value);
        EditorUtility.SetDirty(skillData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [VerticalGroup("属性名称"), PropertyOrder(0)]
    [Button(ButtonSizes.Large)]
    [LabelText("$name")]
    private void empty()//空语句，下文防止unity报错
    {
        if (ifFloat)
            ifFloat = ifFloat || false;
    }

    [VerticalGroup("数值"), PropertyOrder(1)]
    [OnValueChanged(nameof(OnFloatValueChange))]
    [ShowIf(nameof(ifFloat))]
    [SuffixLabel("小数值")]
    [HideLabel]
    public float floatValue;

    [VerticalGroup("数值"), LabelWidth(100), PropertyOrder(1)]
    [OnValueChanged(nameof(OnIntValueChange))]
    [HideIf(nameof(ifFloat))]
    [SuffixLabel("整数值")]
    [HideLabel]
    public int intValue;
}