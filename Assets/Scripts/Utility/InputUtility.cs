using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public static class InputUtility
{
    private static readonly MonoBehaviour mono;
    private static readonly Dictionary<int, List<KeyCode>> numKeyCodes;
    private static readonly Dictionary<char, KeyCode> letterKeyCodes;

    static InputUtility()
    {
        mono = ServiceLocator.Get<GameManager>();
        numKeyCodes = new Dictionary<int, List<KeyCode>>();
        for (int i = 0; i <= 9; i++)
        {
            numKeyCodes.Add(i, new List<KeyCode>
            {
                (KeyCode)((int)KeyCode.Alpha0 + i),
                (KeyCode)((int)KeyCode.Keypad0 + i),
            });
        }
        letterKeyCodes = new Dictionary<char, KeyCode>();
        for (char ch = 'A'; ch <= 'Z'; ch++)
            letterKeyCodes.Add(ch, (KeyCode)(ch - 'A' + (int)KeyCode.A));
        for (char ch = 'a'; ch <= 'z'; ch++)
            letterKeyCodes.Add(ch, (KeyCode)(ch - 'a' + (int)KeyCode.A));
    }

    /// <summary>
    /// 按键检测来调整某浮点型变量，长按按键可重复实行，方便策划界面化调参
    /// </summary>
    /// <param name="value">变量初值</param>
    /// <param name="decreaseKey">减小的键</param>
    /// <param name="increaseKey">增大的键</param>
    /// <param name="returnAction">对变量实行的操作</param>
    /// <param name="min">最小值</param>
    /// <param name="interval">改变间隔</param>
    public static void CheckValueChangePress(float value, KeyCode decreaseKey, KeyCode increaseKey, 
        UnityAction<float> returnAction, float min = 0.1f, float interval = 0.1f)
    {
        if (Input.GetKeyDown(decreaseKey))
        {
            mono.StartCoroutine(OnKeyDown(value, decreaseKey, returnAction, 
                x => Mathf.Max(x - interval, min)));
        }
        if (Input.GetKeyDown(increaseKey))
        {
            mono.StartCoroutine(OnKeyDown(value, increaseKey, returnAction, 
                x => x + interval));
        }
    }

    /// <summary>
    /// 按键检测来调整某整形变量，长按按键可重复实行，方便策划界面化调参
    /// </summary>
    /// <param name="value">变量初值</param>
    /// <param name="decreaseKey">减小的键</param>
    /// <param name="increaseKey">增大的键</param>
    /// <param name="returnAction">对变量实行的操作</param>
    /// <param name="min">最小值</param>
    /// <param name="interval">改变间隔</param>
    public static void CheckValueChangePress(int value, KeyCode decreaseKey, KeyCode increaseKey,
        UnityAction<float> returnAction, int min = 1, int interval = 1)
    {
        CheckValueChangePress((float)value, decreaseKey, increaseKey, returnAction, min, interval);
    }

    /// <summary>
    /// 按键检测来调整某枚举变量，方便策划界面化调参
    /// </summary>
    /// <param name="value">变量初值</param>
    /// <param name="key">按键</param>
    /// <param name="returnAction">对变量实行的操作</param>
    public static void CheckValueChangePress(Enum value, KeyCode key, UnityAction<int> returnAction)
    {
        if (Input.GetKeyDown(key))
        {
            int aimType = Convert.ToInt32(value) + 1;
            if (aimType >= Enum.GetValues(value.GetType()).Length)
                aimType = 0;
            returnAction(aimType);
        }
    }

    private static IEnumerator OnKeyDown<T>(T value, KeyCode key, 
        UnityAction<T> returnAction, Func<T, T> valueAction)
    {
        float pressTimer = 0f;
        while (Input.GetKey(key) && pressTimer <= 0.5f)
        {
            pressTimer += Time.deltaTime;
            yield return null;
        }
        if (!Input.GetKey(key))
        {
            value = valueAction(value);
            returnAction(value);
        }
        else
        {
            int actionCount = 0;
            float judgeInterval = 0.2f;
            while (Input.GetKey(key))
            {
                pressTimer += Time.deltaTime;
                if (pressTimer >= judgeInterval)
                {
                    value = valueAction(value);
                    returnAction(value);
                    if (++actionCount == 5)
                    {
                        judgeInterval /= 2;
                    }
                    pressTimer = 0f;
                }
                yield return null;
            }
        }
    }

    public static bool CheckPressNumKey(int num)
    {
        if (num >= 10)
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                return false;
            }
            num -= 10;
        }
        foreach (KeyCode key in numKeyCodes[num])
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckPressAnyNumKey(out int num)
    {
        for (int i = 0; i <= 9; i++)
        {
            foreach (KeyCode key in numKeyCodes[i])
            {
                if (Input.GetKeyDown(key))
                {
                    num = i;
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        num += 10;
                    }
                    return true;
                }
            }
        }
        num = 0;
        return false;
    }

    public static void StartCheckInputWord(string str, UnityAction action)
    {
        if (string.IsNullOrEmpty(str)) return;
        List<KeyCode> keyCodes = new List<KeyCode>();
        foreach (var letter in str) keyCodes.Add(letterKeyCodes[letter]);
        mono.StartCoroutine(DoCheckKeyCodes(keyCodes, action));
    }

    private static IEnumerator DoCheckKeyCodes(List<KeyCode> keyCodes, UnityAction action)
    {
        int currentIndex = 0;
        for (; ; )
        {
            if (Input.GetKeyDown(keyCodes[currentIndex]))
            {
                if (++currentIndex == keyCodes.Count)
                {
                    action();
                    currentIndex = 0;
                }
            }
            else if (Input.GetKeyDown(keyCodes[0]))
            {
                currentIndex = 1;
                if (keyCodes.Count == 1)
                {
                    action();
                    currentIndex = 0;
                }
            }
            else if (Input.anyKeyDown) currentIndex = 0;
            yield return null;
        }
    }
}
