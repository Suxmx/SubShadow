using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        hotSpot = new Vector2(cursorTexture.height, cursorTexture.width) / 2f;
        //Debug.Log(hotSpot);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

}