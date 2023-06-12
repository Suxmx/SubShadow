using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OldSkillInfoWindow : EditorWindow
{
    private SkillManager skillManager;
    private Dictionary<SkillType, Skill> skills;
    private Vector2 scroll1;
    private bool showNotHaved = false;

    //[MenuItem("Tools/������Ϣ����")]
    private static void ShowWindow()
    {
        var window = GetWindow<OldSkillInfoWindow>();
        window.titleContent = new GUIContent("��ǰ������Ϣ");
        window.Show();
    }

    private void OnGUI()
    {
        Prepare();

        if (skillManager != null)
        {
            scroll1 = EditorGUILayout.BeginScrollView(scroll1);
            EditorGUILayout.LabelField("��ǰӵ�еļ�����Ϣ", EditorStyles.layerMaskField);

            foreach (var skillType in skills.Keys)
            {
                Skill currentSkill = skills[skillType];
                if (currentSkill.ownedGrade > 0)
                {
                    SkillInfo skillInfo = currentSkill.skillInfo;
                    EditorGUILayout.LabelField(skillInfo.name, EditorStyles.linkLabel);
                    EditorGUILayout.LabelField("��ǰ�ȼ�:" + currentSkill.ownedGrade.ToString());
                    EditorGUILayout.LabelField("���:" + ((int)skillInfo.skillType + 1).ToString());
                    EditorGUILayout.LabelField("��������:");
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
            showNotHaved = EditorGUILayout.ToggleLeft("��ʾδӵ�еļ���", showNotHaved);
            if (showNotHaved)
                ShowNotHaved();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            if (!Application.isPlaying)
                EditorGUILayout.LabelField("��Ϸδ����!");
            else if (skillManager == null)
            {
                var guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("��ȡ���ܹ���������!", guiStyle);
            }
            else
            {
                var guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("δ֪����!", guiStyle);
            }
        }
    }

    private void Prepare()
    {
        if (Application.isPlaying && skillManager == null)
        {
            // �����˼��ܽṹ��֮ǰ�Ĳ������ˣ���ʱע�͵�
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
        EditorGUILayout.LabelField("δӵ�еļ�����Ϣ", EditorStyles.layerMaskField);
        //scroll2 = EditorGUILayout.BeginScrollView(scroll2);
        foreach (var skillType in skills.Keys)
        {
            Skill currentSkill = skills[skillType];
            if (currentSkill.ownedGrade == 0)
            {
                SkillInfo skillInfo = currentSkill.skillInfo;
                EditorGUILayout.LabelField(skillInfo.name, EditorStyles.linkLabel);
                EditorGUILayout.LabelField("���:" + ((int)skillInfo.skillType + 1).ToString());
                EditorGUILayout.LabelField("��������:");
                for (int i = 1; i <= 4; i++)
                {
                    EditorGUILayout.LabelField("\t" + skillInfo.desc[i - 1]);
                }
            }
        }
    }
}
