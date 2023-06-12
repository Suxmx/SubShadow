using UnityEngine;

[System.Serializable]
public class PlayerHPUITypeSpritePair
{
    [Label("Ѫ��UI����")]
    public PlayerHPUIType playerHPUIType;
    public Sprite sprite;
}

public class PlayerHPUIData : ScriptableObject
{
    public PlayerHPUITypeSpritePair[] playerHPUITypeSpritePairs;
}
