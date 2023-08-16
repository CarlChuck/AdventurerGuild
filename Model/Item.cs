using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string itemName;
    private ItemSO itemBase;
    private ItemType itemType;
    private ItemQuality itemQuality;
    private Stat combat;
    private Stat healing;
    private Stat social;
    private Stat subterfuge;
    private Stat hunting;
    private Stat magic;
    private Stat craft;
    private int goldValue;


    public void SetItem(ItemSO item)
    {
        InitialiseStats();
        itemBase = item;
        itemName = item.name;
        name = item.name;
        itemType = item.itemType;
        combat.SetValue(item.combat);
        healing.SetValue(item.healing);
        social.SetValue(item.social);
        subterfuge.SetValue(item.subterfuge);
        hunting.SetValue(item.hunting);
        magic.SetValue(item.magic);
        craft.SetValue(item.craft);
        SetItemQuality(ItemQuality.Common);
    }
    private void InitialiseStats()
    {
        combat = Instantiate(Guild.Instance.statPrefab, transform);
        healing = Instantiate(Guild.Instance.statPrefab, transform);
        social = Instantiate(Guild.Instance.statPrefab, transform);
        subterfuge = Instantiate(Guild.Instance.statPrefab, transform);
        hunting = Instantiate(Guild.Instance.statPrefab, transform);
        magic = Instantiate(Guild.Instance.statPrefab, transform);
        craft = Instantiate(Guild.Instance.statPrefab, transform);

        combat.InitialiseStat("Combat");
        healing.InitialiseStat("Healing");
        social.InitialiseStat("Social");
        subterfuge.InitialiseStat("Subterfuge");
        hunting.InitialiseStat("Hunting");
        magic.InitialiseStat("Magic");
        craft.InitialiseStat("Craft");
    }
    public void SetItemQuality(ItemQuality qual)
    {
        itemQuality = qual;
        AddQuality();
    }
    private void AddQuality()
    {
        //TODO add appropriate stats
        SetGoldValue();
    }

    private void SetGoldValue()
    {
        int rand = Random.Range(1, 5);
        int multiplier = rand switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 8,
            _ => 1
        };
        goldValue = itemBase.value * multiplier;
    }
    public void GetStats(out int comb, out int heal, out int soc, out int subt, out int hunt, out int mag, out int craf)
    {
        comb = combat.GetValue();
        heal = healing.GetValue();
        soc = social.GetValue();
        subt = subterfuge.GetValue();
        hunt = hunting.GetValue();
        mag = magic.GetValue();
        craf = craft.GetValue();
    }

    public int GetCombat()
    {
        return combat.GetValue();
    }
    public int GetHealing()
    {
        return healing.GetValue();
    }
    public int GetSocial()
    {
        return social.GetValue();
    }
    public int GetSubterfuge()
    {
        return subterfuge.GetValue();
    }
    public int GetHunting()
    {
        return hunting.GetValue();
    }
    public int GetMagic()
    {
        return magic.GetValue();
    }
    public int GetCraft()
    {
        return craft.GetValue();
    }

    public string GetItemName()
    {
        return itemName;
    }
    public ItemType GetItemType()
    {
        return itemType;
    }
}
public enum ItemType { Weapon, Outfit, Accessory}
public enum ItemQuality { Common, Uncommon, Masterwork, Rare, Legendary}