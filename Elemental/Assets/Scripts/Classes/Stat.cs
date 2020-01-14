using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    // Start is called before the first frame update
    [SerializeField]
    private float baseStat;
    [SerializeField]
    private string statInfo;

    public float GetStat()
    {
        return baseStat;
    }

    public string GetInfo()
    {
        return statInfo;
    }

    public void updateStat(float change)
    {
        baseStat = baseStat + change;
    }
}
