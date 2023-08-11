using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemEntry : MonoBehaviour
{
    [SerializeField] Item itemReference;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemStats;
    [SerializeField] private TextMeshProUGUI itemValue;


    public void SetItem(Item item)
    {
        itemReference = item;
        itemName.text = item.name;
    }
    public void SetItemStats(Item item)
    {

    }
    public void OnButtonPress()
    {
        UIManager.Instance.OnButtonSelectItem(itemReference);
    }
}
