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

    public Adventurer GenerateRandomCharacter(Transform parent)
    {
        GameObject newPrefab = Instantiate(adventurerPrefab, parent);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        newAdventurer.GenerateStartingCharacter(GetRandomSpecies(), GetRandomProfession());
        return newAdventurer;
    }
    public Adventurer GenerateStarterCharacter(Transform parent)
    {
        GameObject newPrefab = Instantiate(adventurerPrefab, parent);
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

    #region Save System Integration
    
    public ProfessionSO GetProfessionByName(string professionName)
    {
        if (string.IsNullOrEmpty(professionName)) return null;
        
        foreach (ProfessionSO profession in professions)
        {
            if (profession != null && profession.name == professionName)
                return profession;
        }
        
        foreach (ProfessionSO profession in starterProfessions)
        {
            if (profession != null && profession.name == professionName)
                return profession;
        }
        
        Debug.LogWarning($"Profession '{professionName}' not found, using default");
        return professions.Count > 0 ? professions[0] : null;
    }
    
    public SpeciesSO GetSpeciesByName(string speciesName)
    {
        if (string.IsNullOrEmpty(speciesName)) return null;
        
        foreach (SpeciesSO spec in species)
        {
            if (spec != null && spec.name == speciesName)
                return spec;
        }
        
        Debug.LogWarning($"Species '{speciesName}' not found, using default");
        return species.Count > 0 ? species[0] : null;
    }
    
    public Adventurer RecreateAdventurer(AdventurerSaveData saveData, Transform parent)
    {
        if (saveData == null) return null;
        
        ProfessionSO profession = GetProfessionByName(saveData.professionName);
        SpeciesSO speciesData = GetSpeciesByName(saveData.speciesName);
        
        if (profession == null || speciesData == null)
        {
            Debug.LogError($"Cannot recreate adventurer '{saveData.name}' - missing profession or species data");
            return null;
        }
        
        GameObject newPrefab = Instantiate(adventurerPrefab, parent);
        Adventurer newAdventurer = newPrefab.GetComponent<Adventurer>();
        
        newAdventurer.GenerateStartingCharacter(speciesData, profession);
        
        newAdventurer.SetAdventurerName(saveData.name);
        newAdventurer.SetLevel(saveData.level);
        newAdventurer.SetExperience(saveData.experience);
        
        if (saveData.stats != null)
        {
            newAdventurer.SetCombat(saveData.stats.combat);
            newAdventurer.SetHealing(saveData.stats.healing);
            newAdventurer.SetSocial(saveData.stats.social);
            newAdventurer.SetSubterfuge(saveData.stats.subterfuge);
            newAdventurer.SetHunting(saveData.stats.hunting);
            newAdventurer.SetMagic(saveData.stats.magic);
            newAdventurer.SetCraft(saveData.stats.craft);
        }
        
        return newAdventurer;
    }
    
    #endregion
}
