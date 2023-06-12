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

public class SkillSettingWindow : OdinMenuEditorWindow
{
    public SkillInfoData skillData = null;

    [MenuItem("Tools/技能配置")]
    private static void OpenWindow()
    {
        var window = GetWindow<SkillSettingWindow>("技能配置");
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    private void OnInspectorUpdate() { }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();
        HandleSkillData(tree);

        return tree;
    }

    private void HandleSkillData(OdinMenuTree tree)
    {
        skillData = AssetDatabase.LoadAssetAtPath<SkillInfoData>(
            "Assets/Resources/SkillInfoData.asset"
        );
        Dictionary<String, List<FieldInfo>> skillDic = new Dictionary<string, List<FieldInfo>>();
        Dictionary<String, SkillEditPanel> panelDic = new Dictionary<string, SkillEditPanel>();
        Dictionary<String, SkillInfo> skillDescDic = new Dictionary<string, SkillInfo>();

        string currentHeader = null;
        SkillEditPanel basicAttributePanel = new SkillEditPanel(skillData, "系统属性初始值");
        panelDic.Add("系统属性初始值", basicAttributePanel);
        skillDic.Add("系统属性初始值", new List<FieldInfo>());
        tree.Add("系统属性初始值", basicAttributePanel);

        foreach (var member in skillData.GetType().GetMembers())
        {
            if (member is FieldInfo fieldInfo && fieldInfo != null)
            {
                if (fieldInfo.FieldType == typeof(List<SkillInfo>)) //从前两个列表中直接读取各个技能的名字
                {
                    string skillGroupName = "读取出错"; //方便错误时debug

                    if (fieldInfo.Name.Contains("attack"))
                        skillGroupName = "攻击技能组";
                    else if (fieldInfo.Name.Contains("assist"))
                        skillGroupName = "辅助技能组";

                    foreach (var skillInfo in (List<SkillInfo>)fieldInfo.GetValue(skillData))
                    {
                        SkillEditPanel newPanel = new SkillEditPanel(skillData, skillInfo.name);
                        skillDic.Add(skillInfo.name, new List<FieldInfo>());
                        panelDic.Add(skillInfo.name, newPanel);
                        skillDescDic.Add(skillInfo.name, skillInfo);
                        tree.Add($"技能组配置/{skillGroupName}/{skillInfo.name}", newPanel);
                    }
                }
                else
                {
                    foreach (var attribute in fieldInfo.GetCustomAttributes(false))
                    {
                        if (attribute is HeaderAttribute headerAttr)
                        {
                            currentHeader = headerAttr.header;
                        }
                    }
                    if (skillDic.ContainsKey(currentHeader))
                        skillDic[currentHeader].Add(fieldInfo);
                    else
                        Debug.LogError($"技能窗口变量{currentHeader}命名错误!");
                }
            }
        }
        foreach (var panel in panelDic)
        {
            panel.Value.HandleAttributeInfos(skillDic[panel.Key]);
            if (!panel.Key.Contains("系统"))
            {
                try
                {
                    panel.Value.HandleDescInfos(skillDescDic[panel.Key]);
                }
                catch
                {
                    Debug.LogError(panel.Key);
                }
            }
        }
    }
}

public class SkillEditPanel
{
    private string skillName;
    private bool ifSystem=false;
    SkillInfoData skillData = null;

    public SkillEditPanel(SkillInfoData skillData, string _name)
    {
        skillName = _name;
        this.skillData = skillData;
        if(skillName.Contains("系统"))
            ifSystem=true;
    }

    public void HandleAttributeInfos(List<FieldInfo> fieldInfos)
    {
        skillAttributeInfos = new List<SkillAttributeInfo>();
        foreach (var fieldInfo in fieldInfos)
        {
            string attributeName = null;
            attributeName = fieldInfo.GetAttribute<LabelAttribute>().Name;
            SkillAttributeInfo info = null;

            if (fieldInfo.GetValue(skillData).GetType() == typeof(int))
                info = new SkillAttributeInfo(
                    skillData,
                    fieldInfo,
                    attributeName,
                    (int)fieldInfo.GetValue(skillData)
                );
            else if ((fieldInfo.GetValue(skillData).GetType() == typeof(float)))
                info = new SkillAttributeInfo(
                    skillData,
                    fieldInfo,
                    attributeName,
                    (float)fieldInfo.GetValue(skillData)
                );
            else
                Debug.LogError("读取技能时类型错误");

            skillAttributeInfos.Add(info);
        }
    }

    public void HandleDescInfos(SkillInfo skillInfo)
    {
        if (ifSystem)
            ifSystem = ifSystem || false;//防止报警告
        skillDescInfos = new List<SkillDescInfo>();
        skillDescInfos.Add(new SkillDescInfo(skillData,skillInfo));
        skillDescInfos[0].icon = skillInfo.icon;
        skillDescInfos[0].grade1Descs = skillInfo.desc[0];
        skillDescInfos[0].grade2Descs = skillInfo.desc[1];
        skillDescInfos[0].grade3Descs = skillInfo.desc[2];
        skillDescInfos[0].grade4Descs = skillInfo.desc[3];
    }

    [Title("属性配置", TitleAlignment = TitleAlignments.Centered)]
    [TableList(ShowIndexLabels = true)]
    [HideLabel]
    public List<SkillAttributeInfo> skillAttributeInfos;

    [Title("技能描述", TitleAlignment = TitleAlignments.Centered)]
    [TableList]
    [HideLabel]
    [HideIf("ifSystem")]
    public List<SkillDescInfo> skillDescInfos;
}
