using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGen : MonoBehaviour
{
    #region Singleton
    public static ItemGen Instance;
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
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<ItemSO> itemTemplates;
    [SerializeField] private List<ItemSO> starterItemList;

    public Item GenerateRandomItem(Transform parent)
    {
        GameObject newItemObject = Instantiate(itemPrefab, parent);
        Item newItem = newItemObject.GetComponent<Item>();
        newItem.SetItem(GetRandomItemType());
        return newItem;
    }
    public Item GenerateStarterItem(Transform parent)
    {
        GameObject newItemObject = Instantiate(itemPrefab, parent);
        Item newItem = newItemObject.GetComponent<Item>();
        newItem.SetItem(GetRandomStarterItemType());
        return newItem;
    }
    public Item GenerateRandomLootItem(Transform parent)
    {
        Item newItem = GenerateRandomItem(parent);
        int rand = Random.Range(1, 1001);
        if (rand < 701)
        {
            newItem.SetItemQuality(ItemQuality.Common);
        }
        else if (rand < 901)
        {
            newItem.SetItemQuality(ItemQuality.Uncommon);
        }
        else if (rand < 976)
        {
            newItem.SetItemQuality(ItemQuality.Masterwork);
        }
        else if (rand < 996)
        {
            newItem.SetItemQuality(ItemQuality.Rare);
        }
        else
        {
            newItem.SetItemQuality(ItemQuality.Legendary);
        }
        return newItem;
    }

    private ItemSO GetRandomItemType()
    {
        int rand = Random.Range(0, itemTemplates.Count);
        ItemSO itemToReturn = itemTemplates[rand];
        return itemToReturn;
    }
    private ItemSO GetRandomStarterItemType()
    {
        int rand = Random.Range(0, starterItemList.Count);
        ItemSO itemToReturn = starterItemList[rand];
        return itemToReturn;
    }
}
