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
    [SerializeField] private UIFullCharacter uiForSelectedAdventurer;
    [SerializeField] private GameObject unequipItemButtonPrefab;
    [SerializeField] private GameObject unequipItemButtonInstance;

    public void OnButtonRoster()
    {
        PopulateRoster(Guild.Instance.GetAdventurers());
        rosterPane.SetActive(true);
        adventurerPane.SetActive(false);
        itemsPane.SetActive(false);
    }
    public void OnButtonAdventurer(Adventurer adventurer)
    {
        adventurerPane.SetActive(true);
        rosterPane.SetActive(false);
        selectedAdventurer = adventurer;
        uiForSelectedAdventurer.SetStats(adventurer);
    }
    public void OnButtonAdventurerBack()
    {
        OnButtonRoster();
        rosterPane.SetActive(true);
    }
    public void OnButtonInventorySlot(int iType) //0 - Weapon || 1 - Outfit || 2 - Accessory
    {
        List<Item> listOfInventoryToDisplay;
        switch (iType)
        {
            case 0:
                listOfInventoryToDisplay = Guild.Instance.GetWeapons();
                PopulateItemList(listOfInventoryToDisplay, ItemType.Weapon);
                break;
            case 1:
                listOfInventoryToDisplay = Guild.Instance.GetOutfits();
                PopulateItemList(listOfInventoryToDisplay, ItemType.Outfit);
                break;
            case 2:
                listOfInventoryToDisplay = Guild.Instance.GetAccessories();
                PopulateItemList(listOfInventoryToDisplay, ItemType.Accessory);
                break;
            default:
                listOfInventoryToDisplay = Guild.Instance.GetWeapons();
                PopulateItemList(listOfInventoryToDisplay, ItemType.Weapon);
                break;
        }
        itemsPane.SetActive(true);
    }
    public void OnButtonUnEquip(ItemType iType)
    {
        if (iType == ItemType.Weapon)
        {
            selectedAdventurer.UnEquipWeapon();
        }
        else if (iType == ItemType.Outfit)
        {
            selectedAdventurer.UnEquipOutfit();
        }
        else
        {
            selectedAdventurer.UnEquipAccessory();
        }
        itemsPane.SetActive(false);
        uiForSelectedAdventurer.SetStats(selectedAdventurer);
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
        uiForSelectedAdventurer.SetStats(selectedAdventurer);
    }
    private void ClearRoster()
    {
        foreach (UIRosterEntry child in rosterList)
        {
            Destroy(child.gameObject);
        }
        rosterList.Clear();
    }
    private void ClearItems()
    { 
        foreach (UIItemEntry child in itemList)
        {
            Destroy(child.gameObject);
        }
        itemList.Clear();
        Destroy(unequipItemButtonInstance);
    }
    private void PopulateRoster(List<Adventurer> adventurers)
    {
        ClearRoster();
        foreach (Adventurer adventurer in adventurers)
        {
            GameObject newCharacter = Instantiate(rosterPrefab, rosterContainer);
            UIRosterEntry rosterEntry = newCharacter.GetComponent<UIRosterEntry>();
            rosterList.Add(rosterEntry);
            rosterEntry.SetStats(adventurer);
        }
    }
    private void PopulateItemList(List<Item> listOfItemsIn, ItemType iType)
    {
        ClearItems();

        unequipItemButtonInstance = Instantiate(unequipItemButtonPrefab, itemContainer);
        UIItemEntry unequipBut = unequipItemButtonInstance.GetComponent<UIItemEntry>();
        unequipBut.SetItemType(iType);
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
    [Header("Tasks")]
    [SerializeField] private Transform taskContainer;
    [SerializeField] private GameObject taskitemPrefab;

    #endregion
    #region Actions
    [Header("Actions")]
    [SerializeField] private Transform actionContainer;
    [SerializeField] private GameObject actionitemPrefab;

    #endregion
    #region InProgress
    [Header("In Progress")]
    [SerializeField] private Transform inProgressContainer;
    [SerializeField] private GameObject inProgressitemPrefab;

    #endregion
    #region Completed
    [Header("Completed")]
    [SerializeField] private Transform completedContainer;
    [SerializeField] private GameObject completeditemPrefab;

    #endregion
    #region Market
    [Header("Market")]
    [SerializeField] private Transform marketContainer;
    [SerializeField] private GameObject marketItemPrefab;
    [SerializeField] private List<UIMarketItemEntry> marketList;

    public void OnButtonMarket()
    {
        PopulateMarketList(Markets.Instance.GetItemList());
    }
    public void OnButtonBuyItem(Item item)
    {
        Markets.Instance.OnBuyItem(item);
        OnButtonMarket();
    }
    public void PopulateMarketList(List<Item> listOfItemsIn)
    {
        ClearMarketList();
        foreach (Item item in listOfItemsIn)
        {
            GameObject newItem = Instantiate(marketItemPrefab, marketContainer);
            UIMarketItemEntry itemEntry = newItem.GetComponent<UIMarketItemEntry>();
            marketList.Add(itemEntry);
            itemEntry.SetItem(item);
        }
    }
    public void ClearMarketList()
    {
        foreach (UIMarketItemEntry child in marketList)
        {
            Destroy(child.gameObject);
        }
        marketList.Clear();
    }

    #endregion
    #region Recruit

    [Header("Recruit")]
    [SerializeField] private Transform recruitContainer;
    [SerializeField] private GameObject marketRecruitPrefab;
    [SerializeField] private List<UIMarketRecruitEntry> recruitList;

    public void OnButtonRecruit()
    {
        PopulateRecruitList(Markets.Instance.GetAdventurerList());
    }
    public void OnButtonBuyRecruit(Adventurer adventurer)
    {
        Markets.Instance.OnBuyAdventurer(adventurer);
        OnButtonRecruit();
    }
    public void PopulateRecruitList(List<Adventurer> ListOfadventurersIn)
    {
        ClearRecruitList();
        foreach (Adventurer adventurer in ListOfadventurersIn)
        {
            GameObject newRecruit = Instantiate(marketRecruitPrefab, recruitContainer);
            UIMarketRecruitEntry rosterEntry = newRecruit.GetComponent<UIMarketRecruitEntry>();
            recruitList.Add(rosterEntry);
            rosterEntry.SetStats(adventurer);
        }
    }
    public void ClearRecruitList()
    {
        foreach (UIMarketRecruitEntry child in recruitList)
        {
            Destroy(child.gameObject);
        }
        recruitList.Clear();
    }
    #endregion
    public void NotEnoughGold()
    {
        //TODO
    }
}
