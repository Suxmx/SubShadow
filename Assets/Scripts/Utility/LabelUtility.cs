using System.Collections.Generic;
using System.Reflection;

public static class LabelUtility
{
    /// <summary>
    /// ��ȡһ��ö�����г�Ա��Ӧ��Label��ǩ���ֵ�
    /// </summary>
    public static Dictionary<T, string> CreateLabelDictionary<T>()
    {
        Dictionary<T, string> labelDict = new Dictionary<T, string>();
        FieldInfo[] fields = typeof(T).GetFields(
            BindingFlags.Static | BindingFlags.Public);
        foreach (var field in fields)
        {
            T key = (T)field.GetValue(null);
            LabelAttribute att = (LabelAttribute)field.GetCustomAttribute(
                typeof(LabelAttribute), true);
            if (att != null)
            {
                labelDict.Add(key, att.Name);
            }
            else
            {
                labelDict.Add(key, field.Name);
            }
        }
        return labelDict;
    }
}
