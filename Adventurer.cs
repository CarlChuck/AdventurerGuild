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
    [SerializeField] private Stat combat;
    [SerializeField] private Stat healing;
    [SerializeField] private Stat social;
    [SerializeField] private Stat subterfuge;
    [SerializeField] private Stat hunting;
    [SerializeField] private Stat magic;
    [SerializeField] private Stat craft;

    [SerializeField] private Item weaponSlot;
    [SerializeField] private Item outfitSlot;
    [SerializeField] private Item accessorySlot;

    #region Core Generation
    public void GenerateStartingCharacter(SpeciesSO spec, ProfessionSO prof)
    {
        species = spec;
        profession = prof;
        GenerateStats();
        experience = 0;
        AddLevel();
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
    #endregion
}
public enum IfCrafter { NA, Weaponsmith, Armoursmith, Leatherworker, Tailor, Fletcher, Jeweller, Carpenter }