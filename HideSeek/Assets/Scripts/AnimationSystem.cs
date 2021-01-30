using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationSystem
{
    private static bool sewAnim, fridgeAnim, chainAnim, tvAnim, casetteAnim, clockAnim;

    public static bool SewAnim
    {
        get
        {
            return sewAnim;
        }
        set
        {
            sewAnim = value;
        }
    }

    public static bool FridgeAnim
    {
        get
        {
            return fridgeAnim;
        }
        set
        {
            fridgeAnim = value;
        }
    }

    public static bool ChairAnim
    {
        get
        {
            return chainAnim;
        }
        set
        {
            chainAnim = value;
        }
    }

    public static bool TVAnim
    {
        get
        {
            return tvAnim;
        }
        set
        {
            tvAnim = value;
        }
    }

    public static bool CasetteAnim
    {
        get
        {
            return casetteAnim;
        }
        set
        {
            casetteAnim = value;
        }
    }

    public static bool ClockAnim
    {
        get
        {
            return clockAnim;
        }
        set
        {
            clockAnim = value;
        }
    }
}
