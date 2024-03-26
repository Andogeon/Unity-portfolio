using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

public static class CGlobalValues
{
    public static readonly int m_iIndex = 0;

    public static bool m_bIsGameOver = false;

    public static bool m_bIsPause = false;
}

public static class CGlobalEnum
{
    public enum BOUNSTILETYPE
    {
        BOUNSTILE_NONE,
        BOUNSTILE_LIFE,
        BOUNSTILE_NICE,
        BOUNSTILE_GOOD,
        BOUNSTILE_BOUNS,
        BOUNSTILE_END
    }

    public enum SCENE_RANGE
    {
        RANGE_NONE,
        RANGE_LEFT,
        RANGE_LEFTMID,
        RANGE_RIGHT,
        RANGE_RIGHTMID,
        RANGE_CENTER,
    }

    public enum Jelly_Effect_Name
    {
        Jelly_Effect,
        Pink_Jelly_Effect,
        Purple_Jelly_Effect,
        Red_Jelly_Effect,
        Twinkle_Effect,
        Big_Bear_Effect,
        Yellow_Effect
    }

    public enum SCENE_NAME
    {
        SCENE_BACKGROUND,
        SCENE_TERRIN1,
        SCENE_TERRIN2,
        SCENE_TERRIN3
    }

    public enum COOKIE_SKILL
    {
        
        COOKIE_BOOSTER,
        COOKIE_POWER_UP,
        COOKIE_INVIN,
        COOKIE_CHANGE_YELLOW_JELLY,
        COOKIE_MAGNET,
        COOKIE_MAGNETANDCHANGEJELLY,
        COOKIE_NONE
    }
}

public class CKeyValue<T, N>
{
    public CKeyValue(T _pKey, N _pValue)
    {
        m_pKey = _pKey;
        m_pValue = _pValue;
    }

    public T m_pKey;
    public N m_pValue;
}

public delegate void Callback_CollisionMessage();