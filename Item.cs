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
    private Stat crafting;
    private int goldValue;


    public void SetItem(ItemSO item)
    {
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
        crafting.SetValue(item.craft);
    }

    public void SetItemQuality(int qNum)
    {
        itemQuality = qNum switch
        {
            0 => ItemQuality.Common,
            1 => ItemQuality.Uncommon,
            2 => ItemQuality.Masterwork,
            3 => ItemQuality.Rare,
            4 => ItemQuality.Legendary,
            _ => ItemQuality.Common
        };
    }

    public void GetStats(out int comb, out int heal, out int soc, out int subt, out int hunt, out int mag, out int craf)
    {
        comb = combat.GetValue();
        heal = healing.GetValue();
        soc = social.GetValue();
        subt = subterfuge.GetValue();
        hunt = hunting.GetValue();
        mag = magic.GetValue();
        craf = crafting.GetValue();
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