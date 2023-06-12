using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CommonUtility
{
    /// <summary>
    /// 判断两向量是否方向相似，即夹角小于90°
    /// </summary>
    public static bool IsSimilarDir(this Vector3 lhs, Vector3 rhs)
    {
        return Vector3.Dot(lhs, rhs) > 0;
    }

    /// <summary>
    /// 从一个列表中不重复地随机选取组成新列表，当原列表过大时慎用
    /// </summary>
    /// <param name="fromList">原列表</param>
    /// <param name="count">选取数目</param>
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
