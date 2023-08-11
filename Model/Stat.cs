using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
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
    public void InitialiseStat(string theNameToSet)
    {
        name = theNameToSet;
        value = 0;
    }
    public string GetStatName()
    {
        return name;
    }
}
