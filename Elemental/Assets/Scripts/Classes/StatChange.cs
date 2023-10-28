using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChange
{
    #region Properties
    [SerializeField]
    private string StatName;
    [SerializeField]
    private int NumChange;
    [SerializeField]
    private int ChangeDuration;
    [SerializeField]
    private bool IsPercentChange;

    public string GetStatName()
    {
        return StatName;
    }

    public void SetStatName(string value)
    {
        StatName = value;
    }

    public int GetNumChange()
    {
        return NumChange;
    }

    public void SetNumChange(int value)
    {
        NumChange = value;
    }

    public int GetChangeDuration()
    {
        return ChangeDuration;
    }

    public void SetChangeDuration(int value)
    {
        ChangeDuration = value;
    }

    public bool GetIsPercentChange()
    {
        return IsPercentChange;
    }

    public void SetIsPercentChange(bool value)
    {
        IsPercentChange = value;
    }
    #endregion

    public int ChangeStat(int stat, int change, bool isPercentage)
    {
        //HOW DO I WANT TO HANDLE ROUNDING?
        if(isPercentage)
        {
            IsPercentChange = true;
            NumChange = stat * change;
            return stat + NumChange;
        }
        else
        {
            NumChange = stat + change;
            return stat + NumChange;
        }
    }
}
