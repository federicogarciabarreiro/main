using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorTextureType
{
    NONE,
    PRIMARY,
    SECONDARY,
    DUAL
}



public class CursorManager : MonoBehaviour {

    public static CursorManager me;

    public Texture2D currentCursorTexture;
    public List<Texture2D> listOfTextures = new List<Texture2D>();
    bool primary;
    bool secondary;

    public void Awake()
    {
        me = this;
    }
    public void Start()
    {
        SetCursorType(CursorTextureType.NONE);
    }
    public void SetCursorType(CursorTextureType ct)
    {
        currentCursorTexture = listOfTextures[(int)ct];
        Vector2 cursorHotspot = new Vector2(listOfTextures[(int)ct].width / 2, listOfTextures[(int)ct].height / 2);
        Cursor.SetCursor(currentCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void SetCursorColor(bool state, CursorTextureType type)
    {
        //if(type == CursorTextureType.PRIMARY && primary != state)
        //{
        //    primary = state;
        //    SetCursorSkillType();
        //}

        //if (type == CursorTextureType.SECONDARY && secondary != state)
        //{
        //    secondary = state;
        //    SetCursorSkillType();
        //}

    }

    public void SetCursorSkillType()
    {
        //if (!primary && !secondary)  SetCursorType(CursorTextureType.NONE);
        //if (primary && !secondary) SetCursorType(CursorTextureType.PRIMARY);
        //if (!primary && secondary) SetCursorType(CursorTextureType.SECONDARY);
        //if (primary && secondary) SetCursorType(CursorTextureType.DUAL);

    }
}
