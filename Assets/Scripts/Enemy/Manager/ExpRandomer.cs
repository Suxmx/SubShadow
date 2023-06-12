using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpRandomer
{
    public static List<Vector2> GenerateCoordinates(int dropEXP, Vector2 enemyPos, float space)
    {
        List<Vector2> matrixCoors = new List<Vector2>();
        List<Vector2> randomCoors = new List<Vector2>();
        float startX,
            startY;
        int sqr = 0;
        while (Mathf.Pow((++sqr), 2) < dropEXP)
            ;
        //Debug.Log(sqr);
        if (sqr % 2 == 0)
        {
            startX = enemyPos.x - (sqr / 2 - 0.5f) * space;
            startY = enemyPos.y + (sqr / 2 - 0.5f) * space;
        }
        else
        {
            startX = enemyPos.x - (sqr / 2) * space;
            startY = enemyPos.y + (sqr / 2) * space;
        }

        for (int i = 1; i <= sqr; i++)
            for (int j = 1; j <= sqr; j++)
            {
                Vector2 pos = new Vector2(startX + (j - 1) * space, startY - (i - 1) * space);
                matrixCoors.Add(pos);
            }
        for (int i = 1; i <= sqr * sqr; i++)
        {
            Vector2 randomCoor = matrixCoors[Random.Range(0, matrixCoors.Count)];
            randomCoor.x += Gauss.Get(0) * space * 0.5f;
            randomCoor.y += Gauss.Get(0) * space * 0.5f;
            randomCoors.Add(randomCoor);
            matrixCoors.Remove(randomCoor);
        }
        return randomCoors;
    }
}
// 。。。。
// 。。。。
// 。。。。
// 。。。。

// 。。。
// 。。。
// 。。。
