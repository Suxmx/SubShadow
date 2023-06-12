//using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using Sirenix.Utilities;

public class SkillInfoWindow : OdinEditorWindow
{
    private Color red=Color.red;
    private SkillManager skillManager;

   // [MenuItem("Test/window")]
    private static void ShowWindow()
    {
        var window = GetWindow<SkillInfoWindow>("运行时技能信息窗口");
        window.Show();
    }

    private void OnInspectorUpdate()
    {
        if (Application.isPlaying)
        {
            if (!skillManager)
                Init();
        }
        else
        {
            skillManager = null;
        }
    }

    private void Init()
    {
        skillManager = SkillWindowHelper.GetSkillManager();
    }

    [PropertyOrder(-1)]
    [Button(ButtonSizes.Gigantic),GUIColor("$red")]
    [LabelText("游戏尚未运行")]
    [HideInPlayMode]
    private void empty(){}
    [TableList(ShowIndexLabels =true)]
    [HideLabel]
    [DisableInEditorMode]
    public List<SkillRuntimeInfo> runtimeInfo=new List<SkillRuntimeInfo>();
}
[System.Serializable]
public class SkillRuntimeInfo{
    [PropertyOrder(-1)]
    [VerticalGroup("技能名称")]
    [Button(ButtonSizes.Gigantic)]
    [HideLabel]
    private void empty(){}
    [VerticalGroup("一级描述")]
    [HideLabel]
    [TextArea]
    public string oneGrade;
    [VerticalGroup("二级描述")]
    [HideLabel]
    [TextArea]
    public string twoGrade;
    [VerticalGroup("三级描述")]
    [HideLabel]
    [TextArea]
    public string threeGrade;
    [VerticalGroup("四级描述")]
    [HideLabel]
    [TextArea]
    public string fourGrade;
}
