using UnityEngine;

/// <summary>
/// ��ǩ����
/// </summary>
public class LabelAttribute : PropertyAttribute
{
    public string Name { get; private set; }

    public LabelAttribute(string name)
    {
        Name = name;
    }
}
