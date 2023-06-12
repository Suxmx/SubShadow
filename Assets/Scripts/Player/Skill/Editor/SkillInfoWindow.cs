using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OldSkillInfoWindow : EditorWindow
{
    private SkillManager skillManager;
    private Dictionary<SkillType, Skill> skills;
    private Vector2 scroll1;
    private bool showNotHaved = false;

    //[MenuItem("Tools/技能信息窗口")]
    private static void ShowWindow()
    {
        var window = GetWindow<OldSkillInfoWindow>();
        window.titleContent = new GUIContent("当前技能信息");
        window.Show();
    }

    private void OnGUI()
    {
        Prepare();

        if (skillManager != null)
        {
            scroll1 = EditorGUILayout.BeginScrollView(scroll1);
            EditorGUILayout.LabelField("当前拥有的技能信息", EditorStyles.layerMaskField);

            foreach (var skillType in skills.Keys)
            {
                Skill currentSkill = skills[skillType];
                if (currentSkill.ownedGrade > 0)
                {
                    SkillInfo skillInfo = currentSkill.skillInfo;
                    EditorGUILayout.LabelField(skillInfo.name, EditorStyles.linkLabel);
                    EditorGUILayout.LabelField("当前等级:" + currentSkill.ownedGrade.ToString());
                    EditorGUILayout.LabelField("编号:" + ((int)skillInfo.skillType + 1).ToString());
                    EditorGUILayout.LabelField("技能描述:");
                    for (int i = 1; i <= 4; i++)
                    {
                        var guiStyle = new GUIStyle();
                        guiStyle.normal.textColor = Color.green;

                        if (i <= currentSkill.ownedGrade)
                            EditorGUILayout.LabelField("\t" + skillInfo.desc[i - 1], guiStyle, GUILayout.MaxWidth(1000));
                        else
                            EditorGUILayout.LabelField("\t" + skillInfo.desc[i - 1], GUILayout.MaxWidth(1000));
                    }

                }
                //EditorGUILayout.EndScrollView();
            }
            showNotHaved = EditorGUILayout.ToggleLeft("显示未拥有的技能", showNotHaved);
            if (showNotHaved)
                ShowNotHaved();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            if (!Application.isPlaying)
                EditorGUILayout.LabelField("游戏未运行!");
            else if (skillManager == null)
            {
                var guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("获取技能管理器错误!", guiStyle);
            }
            else
            {
                var guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("未知错误!", guiStyle);
            }
        }
    }

    private void Prepare()
    {
        if (Application.isPlaying && skillManager == null)
        {
            // 调整了技能结构，之前的不可用了，暂时注释掉
            skillManager = null;
            //skillManager = SkillWindowHelper.GetSkillManager();
            //skills = skillManager.skills;
            //sDatas = skillManager.SkillInfoData;
        }
        else if (!Application.isPlaying)
            skillManager = null;
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void ShowNotHaved()
    {
        EditorGUILayout.LabelField("未拥有的技能信息", EditorStyles.layerMaskField);
        //scroll2 = EditorGUILayout.BeginScrollView(scroll2);
        foreach (var skillType in skills.Keys)
        {
            Skill currentSkill = skills[skillType];
            if (currentSkill.ownedGrade == 0)
            {
                SkillInfo skillInfo = currentSkill.skillInfo;
                EditorGUILayout.LabelField(skillInfo.name, EditorStyles.linkLabel);
                EditorGUILayout.LabelField("编号:" + ((int)skillInfo.skillType + 1).ToString());
                EditorGUILayout.LabelField("技能描述:");
                for (int i = 1; i <= 4; i++)
                {
                    EditorGUILayout.LabelField("\t" + skillInfo.desc[i - 1]);
                }
            }
        }
    }
}
