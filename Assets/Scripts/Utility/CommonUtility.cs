using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CommonUtility
{
    /// <summary>
    /// �ж��������Ƿ������ƣ����н�С��90��
    /// </summary>
    public static bool IsSimilarDir(this Vector3 lhs, Vector3 rhs)
    {
        return Vector3.Dot(lhs, rhs) > 0;
    }

    /// <summary>
    /// ��һ���б��в��ظ������ѡȡ������б���ԭ�б����ʱ����
    /// </summary>
    /// <param name="fromList">ԭ�б�</param>
    /// <param name="count">ѡȡ��Ŀ</param>
    public static List<T> RandomPick<T>(this ICollection<T> fromList, int count)
    {
        List<T> result = new List<T>();
        if (fromList.Count <= count)
            return fromList.ToList();
        T[] list = new T[fromList.Count];
        fromList.CopyTo(list, 0);
        int listCount = fromList.Count;
        while (result.Count < count)
        {
            int randomIndex = Random.Range(0, listCount);
            result.Add(list[randomIndex]);
            list[randomIndex] = list[--listCount];
        }
        return result;
    }

    public static float RemapFloat(this float x, float t1, float t2, float s1, float s2)
    {
        return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
    }

// float Remap(float x, float t1, float t2, float s1, float s2)
// {
//     return (s2 - s1) / (t2 - t1) * (x - t1) + s1;
//     return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
// }
}
