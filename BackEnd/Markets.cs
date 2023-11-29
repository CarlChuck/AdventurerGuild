using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markets : MonoBehaviour
{
    #region Singleton
    public static Markets Instance;
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
        marketRecruits = new();
        marketItems = new();
    }
    #endregion
    [SerializeField] private Transform marketItemsParent;
    [SerializeField] private List<Adventurer> marketRecruits;
    [SerializeField] private List<Item> marketItems;
    private bool marketOpen;

    #region Public
    public void InitialiseMarket(List<Adventurer> savedMarketAdventurers = null, List<Item> savedMarketItems = null)
    {
        marketOpen = false;
        if (savedMarketAdventurers == null)
        {
            GenerateAdventurerList();
        }
        else
        {
            foreach (Adventurer adventurer in savedMarketAdventurers)
            {
                marketRecruits.Add(adventurer);
                adventurer.transform.SetParent(marketItemsParent);
            }
        }

        if (savedMarketItems == null)
        {
            GenerateItemList();
        }
        else
        {
            foreach (Item item in savedMarketItems)
            {
                marketItems.Add(item);
                item.transform.SetParent(marketItemsParent);
            }
        }
        StartCoroutine(MarketHeartBeat());
    }
    public List<Adventurer> GetAdventurerList()
    {
        return marketRecruits;
    }
    public List<Item> GetItemList()
    {
        return marketItems;
    }
    public void OnBuyAdventurer(Adventurer adventurer)
    {
        if (adventurer.GetCharacterValue() < Guild.Instance.GetGold())
        {
            marketRecruits.Remove(adventurer);
            Guild.Instance.AddAdventurer(adventurer);
            Guild.Instance.RemoveGold(adventurer.GetCharacterValue());
        }
        else
        {
            UIManager.Instance.NotEnoughGold();
        }

    }
    public void OnBuyItem(Item item)
    {
        if (item.GetGoldValue() < Guild.Instance.GetGold())
        {
            marketItems.Remove(item);
            Guild.Instance.AddItemToInventory(item);
            Guild.Instance.RemoveGold(item.GetGoldValue());
        }
        else
        {
            UIManager.Instance.NotEnoughGold();
        }

    }
    #endregion

    private void GenerateAdventurerList()
    {
        int numberOfRecruits = 10;
        ClearRecruits();
        for (int i = 0; i < numberOfRecruits; i++)
        {
            Adventurer newAdventurer = CharGen.Instance.GenerateRandomCharacter(marketItemsParent);
            marketRecruits.Add(newAdventurer);
        }
    }
    private void GenerateItemList()
    {
        int numberOfItems = 20;
        ClearItems();
        for (int i = 0; i < numberOfItems; i++)
        {
            Item newItem = ItemGen.Instance.GenerateRandomItem(marketItemsParent);
            marketItems.Add(newItem);
        }
    }
    private void ClearRecruits()
    {
        foreach (Adventurer adventurer in marketRecruits)
        {
            Destroy(adventurer.gameObject);
        }
        marketRecruits.Clear();
    }
    private void ClearItems()
    {
        foreach (Item item in marketItems)
        {
            Destroy(item.gameObject);
        }
        marketItems.Clear();
    }

    #region Market Open and Close, and Heartbeat
    public void OnMarketOpen()
    {
        marketOpen = true;
    }
    public void OnMarketClose()
    {
        marketOpen = false;
    }
    private IEnumerator MarketHeartBeat()
    {
        yield return new WaitForSeconds(1800);
        StartCoroutine(ToRefreshMarket());
    }
    private IEnumerator ToRefreshMarket()
    {
        yield return new WaitForSeconds(60);
        if (marketOpen == true)
        {
            StartCoroutine(ToRefreshMarket());
        }
        else
        {
            GenerateAdventurerList();
            GenerateItemList();
            StartCoroutine(MarketHeartBeat());
        }
    }
    #endregion
}