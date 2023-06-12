using UnityEngine;

[System.Serializable]
public class PlayerHPUITypeSpritePair
{
    [Label("ÑªÁ¿UIÀàÐÍ")]
    public PlayerHPUIType playerHPUIType;
    public Sprite sprite;
}

public class PlayerHPUIData : ScriptableObject
{
    public PlayerHPUITypeSpritePair[] playerHPUITypeSpritePairs;
}
