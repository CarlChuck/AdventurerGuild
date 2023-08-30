using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "ScriptableObject/Mission")]
public class MissionSO : ScriptableObject
{
    public IfCrafter ifCrafter;
    public int combat;
    public int healing;
    public int social;
    public int subterfuge;
    public int hunting;
    public int magic;
    public int craft;
}
public enum MissionType 
{ 
    Dungeon, Patrol, Defence, 
    Healing, Cure, HerbGather, 
    Diplomacy, Entertain, Networking, 
    Stealth, Smuggling, Larceny, 
    MonsterHunt, BountyHunt, Scouting,
    Enchantment, SpiritQuest, Divination,
    MiningExpedition, OreRefinement, WoodcuttingExpedition
}