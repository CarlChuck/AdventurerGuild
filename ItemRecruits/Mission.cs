using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private string missionName;
    private MissionSO missionBase;
    private MissionType missionType;
    private int missionLevel;
    private Quality missionRarity;
    private int difficultyValue;
    private int combat;
    private int healing;
    private int social;
    private int subterfuge;
    private int hunting;
    private int magic;
    private int craft;
    private IfCrafter craftType;
    private int goldValue;
    private List<Item> itemRewards;
    private int maxAdventurers;
    private List<Adventurer> adventurersOnMission;
    private Adventurer adventurerSlot1;
    private Adventurer adventurerSlot2;
    private Adventurer adventurerSlot3;
    private Adventurer adventurerSlot4;
    private int missionTime;
    private MissionState missionState;

    #region Setters
    public void SetMission(MissionSO theMission)
    {
        missionBase = theMission;
        missionType = theMission.mType;
        SetMissionName();
        SetMissionLevel();
        SetMissionQuality();
        SetMaxAdventurers();
        SetMissionStats();
        SetMissionRewards();
        CalculateDifficultyValue();
        SetMissionTime();
        craftType = theMission.ifCrafter;
        missionState = MissionState.Available;
        adventurersOnMission.Clear();
        adventurerSlot1 = null;
        adventurerSlot2 = null;
        adventurerSlot3 = null;
        adventurerSlot4 = null;
    }
    private void SetMissionQuality()
    {
        int rand = Random.Range(1, 1001);
        if (rand <= 700)
        {
            missionRarity = Quality.Common;
        }
        else if (rand <= 900)
        {
            missionRarity = Quality.Uncommon;
        }
        else if (rand <= 975)
        {
            missionRarity = Quality.Masterwork;
        }
        else if (rand <= 995)
        {
            missionRarity = Quality.Rare;
        }
        else
        {
            missionRarity = Quality.Legendary;
        }
    }
    private void SetMissionLevel()
    {
        int topAdvLevel = Guild.Instance.GetHighestLevelAdventurer();
        if (topAdvLevel > 1)
        {
            missionLevel = Random.Range(1, topAdvLevel + 1);
        }
        else
        {
            missionLevel = 1;
        }
    }
    private void CalculateDifficultyValue()
    {
        difficultyValue = combat + healing + social + subterfuge + hunting + magic + craft;
    }
    private void SetMissionName()
    {
        missionName = "theMission"; //TODO
    }
    private void SetMissionStats()
    {
        float multiplier = 0;
        switch (missionRarity)
        {
            case Quality.Common:
                multiplier = 1;
                break;
            case Quality.Uncommon:
                multiplier = 1.2f;
                break;
            case Quality.Masterwork:
                multiplier = 1.5f;
                break;
            case Quality.Rare:
                multiplier = 2f;
                break;
            case Quality.Legendary:
                multiplier = 3f;
                break;
        }
        combat = (int)(GetStatBasedOnStartingValue(missionBase.combat) * missionLevel * maxAdventurers * multiplier);
        healing = (int)(GetStatBasedOnStartingValue(missionBase.healing) * missionLevel * maxAdventurers * multiplier);
        social = (int)(GetStatBasedOnStartingValue(missionBase.social) * missionLevel * maxAdventurers * multiplier);
        subterfuge = (int)(GetStatBasedOnStartingValue(missionBase.subterfuge) * missionLevel * maxAdventurers * multiplier);
        hunting = (int)(GetStatBasedOnStartingValue(missionBase.hunting) * missionLevel * maxAdventurers * multiplier);
        magic = (int)(GetStatBasedOnStartingValue(missionBase.magic) * missionLevel * maxAdventurers * multiplier);
        craft = (int)(GetStatBasedOnStartingValue(missionBase.craft) * missionLevel * maxAdventurers * multiplier);
    }
    private void SetMissionRewards()
    {
        goldValue = 0; //TODO
        itemRewards = null; //TODO
    }
    private void SetMaxAdventurers()
    {
        int rand = Random.Range(1, 11);
        if (missionType == MissionType.Dungeon || missionType == MissionType.Patrol || missionType == MissionType.Defence || missionType == MissionType.MonsterHunt 
            || missionType == MissionType.MiningExpedition || missionType == MissionType.WoodcuttingExpedition)
        {
            if (rand < 3)
            {
                maxAdventurers = 2;
            }
            else if (rand < 7)
            {
                maxAdventurers = 3;
            }
            else
            {
                maxAdventurers = 4;
            }
        }
        else if (missionType == MissionType.CraftingOrder)
        {
            maxAdventurers = 1;
        }
        else
        {
            if (rand < 8)
            {
                maxAdventurers = 1;
            }
            else
            {
                maxAdventurers = 2;
            }
        }
    }
    private void SetMissionTime()
    {
        missionTime = missionBase.missionTime;
    }
    #endregion
    #region Getters
    public int GetStatBasedOnStartingValue(int valueToPass)
    {
        int valueToExit = 1;
        switch (valueToPass)
        {
            case 1:
                valueToExit = Random.Range(1, 4);
                break;
            case 2:
                valueToExit = Random.Range(2, 6);
                break;
            case 3:
                valueToExit = Random.Range(3, 8);
                break;
            case 4:
                valueToExit = Random.Range(4, 10);
                break;
            case 5:
                valueToExit = Random.Range(5, 12);
                break;
        }
        return valueToExit;
    }
    public string GetMissionName()
    {
        return missionName;
    }
    public int GetMissionDifficultyValue()
    {
        return difficultyValue;
    }
    public Quality GetMissionQuality()
    {
        return missionRarity;
    }
    public MissionType GetMissionType()
    {
        return missionType;
    }
    public int GetMissionLevel()
    {
        return missionLevel;
    }
    public void GetStats(out int OutCombat, out int OutHealing, out int OutSocial, out int OutSubterfuge, out int OutHunting, out int OutMagic, out int OutCraft, out IfCrafter OutCraftType)
    {
        OutCombat = combat;
        OutHealing = healing;
        OutSocial = social;
        OutSubterfuge = subterfuge;
        OutHunting = hunting;
        OutMagic = magic;
        OutCraft = craft;
        OutCraftType = craftType;
    }
    public int GetGoldValue()
    {
        return goldValue;
    }
    public List<Item> GetRewards()
    {
        return itemRewards;
    }
    public List<Adventurer> GetAdventurersOnMission()
    {
        return adventurersOnMission;
    }
    public int GetMaxAdventurers()
    {
        return maxAdventurers;
    }
    public int GetMissionTime()
    {
        return missionTime;
    }
    #endregion
    #region MissionProgression
    public void AssignAdventurer(Adventurer adventurerToAssign)
    {
        if (missionState == MissionState.Available)
        {

        }
    }
    public void RemoveAdventurer(Adventurer adventurerToRemove)
    {
        if (missionState == MissionState.Available)
        {

        }
    }
    public void BeginMission()
    {
        missionState = MissionState.InProgress;
    }
    public void OnMissionEnd()
    {
        missionState = MissionState.Completed;
    }
    #endregion
}
public enum MissionType
{
    Dungeon, Patrol, Defence,
    Healing, Cure, HerbGather,
    Diplomacy, Entertain, Networking,
    Stealth, Smuggling, Larceny,
    MonsterHunt, BountyHunt, Scouting,
    Enchantment, SpiritQuest, Divination,
    MiningExpedition, OreRefinement, WoodcuttingExpedition, CraftingOrder
}
public enum MissionState { Available, InProgress, Completed}