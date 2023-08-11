using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGen : MonoBehaviour
{
    #region Singleton
    public static CharGen Instance;
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

    public List<ProfessionSO> professions;
    public List<ProfessionSO> starterProfessions;
    public List<SpeciesSO> species;

    [SerializeField] private GameObject adventurerPrefab;

    public Adventurer GenerateRandomCharacter()
    {
        GameObject newPrefab = Instantiate(adventurerPrefab);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        newAdventurer.GenerateStartingCharacter(GetRandomSpecies(), GetRandomProfession());
        return newAdventurer;
    }
    public Adventurer GenerateStarterCharacter()
    {
        GameObject newPrefab = Instantiate(adventurerPrefab);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        newAdventurer.GenerateStartingCharacter(GetRandomSpecies(), GetRandomStarterProfession());
        return newAdventurer;
    }

    private ProfessionSO GetRandomProfession()
    {
        int rand = Random.Range(0, professions.Count);
        ProfessionSO profToReturn = professions[rand];
        return profToReturn;
    }
    private ProfessionSO GetRandomStarterProfession()
    {
        int rand = Random.Range(0, starterProfessions.Count);
        ProfessionSO profToReturn = starterProfessions[rand];
        return profToReturn;
    }
    private SpeciesSO GetRandomSpecies()
    {
        int rand = Random.Range(0, species.Count);
        SpeciesSO specToReturn = species[rand];
        return specToReturn;
    }
}
