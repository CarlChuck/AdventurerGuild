using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRosterEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charProfession;
    [SerializeField] private TextMeshProUGUI combat;
    [SerializeField] private TextMeshProUGUI healing;
    [SerializeField] private TextMeshProUGUI social;
    [SerializeField] private TextMeshProUGUI subterfuge;
    [SerializeField] private TextMeshProUGUI hunting;
    [SerializeField] private TextMeshProUGUI magic;
    [SerializeField] private TextMeshProUGUI craft;
    [SerializeField] private TextMeshProUGUI charValue;
    [SerializeField] private TextMeshProUGUI availabilityStatus;
    [SerializeField] private Adventurer adventurerReference;

    public void SetStats(Adventurer adventurer)
    {
        adventurerReference = adventurer;
        charName.text = adventurer.GetName();
        charProfession.text = adventurer.GetProfession();
        combat.text = adventurer.GetCombat().ToString();
        healing.text = adventurer.GetHealing().ToString();
        social.text = adventurer.GetSocial().ToString();
        subterfuge.text = adventurer.GetSubterfuge().ToString();
        hunting.text = adventurer.GetHunting().ToString();
        magic.text = adventurer.GetMagic().ToString();
        craft.text = adventurer.GetCraft().ToString();
        charValue.text = adventurer.GetCharacterValue().ToString();
        
        // Update availability status
        UpdateAvailabilityStatus();
    }
    
    private void UpdateAvailabilityStatus()
    {
        if (availabilityStatus == null || adventurerReference == null)
            return;
            
        List<Adventurer> busyAdventurers = Guild.Instance.GetAdventurersOnActiveMissions();
        
        if (busyAdventurers.Contains(adventurerReference))
        {
            availabilityStatus.text = "On Mission";
            availabilityStatus.color = Color.red;
        }
        else
        {
            availabilityStatus.text = "Available";
            availabilityStatus.color = Color.green;
        }
    }
    
    public void RefreshAvailabilityStatus()
    {
        UpdateAvailabilityStatus();
    }
    public void OnButtonPress()
    {
        UIManager.Instance.OnButtonAdventurer(adventurerReference);
    }
}
