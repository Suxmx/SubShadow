using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Gauss
{
    /// <summary>
    /// 产生一个期望为miu,方差为sigma2的正态分布数，默认移除超出miu+2sigma的数
    /// </summary>
    /// <param name="miu">μ</param>
    /// <param name="sigma2">σ</param>
    /// <param name="remove">去除离谱数</param>
    /// <returns>符合正态分布的数(中的一个)</returns>
    public static float Get(
        float miu,
        float sigma2 = 1f,
        float remove = 2
    ) // 均值 方差
    {
        System.Random ran = new System.Random(GetRandomSeed());
        float r1 = (float)ran.NextDouble();
        float r2 = (float)ran.NextDouble();
        float r = Mathf.Sqrt((-2) * Mathf.Log(r2)) * Mathf.Sin(2 * (float)Mathf.PI * r1);
        float result = miu + sigma2 * r;
        if (Mathf.Abs(result) < miu + 2f * Mathf.Sqrt(sigma2))
            return result;
        else
            return Get(miu, sigma2, remove);
    }

    private static int GetRandomSeed() //产生随机种子
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng =
            new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}
