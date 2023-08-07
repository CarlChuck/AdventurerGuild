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
    public List<SpeciesSO> species;

    [SerializeField] private GameObject adventurerPrefab;

    public void GenerateCharacter()
    {
        GameObject newPrefab = Instantiate(adventurerPrefab);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        newAdventurer.GenerateStartingCharacter(GetRandomSpecies(), GetRandomProfession());
    }

    private ProfessionSO GetRandomProfession()
    {
        int rand = Random.Range(0, professions.Count);
        ProfessionSO profToReturn = professions[rand];
        return profToReturn;
    }
    private SpeciesSO GetRandomSpecies()
    {
        int rand = Random.Range(0, species.Count);
        SpeciesSO specToReturn = species[rand];
        return specToReturn;
    }
}
