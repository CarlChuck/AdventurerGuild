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
        AddQuality(qual);
    }
    private void AddQuality(ItemQuality qual)
    {
        switch (qual)
        {
            case ItemQuality.Common:
                break;
            case ItemQuality.Uncommon:
                AddAStatToItem(1);
                break;
            case ItemQuality.Masterwork:
                AddAStatToItem(2);
                break;
            case ItemQuality.Rare:
                AddAStatToItem(3);
                break;
            case ItemQuality.Legendary:
                AddAStatToItem(4);
                break;
            default:
                break;
        }
        SetGoldValue(qual);
    }
    private void AddAStatToItem(int numberToAdd)
    {
        for (int i = 0; i < numberToAdd; i++)
        {
            int rand = Random.Range(1, 4);
            if (rand < 3)
            {
                GetHighestStat().AddValue(Random.Range(1, 3));
            }
            else
            {
                GetSecondHighestStat().AddValue(1);
            }
        }
    }

    private void SetGoldValue(ItemQuality qual)
    {
        int multiplier = qual switch
        {
            ItemQuality.Common => 1,
            ItemQuality.Uncommon => 2,
            ItemQuality.Masterwork => 4,
            ItemQuality.Rare => 8,
            ItemQuality.Legendary => 16,
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
    public Stat GetHighestStat()
    {
        Stat highestStat = combat;
        if (healing.GetValue() > highestStat.GetValue())
        {
            highestStat = healing;
        }
        if (social.GetValue() > highestStat.GetValue())
        {
            highestStat = social;
        }
        if (subterfuge.GetValue() > highestStat.GetValue())
        {
            highestStat = subterfuge;
        }
        if (hunting.GetValue() > highestStat.GetValue())
        {
            highestStat = hunting;
        }
        if (magic.GetValue() > highestStat.GetValue())
        {
            highestStat = magic;
        }
        if (craft.GetValue() > highestStat.GetValue())
        {
            highestStat = craft;
        }
        return highestStat;
    }
    public Stat GetSecondHighestStat()
    {
        Stat highestStat = GetHighestStat();
        Stat secondhighestStat = combat;
        if (healing.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != healing)
            {
                secondhighestStat = healing;
            }
        }
        if (social.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != social)
            {
                secondhighestStat = social;
            }
        }
        if (subterfuge.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != subterfuge)
            {
                secondhighestStat = subterfuge;
            }
        }
        if (hunting.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != hunting)
            {
                secondhighestStat = hunting;
            }
        }
        if (magic.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != magic)
            {
                secondhighestStat = magic;
            }
        }
        if (craft.GetValue() > secondhighestStat.GetValue())
        {
            if (highestStat != craft)
            {
                secondhighestStat = craft;
            }
        }
        return secondhighestStat;
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
    public int GetGoldValue()
    {
        return goldValue;
    }
    public ItemQuality GetItemQuality()
    {
        return itemQuality;
    }
}
public enum ItemType { Weapon, Outfit, Accessory}
public enum ItemQuality { Common, Uncommon, Masterwork, Rare, Legendary}