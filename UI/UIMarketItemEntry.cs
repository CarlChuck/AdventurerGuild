using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMarketItemEntry : MonoBehaviour
{
    [SerializeField] private Item itemReference;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemStats;
    [SerializeField] private TextMeshProUGUI itemValue;

    public void SetItem(Item item)
    {
        itemReference = item;
        itemName.text = item.name;
        string textToPresent = "";
        itemReference.GetStats(out int combat, out int healing, out int social, out int subterfuge, out int hunting, out int magic, out int craft);
        if (combat > 0)
        {
            textToPresent = "Combat: " + combat.ToString() + " ";
        }
        if (healing > 0)
        {
            textToPresent += "Healing: " + healing.ToString() + " ";
        }
        if (social > 0)
        {
            textToPresent += "Social: " + social.ToString() + " ";
        }
        if (subterfuge > 0)
        {
            textToPresent += "Subterfuge: " + subterfuge.ToString() + " ";
        }
        if (hunting > 0)
        {
            textToPresent += "Hunting: " + hunting.ToString() + " ";
        }
        if (magic > 0)
        {
            textToPresent += "Magic: " + magic.ToString() + " ";
        }
        if (craft > 0)
        {
            textToPresent += "Craft: " + craft.ToString() + " ";
        }
        itemStats.text = textToPresent;
        itemValue.text = item.GetGoldValue().ToString();
        SetItemColour();
    }

    public void OnButtonBuy()
    {
        UIManager.Instance.OnButtonBuyItem(itemReference);
    }
    private void SetItemColour()
    {
        ItemQuality iQuality = itemReference.GetItemQuality();
        //TODO ColourStuff
    }
}
