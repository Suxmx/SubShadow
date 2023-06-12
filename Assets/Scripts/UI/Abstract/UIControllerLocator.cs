using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ڻ�ȡUIController
/// </summary>
public static class UIControllerLocator
{
    internal static readonly Dictionary<Type, UIController> uiControllerDict = new Dictionary<Type, UIController>();

    internal static T Get<T>() where T : UIController
        => Get(typeof(T)) as T;

    internal static UIController Get(Type type)
    {
        if (!uiControllerDict.ContainsKey(type))
        {
            Debug.LogWarning($"����Ϊ{type}��UIController������");
            return null;
        }
        return uiControllerDict[type];
    }

    internal static void Register(UIController controller)
    {
        Type type = controller.GetType();
        if (uiControllerDict.ContainsKey(type))
        {
            Debug.LogWarning($"����Ϊ{type}��UIController���޸���");
            uiControllerDict[type] = controller;
        }
        else
            uiControllerDict.Add(type, controller);
    }
}
