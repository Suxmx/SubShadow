using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Gauss
{
    /// <summary>
    /// ����һ������Ϊmiu,����Ϊsigma2����̬�ֲ�����Ĭ���Ƴ�����miu+2sigma����
    /// </summary>
    /// <param name="miu">��</param>
    /// <param name="sigma2">��</param>
    /// <param name="remove">ȥ��������</param>
    /// <returns>������̬�ֲ�����(�е�һ��)</returns>
    public static float Get(
        float miu,
        float sigma2 = 1f,
        float remove = 2
    ) // ��ֵ ����
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

    private static int GetRandomSeed() //�����������
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng =
            new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}
