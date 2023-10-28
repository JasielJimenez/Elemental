using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    #region Properties
    [SerializeField]
    private int baseStat;
    [SerializeField]
    private int currStat;
    [SerializeField]
    private string statInfo;

    public int GetBaseStat()
    {
        return baseStat;
    }

    public void SetBaseStat(int value)
    {
        baseStat = value;
    }

    public int GetCurrStat()
    {
        return currStat;
    }

    public void SetCurrStat(int value)
    {
        currStat = value;
    }

    public string GetInfo()
    {
        return statInfo;
    }

    public void SetInfo(string value)
    {
        statInfo = value;
    }
    #endregion

    public void UpdateCurrStat(int change)
    {
        if(currStat + change < 0)
        {
            currStat = 0;
        }
        else
        {
            currStat = currStat + change;
        }
    }
}
