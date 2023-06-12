using System;
using System.Collections.Generic;

[Flags]
public enum FlagsEnum
{

}

/// <summary>
/// 带有Flags特性的多选枚举的工具类
/// </summary>
public static class FlagsEnumUtility
{
    private static readonly Dictionary<FlagsEnum, string> stringDict;

    static FlagsEnumUtility()
    {
        stringDict = LabelUtility.CreateLabelDictionary<FlagsEnum>();
    }

    public static void Add(ref this FlagsEnum origin, FlagsEnum toAdd)
        => origin |= toAdd;

    public static void Remove(ref this FlagsEnum origin, FlagsEnum toRemove)
        => origin &= ~toRemove;

    public static bool Contains(this FlagsEnum origin, FlagsEnum toCheck)
        => 0 != (origin & toCheck);

    public static bool IsNone(this FlagsEnum origin) => origin == 0;

    public static void TurnNone(ref this FlagsEnum origin) => origin = 0;

    public static string GetString(FlagsEnum flagsEnum)
    {
        if (!stringDict.ContainsKey(flagsEnum))
        {
            string effectString;
            if (flagsEnum.IsNone())
            {
                effectString = "无";
            }
            else
            {
                effectString = string.Empty;
                int num = 0;
                foreach (FlagsEnum value in Enum.GetValues(typeof(FlagsEnum)))
                {
                    if (!flagsEnum.Contains(value)) continue;
                    if (num > 0)
                    {
                        if ((num & 1) == 0) effectString += "、";
                        else effectString += "\n、";
                    }
                    effectString += stringDict[value];
                    num++;
                }
            }
            stringDict.Add(flagsEnum, effectString);
            return effectString;
        }
        else return stringDict[flagsEnum];
    }
}
