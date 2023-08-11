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

    [SerializeField] private List<Item> items;
    [SerializeField] private List<Adventurer> adventurers;
    public Stat statPrefab;

    public void StartGuild(string gName)
    {
        ClearAdventurers();
        ClearItems();
        items = new();
        adventurers = new();
        guildName = gName;
        gold = 10000;
        goldMil = 0;
        AddStartingAdventurers();
        AddStartingItems();
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
    public int GetGold()
    {
        return gold;
    }
    public int GetGoldMil()
    {
        return goldMil;
    }
    public string GetGoldAsString()
    {
        return goldMil.ToString() + gold.ToString();
    }
    public List<Item> GetItems()
    {
        return items;
    }
    public List<Item> GetWeapons()
    {
        List<Item> weapList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Weapon)
            {
                weapList.Add(item);
            }
        }
        return weapList;
    }
    public List<Item> GetOutfits()
    {
        List<Item> outfitList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Outfit)
            {
                outfitList.Add(item);
            }
        }
        return outfitList;
    }
    public List<Item> GetAccessories()
    {
        List<Item> accessList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Accessory)
            {
                accessList.Add(item);
            }
        }
        return accessList;
    }
    public List<Adventurer> GetAdventurers()
    {
        return adventurers;
    }
    #endregion
    #region Adventurers
    public void AddAdventurer(Adventurer adventurer)
    {
        adventurers.Add(adventurer);
    }
    public void RemoveAdventurer(Adventurer adventurer)
    {
        adventurers.Remove(adventurer);
    }
    public void ClearAdventurers()
    {
        if (adventurers.Count > 0)
        {
            foreach (Adventurer adventurer in adventurers)
            {
                Destroy(adventurer.gameObject);
            }
            adventurers.Clear();
        }
    }
    private void AddStartingAdventurers()
    {
        int startingCharacters = 10;
        for (int i = 0; i < startingCharacters; i++)
        {
            Adventurer newAdventurer = CharGen.Instance.GenerateStarterCharacter();
            adventurers.Add(newAdventurer);
        }
    }
    #endregion
    #region Inventory
    public void AddItemToInventory(Item item)
    {
        items.Add(item);
    }
    public void RemoveItemFromInventory(Item item)
    {
        items.Remove(item);
    }
    private void AddStartingItems()
    {
        int startingItems = 5;
        for (int i = 0; i < startingItems; i++)
        {
            Item newItem = ItemGen.Instance.GenerateStarterItem();
            items.Add(newItem);
        }
    }
    public void DestroyItem(Item item)
    {
        Destroy(item.gameObject);
    }
    public void ClearItems()
    {
        if (items.Count > 0)
        {
            foreach (Item item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }
    }
    #endregion
    #region Gold
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
    #endregion

}
