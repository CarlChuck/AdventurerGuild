using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    [SerializeField] private string adventurerName;
    [SerializeField] private SpeciesSO species;
    [SerializeField] private ProfessionSO profession;
    [SerializeField] private int experience;
    [SerializeField] private int level;
    [SerializeField] private int charValue;
    [SerializeField] private Stat combat;
    [SerializeField] private Stat healing;
    [SerializeField] private Stat social;
    [SerializeField] private Stat subterfuge;
    [SerializeField] private Stat hunting;
    [SerializeField] private Stat magic;
    [SerializeField] private Stat craft;
    [SerializeField] private IfCrafter craftType;

    [SerializeField] private Item weaponSlot;
    [SerializeField] private Item outfitSlot;
    [SerializeField] private Item accessorySlot;
    private bool genderM;

    #region Core Generation
    public void GenerateStartingCharacter(SpeciesSO spec, ProfessionSO prof)
    {
        InitialiseStats();
        species = spec;
        profession = prof;
        GenerateStats();
        experience = 0;
        AddLevel();
        SetGender();
        SetCraft();
        SetAdventurerName();
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
    private void GenerateStats()
    {
        AddSpeciesStat(combat, species.combat);
        AddSpeciesStat(healing, species.healing);
        AddSpeciesStat(social, species.social);
        AddSpeciesStat(subterfuge, species.subterfuge);
        AddSpeciesStat(hunting, species.hunting);
    }
    private void AddProfessionStat(Stat stat, int amount)
    {
        var statAmountToAdd = amount switch
        {
            0 => Random.Range(0, 3),
            1 => Random.Range(1, 5),
            2 => Random.Range(2, 7),
            3 => Random.Range(3, 9),
            4 => Random.Range(4, 11),
            5 => Random.Range(5, 13),
            _ => Random.Range(0, 3),
        };
        stat.AddValue(statAmountToAdd);
    }
    private void AddSpeciesStat(Stat stat, int amount)
    {
        var statAmountToAdd = amount switch
        {
            0 => 0,
            1 => Random.Range(1, 5),
            _ => 0,
        };
        stat.AddValue(statAmountToAdd);
    }
    private void SetGender()
    {
        int rand = Random.Range(1, 3);
        if (rand > 1)
        {
            genderM = true;
        }
        else
        {
            genderM = false;
        }
    }
    private void SetCraft()
    {
        craftType = profession.ifCrafter;
        if (craftType == IfCrafter.NA)
        {
            int rand = Random.Range(1, 8);
            craftType = rand switch
            {
                1 => IfCrafter.Weaponsmith,
                2 => IfCrafter.Armoursmith,
                3 => IfCrafter.Leatherworker,
                4 => IfCrafter.Tailor,
                5 => IfCrafter.Fletcher,
                6 => IfCrafter.Jeweller,
                7 => IfCrafter.Carpenter,
                _ => IfCrafter.Carpenter
            };
        }
    }
    private void SetAdventurerName()
    {
        if (NameGen.Instance.AreNamesLoaded())
        {
            adventurerName = NameGen.Instance.GenerateRandomName(genderM);
        }
        else
        {
            adventurerName = "Unnamed Adventurer";
        }
    }
    #endregion
    #region XP and Level
    //Includes adding a level if necessary
    public void AddExperience(int xpValue)
    {
        experience += xpValue;
        int targetLevel;
        if (experience < 11)
        {
            targetLevel = 1;
        }
        else if (experience < 26)
        {
            targetLevel = 2;
        }
        else if (experience < 51)
        {
            targetLevel = 3;
        }
        else if (experience < 101)
        {
            targetLevel = 4;
        }
        else
        {
            targetLevel = 5;
        }
        AddLevelsUpToValue(targetLevel);        
    }
    private void AddLevelsUpToValue(int targetLevel)
    {
        if (level < targetLevel)
        {
            AddLevel();
            targetLevel -= 1;
            if (level < targetLevel)
            {
                AddLevelsUpToValue(targetLevel);
            }
        }
    }
    private void AddLevel()
    {
        if (level <= 5)
        {
            AddProfessionStat(combat, profession.combat);
            AddProfessionStat(healing, profession.healing);
            AddProfessionStat(social, profession.social);
            AddProfessionStat(subterfuge, profession.subterfuge);
            AddProfessionStat(hunting, profession.hunting);
            AddProfessionStat(magic, profession.magic);
            AddProfessionStat(craft, profession.craft);
            level++;
        }
        UpdateCharacterValue();
    }
    private void UpdateCharacterValue()
    {
        int statTotal = combat.GetValue() + healing.GetValue() + social.GetValue() + subterfuge.GetValue() + hunting.GetValue() + magic.GetValue() + craft.GetValue();
        charValue = 200 + (statTotal * 20);
    }
    #endregion
    #region Getters
    public string GetName()
    {
        return adventurerName;
    }
    public string GetSpecies()
    {
        return species.name;
    }
    public string GetProfession()
    {
        return profession.name;
    }
    public int GetExperience()
    {
        return experience;
    }
    public int GetLevel()
    {
        return level;
    }
    public int GetCombat()
    {
        int combatValue = combat.GetValue();
        if (weaponSlot != null)
        {
            combatValue += weaponSlot.GetCombat();
        }
        if (outfitSlot != null)
        {
            combatValue += outfitSlot.GetCombat();
        }
        if (accessorySlot != null)
        {
            combatValue += accessorySlot.GetCombat();
        }
        return combatValue;
    }
    public int GetHealing()
    {
        int healValue = healing.GetValue();
        if (weaponSlot != null)
        {
            healValue += weaponSlot.GetHealing();
        }
        if (outfitSlot != null)
        {
            healValue += outfitSlot.GetHealing();
        }
        if (accessorySlot != null)
        {
            healValue += accessorySlot.GetHealing();
        }
        return healValue;
    }
    public int GetSocial()
    {
        int socValue = social.GetValue();
        if (weaponSlot != null)
        {
            socValue += weaponSlot.GetSocial();
        }
        if (outfitSlot != null)
        {
            socValue += outfitSlot.GetSocial();
        }
        if (accessorySlot != null)
        {
            socValue += accessorySlot.GetSocial();
        }
        return socValue;
    }
    public int GetSubterfuge()
    {
        int subtValue = subterfuge.GetValue();
        if (weaponSlot != null)
        {
            subtValue += weaponSlot.GetSubterfuge();
        }
        if (outfitSlot != null)
        {
            subtValue += outfitSlot.GetSubterfuge();
        }
        if (accessorySlot != null)
        {
            subtValue += accessorySlot.GetSubterfuge();
        }
        return subtValue;
    }
    public int GetHunting()
    {
        int huntValue = hunting.GetValue();
        if (weaponSlot != null)
        {
            huntValue += weaponSlot.GetHunting();
        }
        if (outfitSlot != null)
        {
            huntValue += outfitSlot.GetHunting();
        }
        if (accessorySlot != null)
        {
            huntValue += accessorySlot.GetHunting();
        }
        return huntValue;
    }
    public int GetMagic()
    {
        int magicValue = magic.GetValue();
        if (weaponSlot != null)
        {
            magicValue += weaponSlot.GetMagic();
        }
        if (outfitSlot != null)
        {
            magicValue += outfitSlot.GetMagic();
        }
        if (accessorySlot != null)
        {
            magicValue += accessorySlot.GetMagic();
        }
        return magicValue;
    }
    public int GetCraft()
    {
        int craftValue = craft.GetValue(); 
        if (weaponSlot != null)
        {
            craftValue += weaponSlot.GetCraft();
        }
        if (outfitSlot != null)
        {
            craftValue += outfitSlot.GetCraft();
        }
        if (accessorySlot != null)
        {
            craftValue += accessorySlot.GetCraft();
        }
        return craftValue;
    }
    public IfCrafter GetCraftType()
    {
        return profession.ifCrafter;
    }
    public Item GetWeaponSlot()
    {
        return weaponSlot;
    }
    public Item GetOutfitSlot()
    {
        return outfitSlot;
    }
    public Item GetAccessorySlot()
    {
        return accessorySlot;
    }
    public int GetCharacterValue()
    {
        return charValue;
    }
    public bool GetIfMale()
    {
        return genderM;
    }
    
    // Alias methods for SaveSystem compatibility
    public Item GetWeapon()
    {
        return GetWeaponSlot();
    }
    
    public Item GetOutfit()
    {
        return GetOutfitSlot();
    }
    
    public Item GetAccessory()
    {
        return GetAccessorySlot();
    }
    #endregion
    #region Equipment
    public void EquipWeapon(Item item)
    {
        if (item.GetItemType() == ItemType.Weapon)
        {
            if (weaponSlot != null)
            {
                UnEquipWeapon();
            }
            Guild.Instance.RemoveItemFromInventory(item);
            weaponSlot = item;
            item.SetEquipped(true, adventurerName);
        }
    }
    public void EquipOutfit(Item item)
    {
        if (item.GetItemType() == ItemType.Outfit)
        {
            if (outfitSlot != null)
            {
                UnEquipOutfit();
            }
            Guild.Instance.RemoveItemFromInventory(item);
            outfitSlot = item;
            item.SetEquipped(true, adventurerName);
        }
    }
    public void EquipAccessory(Item item)
    {
        if (item.GetItemType() == ItemType.Accessory)
        {
            if (accessorySlot != null)
            {
                UnEquipAccessory();
            }
            Guild.Instance.RemoveItemFromInventory(item);
            accessorySlot = item;
            item.SetEquipped(true, adventurerName);
        }
    }
    public void UnEquipWeapon()
    {
        if (weaponSlot != null)
        {
            weaponSlot.SetEquipped(false);
            Guild.Instance.AddItemToInventory(weaponSlot);
            weaponSlot = null;
        }
    }
    public void UnEquipOutfit()
    {
        if (outfitSlot != null)
        {
            outfitSlot.SetEquipped(false);
            Guild.Instance.AddItemToInventory(outfitSlot);
            outfitSlot = null;
        }
    }
    public void UnEquipAccessory()
    {
        if (accessorySlot != null)
        {
            accessorySlot.SetEquipped(false);
            Guild.Instance.AddItemToInventory(accessorySlot);
            accessorySlot = null;
        }
    }
    #endregion
}
public enum IfCrafter { NA, Weaponsmith, Armoursmith, Leatherworker, Tailor, Fletcher, Jeweller, Carpenter }