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

    public List<Profession> professions;
    public List<Species> species;

    [SerializeField] private GameObject adventurerPrefab;

    public void GenerateCharacter()
    {
        GameObject newPrefab = Instantiate(adventurerPrefab);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        newAdventurer.GenerateStartingCharacter(GetRandomSpecies(), GetRandomProfession());
    }

    private Profession GetRandomProfession()
    {
        int rand = Random.Range(0, professions.Count);
        Profession profToReturn = professions[rand];
        return profToReturn;
    }
    private Species GetRandomSpecies()
    {
        int rand = Random.Range(0, species.Count);
        Species specToReturn = species[rand];
        return specToReturn;
    }
}
