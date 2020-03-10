using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private static int score, deaths, shoots;

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    public static int Deaths
    {
        get
        {
            return deaths;
        }
        set
        {
            deaths = value;
        }
    }

    public static int Shoots
    {
        get
        {
            return shoots;
        }
        set
        {
            shoots = value;
        }
    }

}
