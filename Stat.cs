using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stat
{
    private int value;

    public void SetValue(int valueToSet)
    {
        value = valueToSet;
    }
    public void AddValue(int valueToAdd)
    {
        value += valueToAdd;
    }
    public int GetValue()
    {
        return value;
    }
}
