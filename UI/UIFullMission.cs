using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFullMission : MonoBehaviour
{
    [Header("Mission Details")]
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionLevel;
    [SerializeField] private TextMeshProUGUI missionQuality;
    [SerializeField] private TextMeshProUGUI missionTime;
    [SerializeField] private TextMeshProUGUI goldReward;
    
    [Header("Mission Requirements")]
    [SerializeField] private TextMeshProUGUI combatReq;
    [SerializeField] private TextMeshProUGUI healingReq;
    [SerializeField] private TextMeshProUGUI socialReq;
    [SerializeField] private TextMeshProUGUI subterfugeReq;
    [SerializeField] private TextMeshProUGUI huntingReq;
    [SerializeField] private TextMeshProUGUI magicReq;
    [SerializeField] private TextMeshProUGUI craftReq;
    [SerializeField] private TextMeshProUGUI craftType;
    
    [Header("Team Composition")]
    [SerializeField] private Transform adventurerSlotsContainer;
    [SerializeField] private GameObject adventurerSlotPrefab;
    [SerializeField] private TextMeshProUGUI teamStatsDisplay;
    [SerializeField] private TextMeshProUGUI successProbability;
    
    [Header("Controls")]
    [SerializeField] private Button startMissionButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject missionPanel;
    
    private Mission currentMission;
    private List<UIAdventurerSlot> adventurerSlots = new List<UIAdventurerSlot>();
    
    private void Start()
    {
        // Setup button listeners
        if (startMissionButton != null)
        {
            startMissionButton.onClick.AddListener(OnStartMission);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnCancel);
        }

        // Start closed
        
        if (missionPanel != null)
        {
            missionPanel.SetActive(false);
        }
    }
    
    public void DisplayMission(Mission mission)
    {
        currentMission = mission;

        // Update mission details
        UpdateMissionInfo();
        UpdateMissionRequirements();
        SetupAdventurerSlots();
        UpdateTeamDisplay();        
        if (missionPanel != null)
        {
            missionPanel.SetActive(true);
        }
    }
    
    private void UpdateMissionInfo()
    {
        if (currentMission == null)
        {
            return;
        }

        if (missionName != null)
        {
            missionName.text = currentMission.GetMissionName();
        }

        if (missionLevel != null)
        {
            missionLevel.text = "Level " + currentMission.GetMissionLevel().ToString();
        }

        if (missionQuality != null)
        {
            missionQuality.text = currentMission.GetMissionQuality().ToString();
        }

        if (missionTime != null)
        {
            missionTime.text = FormatTime(currentMission.GetMissionTime());
        }

        if (goldReward != null)
        {
            goldReward.text = currentMission.GetGoldValue().ToString() + " Gold";
        }
    }
    
    private void UpdateMissionRequirements()
    {
        if (currentMission == null)
        {
            return;
        }

        currentMission.GetStats(out int combat, out int healing, out int social, out int subterfuge, out int hunting,
            out int magic, out int craft, out IfCrafter outCraftType);
        
        if (combatReq != null)
        {
            combatReq.text = combat.ToString();
        }
        if (healingReq != null)
        {
            healingReq.text = healing.ToString();
        }
        if (socialReq != null)
        {
            socialReq.text = social.ToString();
        }
        if (subterfugeReq != null)
        {
            subterfugeReq.text = subterfuge.ToString();
        }
        if (huntingReq != null)
        {
            huntingReq.text = hunting.ToString();
        }
        if (magicReq != null)
        {
            magicReq.text = magic.ToString();
        }
        if (craftReq != null)
        {
            craftReq.text = craft.ToString();
        }
        if (craftType != null)
        {
            craftType.text = outCraftType.ToString();
        }
    }
    
    private void SetupAdventurerSlots()
    {
        if (currentMission == null || adventurerSlotsContainer == null || adventurerSlotPrefab == null)
        {
            return;
        }

        // Clear existing slots
        ClearAdventurerSlots();
        
        // Create slots based on mission's max adventurers
        int maxSlots = currentMission.GetMaxAdventurers();
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(adventurerSlotPrefab, adventurerSlotsContainer);
            UIAdventurerSlot slot = slotObj.GetComponent<UIAdventurerSlot>();
            if (slot != null)
            {
                slot.Initialize(this, i);
                adventurerSlots.Add(slot);
            }
        }
    }
    
    private void ClearAdventurerSlots()
    {
        foreach (UIAdventurerSlot slot in adventurerSlots)
        {
            if (slot != null && slot.gameObject != null)
            {
                Destroy(slot.gameObject);
            }
        }
        adventurerSlots.Clear();
    }
    
    public void OnAdventurerAssigned(Adventurer adventurer, int slotIndex)
    {
        if (currentMission != null && adventurer != null)
        {
            currentMission.AssignAdventurer(adventurer);
            UpdateTeamDisplay();
        }
    }
    
    public void OnAdventurerRemoved(Adventurer adventurer, int slotIndex)
    {
        if (currentMission != null && adventurer != null)
        {
            currentMission.RemoveAdventurer(adventurer);
            UpdateTeamDisplay();
        }
    }
    
    private void UpdateTeamDisplay()
    {
        if (currentMission == null)
        {
            return;
        }
        
        // Calculate current team stats
        currentMission.CalculateTeamStats(out int teamCombat, out int teamHealing, out int teamSocial,
                                         out int teamSubterfuge, out int teamHunting, out int teamMagic, out int teamCraft);
        
        // Update team stats display
        if (teamStatsDisplay != null)
        {
            teamStatsDisplay.text = $"Team Stats:\nCombat: {teamCombat}\nHealing: {teamHealing}\nSocial: {teamSocial}\n" +
                                   $"Subterfuge: {teamSubterfuge}\nHunting: {teamHunting}\nMagic: {teamMagic}\nCraft: {teamCraft}";
        }
        
        // Calculate and display success probability
        if (successProbability != null)
        {
            if (currentMission.GetAssignedAdventurerCount() > 0)
            {
                MissionResult result = currentMission.CalculateMissionSuccess();
                successProbability.text = $"Success Rate: {result.successRate:F1}%\nGrade: {result.grade}";
            }
            else
            {
                successProbability.text = "Assign adventurers to see success rate";
            }
        }
        
        // Update start button state
        if (startMissionButton != null)
        {
            startMissionButton.interactable = currentMission.GetAssignedAdventurerCount() > 0 && 
                                            currentMission.GetMissionState() == MissionState.Available;
        }
    }
    
    public List<Adventurer> GetAvailableAdventurers()
    {
        List<Adventurer> availableAdventurers = new List<Adventurer>();
        
        // Get adventurers who are not on any active missions
        List<Adventurer> freeAdventurers = Guild.Instance.GetAvailableAdventurers();
        
        foreach (Adventurer adventurer in freeAdventurers)
        {
            // Check if adventurer is not already assigned to this mission
            bool alreadyAssigned = false;
            
            if (currentMission != null)
            {
                List<Adventurer> assignedAdventurers = currentMission.GetAdventurersOnMission();
                
                foreach (Adventurer assigned in assignedAdventurers)
                {
                    if (assigned == adventurer)
                    {
                        alreadyAssigned = true;
                        break;
                    }
                }
            }
            
            if (!alreadyAssigned)
            {
                availableAdventurers.Add(adventurer);
            }
        }
        
        return availableAdventurers;
    }
    
    private void OnStartMission()
    {
        if (currentMission != null && currentMission.GetAssignedAdventurerCount() > 0)
        {
            MissionManager.Instance.StartMission(currentMission);
            OnCancel(); // Close the popup
            
            // UI refresh is now handled by MissionManager.StartMission()
        }
    }
    
    private void OnCancel()
    {
        
        if (missionPanel != null)
        {
            missionPanel.SetActive(false);
        }

        currentMission = null;
        ClearAdventurerSlots();
    }
    
    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{minutes:00}:{secs:00}";
    }
    
    private void Update()
    {
        // Update mission time display if mission is in progress
        if (currentMission != null && currentMission.GetMissionState() == MissionState.InProgress)
        {
            float remainingTime = currentMission.GetRemainingTime();
            if (missionTime != null)
            {
                missionTime.text = "Time Left: " + FormatTime(remainingTime);
            }
        }
    }
}