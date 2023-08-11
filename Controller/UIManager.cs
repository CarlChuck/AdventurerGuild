using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
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
    private void Start()
    {
        SetAllInactive();
    }

    #region Tabs
    [Header("Tabs")]
    [SerializeField] private GameObject rosterTab;
    [SerializeField] private GameObject taskTab;
    [SerializeField] private GameObject actionTab;
    [SerializeField] private GameObject inProgressTab;
    [SerializeField] private GameObject completedTab;
    [SerializeField] private GameObject marketTab;
    [SerializeField] private GameObject recruitTab;

    public void SetTabActive(int tabNum)
    {
        SetAllInactive();
        switch (tabNum)
        {
            case 1:
                rosterTab.SetActive(true);
                break;
            case 2:
                taskTab.SetActive(true);
                break;
            case 3:
                actionTab.SetActive(true);
                break;
            case 4:
                inProgressTab.SetActive(true);
                break;
            case 5:
                completedTab.SetActive(true);
                break;
            case 6:
                marketTab.SetActive(true);
                break;
            case 7:
                recruitTab.SetActive(true);
                break;
            default:
                rosterTab.SetActive(true);
                break;
        }
    }
    private void SetAllInactive()
    {
        rosterTab.SetActive(false);
        taskTab.SetActive(false);
        actionTab.SetActive(false);
        inProgressTab.SetActive(false);
        completedTab.SetActive(false);
        marketTab.SetActive(false);
        recruitTab.SetActive(false);
    }
    #endregion

    public void StartUI(string newName, int mil, int units)
    {
        SetGuildName(newName);
        SetGoldAmount(mil, units);
    }

    #region Guild Section
    [Header("Guild Section")]
    [SerializeField] private TextMeshProUGUI guildName;
    [SerializeField] private TextMeshProUGUI goldAmount;

    public void SetGuildName(string newName)
    {
        guildName.text = newName;
    }
    public void SetGoldAmount(int mil, int units)
    {
        string goldString;
        if (mil > 0)
        {
            goldString = mil.ToString() + "," + units.ToString();
        }
        else
        {
            goldString = units.ToString();
        }
        goldAmount.text = goldString;
    }
    #endregion
    #region Roster
    [Header("Roster")]
    [SerializeField] private GameObject rosterPrefab;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform rosterContainer;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private List<UIRosterEntry> rosterList;
    [SerializeField] private List<UIItemEntry> itemList;
    [SerializeField] private GameObject itemsPane;
    [SerializeField] private GameObject rosterPane;
    [SerializeField] private GameObject adventurerPane;
    [SerializeField] private Adventurer selectedAdventurer;

    public void OnButtonRoster()
    {
        PopulateRoster(Guild.Instance.GetAdventurers());
    }
    public void OnButtonAdventurer(Adventurer adventurer)
    {
        adventurerPane.SetActive(true);
        rosterPane.SetActive(false);
        selectedAdventurer = adventurer;
    }
    public void OnButtonAdventurerBack()
    {
        adventurerPane.SetActive(false);
        rosterPane.SetActive(true);
        itemsPane.SetActive(false);
    }
    public void OnButtonInventorySlot(int iType) //0 - Weapon || 1 - Outfit || 2 - Accessory
    {
        List<Item> listOfInventoryToDisplay;
        listOfInventoryToDisplay = iType switch
        {
            0 => Guild.Instance.GetWeapons(),
            1 => Guild.Instance.GetOutfits(),
            2 => Guild.Instance.GetAccessories(),
            _ => Guild.Instance.GetWeapons()
        };
        PopulateItemList(listOfInventoryToDisplay);
        itemsPane.SetActive(true);
    }
    public void OnButtonSelectItem(Item newItem)
    {
        if (newItem.GetItemType() == ItemType.Weapon)
        {
            selectedAdventurer.EquipWeapon(newItem);
        }
        else if (newItem.GetItemType() == ItemType.Outfit)
        {
            selectedAdventurer.EquipOutfit(newItem);
        }
        else
        {
            selectedAdventurer.EquipAccessory(newItem);
        }
        itemsPane.SetActive(false);
    }
    private void ClearRoster()
    {
        foreach (UIRosterEntry child in rosterList)
        {
            Destroy(child.gameObject);
        }
    }
    private void ClearItems()
    {
        foreach (UIItemEntry child in itemList)
        {
            Destroy(child.gameObject);
        }
    }
    private void PopulateRoster(List<Adventurer> adventurers)
    {
        ClearRoster();
        foreach (Adventurer adventurer in adventurers)
        {
            GameObject newCharacter = Instantiate(rosterPrefab, rosterContainer);
            UIRosterEntry rosterEntry = newCharacter.GetComponent<UIRosterEntry>();
            rosterList.Add(rosterEntry);
            rosterEntry.SetCharName(adventurer.GetName());
            rosterEntry.SetProfession(adventurer.GetProfession());
            rosterEntry.SetStats(adventurer);
            rosterEntry.SetCharValue(adventurer.GetCharacterValue());
        }
    }
    private void PopulateItemList(List<Item> listOfItemsIn)
    {
        ClearItems();
        foreach (Item item in listOfItemsIn)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer);
            UIItemEntry iEntry = newItem.GetComponent<UIItemEntry>();
            itemList.Add(iEntry);
            iEntry.SetItem(item);
        }
    }
    #endregion
    #region Tasks

    #endregion
    #region Actions

    #endregion
    #region InProgress

    #endregion
    #region Completed

    #endregion
    #region Market

    #endregion
    #region Recruit

    #endregion
}
