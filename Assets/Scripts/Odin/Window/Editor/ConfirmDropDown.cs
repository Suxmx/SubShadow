using System.Diagnostics;
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ConfirmDropDown : OdinEditorWindow
{
    Action action0;
    Action<Sound> action1;
    Sound para;
    OdinEditorWindow window;
    Color red = Color.red;
    Color green = Color.green;
    bool ifPara = false;

    [Button(ButtonSizes.Gigantic)]
    [ButtonGroup("1")]
    [LabelText("取消")]
    [GUIColor("$green")]
    public void Cancel()
    {
        window.Close();
    }

    [Button(ButtonSizes.Gigantic)]
    [ButtonGroup("1")]
    [LabelText("确认")]
    [GUIColor("$red")]
    [HideIf("$ifPara")]
    public void Confirm0()
    {
        action0();
        window.Close();
        if(ifPara)ifPara=ifPara||false;
    }

    [Button(ButtonSizes.Gigantic)]
    [ButtonGroup("1")]
    [LabelText("确认")]
    [GUIColor("$red")]
    [ShowIf("$ifPara")]
    public void Confirm1()
    {
        action1(para);
        window.Close();
    }

    public void SetWindow(OdinEditorWindow window)
    {
        this.window = window;
    }

    public void SetConfirmAction(Action<Sound> action1, Sound para)
    {
        this.action1 = action1;
        this.para = para;
        ifPara = true;
    }

    public void SetConfirmAction()
    {
        ifPara = false;
    }
}
