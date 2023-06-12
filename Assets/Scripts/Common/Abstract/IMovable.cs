using UnityEngine;

/// <summary>
/// 具有穿透型伤害值的物体继承此接口
/// </summary>
public interface IMovable
{
    Vector3 MoveDir { get; }
}
