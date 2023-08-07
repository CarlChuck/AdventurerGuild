using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    [SerializeField] private string adventurerName;
    [SerializeField] private Species species;
    [SerializeField] private Profession profession;
    [SerializeField] private int level;
    [SerializeField] private Stat combat;
    [SerializeField] private Stat healing;
    [SerializeField] private Stat social;
    [SerializeField] private Stat subterfuge;
    [SerializeField] private Stat hunting;
    [SerializeField] private Stat magic;
    [SerializeField] private Stat craft;

    public void GenerateStartingCharacter(Species spec, Profession prof)
    {
        species = spec;
        profession = prof;
        GenerateStats();
        AddLevel();
    }
    public void AddLevel()
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
    #region Private Generation Methods
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
            0 => Random.Range(0, 2),
            1 => Random.Range(1, 4),
            2 => Random.Range(2, 6),
            3 => Random.Range(3, 8),
            4 => Random.Range(4, 10),
            5 => Random.Range(5, 12),
            _ => Random.Range(0, 2),
        };
        stat.AddValue(statAmountToAdd);
    }
    private void AddSpeciesStat(Stat stat, int amount)
    {
        var statAmountToAdd = amount switch
        {
            0 => 0,
            1 => Random.Range(1, 4),
            _ => 0,
        };
        stat.AddValue(statAmountToAdd);
    }
    #endregion
}
