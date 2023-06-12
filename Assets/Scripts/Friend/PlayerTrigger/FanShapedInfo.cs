using System.Collections.Generic;
using UnityEngine;

public class FanShapedInfo
{
    /// <summary>
    /// 扇形区域半径
    /// </summary>
    public float fanShapedRadius;

    /// <summary>
    /// 扇形圆心角
    /// </summary>
    public float fanShapedAngle;

    /// <summary>
    /// 碰撞体路径
    /// </summary>
    public List<Vector2> colliderPath;

    /// <summary>
    /// 击退信息
    /// </summary>
    public BeatInfo beatInfo;

    public FanShapedInfo(float fanShapedRadius, float fanShapedAngle, BeatInfo beatInfo)
    {
        this.fanShapedRadius = fanShapedRadius;
        this.fanShapedAngle = fanShapedAngle;
        colliderPath = GetColliderPath(fanShapedAngle / 2);
        this.beatInfo = beatInfo;
    }

    public List<Vector2> GetColliderPath(float fanShapedHalfAngle)
    {
        List<Vector2> path = new List<Vector2> { Vector2.zero, new Vector2(0.5f, 0f) };
        void AddPoint(float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 point = new Vector2(Mathf.Sin(angleRad) / 2, Mathf.Cos(angleRad) / 2);
            path.Add(point);
            point.x = -point.x;
            path.Insert(1, point);
        }
        for (float angle = 10f; angle < fanShapedHalfAngle; angle += 10f)
        {
            AddPoint(angle);
        }
        AddPoint(fanShapedHalfAngle);
        return path;
    }
}
