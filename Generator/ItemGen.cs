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
            newItem.SetItemQuality(Quality.Common);
        }
        else if (rand < 901)
        {
            newItem.SetItemQuality(Quality.Uncommon);
        }
        else if (rand < 976)
        {
            newItem.SetItemQuality(Quality.Masterwork);
        }
        else if (rand < 996)
        {
            newItem.SetItemQuality(Quality.Rare);
        }
        else
        {
            newItem.SetItemQuality(Quality.Legendary);
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

    #region Save System Integration
    
    public ItemSO GetItemTemplateByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;
        
        foreach (ItemSO itemTemplate in itemTemplates)
        {
            if (itemTemplate != null && itemTemplate.name == itemName)
                return itemTemplate;
        }
        
        foreach (ItemSO itemTemplate in starterItemList)
        {
            if (itemTemplate != null && itemTemplate.name == itemName)
                return itemTemplate;
        }
        
        Debug.LogWarning($"Item template '{itemName}' not found, using default");
        return itemTemplates.Count > 0 ? itemTemplates[0] : null;
    }
    
    public Item RecreateItem(ItemSaveData saveData, Transform parent)
    {
        if (saveData == null) return null;
        
        ItemSO template = GetItemTemplateByName(saveData.name);
        if (template == null)
        {
            Debug.LogError($"Cannot recreate item '{saveData.name}' - missing template");
            return null;
        }
        
        GameObject newItemObject = Instantiate(itemPrefab, parent);
        Item newItem = newItemObject.GetComponent<Item>();
        
        newItem.SetItem(template);
        
        if (System.Enum.TryParse<Quality>(saveData.quality, out Quality quality))
        {
            newItem.SetItemQuality(quality);
        }
        
        if (saveData.stats != null)
        {
            newItem.SetCombat(saveData.stats.combat);
            newItem.SetHealing(saveData.stats.healing);
            newItem.SetSocial(saveData.stats.social);
            newItem.SetSubterfuge(saveData.stats.subterfuge);
            newItem.SetHunting(saveData.stats.hunting);
            newItem.SetMagic(saveData.stats.magic);
            newItem.SetCraft(saveData.stats.craft);
        }
        
        UniqueId uniqueId = newItem.GetComponent<UniqueId>();
        if (uniqueId == null)
        {
            uniqueId = newItem.gameObject.AddComponent<UniqueId>();
        }
        uniqueId.SetId(saveData.id);
        
        newItem.SetEquipped(saveData.isEquipped, saveData.equippedByAdventurer);
        
        return newItem;
    }
    
    #endregion
}
