using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour
{
    #region Singleton
    public static Guild Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    private string guildName;
    private int gold;
    private int goldMil;

    public void AddGold(int goldToAdd)
    {
        gold += goldToAdd;
        int mil = 1000000;
        if (gold > mil)
        {
            gold -= mil;
            goldMil += 1;
        }
    }
    public void RemoveGold(int goldToTake)
    {
        int mil = 1000000;
        if (goldMil > 0)
        {
            gold += mil;
            goldMil -= 1;
        }
        gold -= goldToTake;
        if (gold > mil)
        {
            gold -= mil;
            goldMil += 1;
        }
    }
    #region Setters
    public void SetGuildName(string newName)
    {
        guildName = newName;
    }
    public void SetStartingGold(int startGold)
    {
        gold = startGold;
        goldMil = 0;
    }
    #endregion
    #region Getters
    public string GetGuildName()
    {
        return guildName;
    }
    public string GetGoldAsString()
    {
        return goldMil.ToString() + gold.ToString();
    }
    #endregion
}
