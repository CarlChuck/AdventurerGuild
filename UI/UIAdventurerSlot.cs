using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAdventurerSlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown adventurerDropdown;
    [SerializeField] private Button removeButton;
    [SerializeField] private TextMeshProUGUI slotLabel;
    [SerializeField] private TextMeshProUGUI adventurerStats;
    [SerializeField] private GameObject assignedAdventurerPanel;
    [SerializeField] private GameObject emptySlotPanel;
    
    private UIFullMission parentMissionUI;
    private int slotIndex;
    private Adventurer assignedAdventurer;
    private List<Adventurer> availableAdventurers = new List<Adventurer>();
    
    public void Initialize(UIFullMission parent, int index)
    {
        parentMissionUI = parent;
        slotIndex = index;
        
        // Setup UI
        if (slotLabel != null)
            slotLabel.text = $"Slot {index + 1}";
            
        // Setup button listeners
        if (removeButton != null)
            removeButton.onClick.AddListener(OnRemoveAdventurer);
        if (adventurerDropdown != null)
            adventurerDropdown.onValueChanged.AddListener(OnDropdownChanged);
            
        RefreshSlot();
    }
    
    public void RefreshSlot()
    {
        UpdateAvailableAdventurers();
        UpdateDropdown();
        UpdateDisplay();
    }
    
    private void UpdateAvailableAdventurers()
    {
        if (parentMissionUI != null)
        {
            availableAdventurers = parentMissionUI.GetAvailableAdventurers();
        }
    }
    
    private void UpdateDropdown()
    {
        if (adventurerDropdown == null) return;
        
        // Clear existing options
        adventurerDropdown.ClearOptions();
        
        // Add "None" option
        List<string> options = new List<string> { "None" };
        
        // Add available adventurers
        foreach (Adventurer adventurer in availableAdventurers)
        {
            if (adventurer != null)
            {
                string optionText = $"{adventurer.GetName()} (Lv.{adventurer.GetLevel()})";
                options.Add(optionText);
            }
        }
        
        // Add currently assigned adventurer if they exist
        if (assignedAdventurer != null)
        {
            string assignedText = $"{assignedAdventurer.GetName()} (Lv.{assignedAdventurer.GetLevel()}) [ASSIGNED]";
            options.Insert(1, assignedText); // Insert after "None"
        }
        
        adventurerDropdown.AddOptions(options);
        
        // Set current selection
        if (assignedAdventurer != null)
        {
            adventurerDropdown.value = 1; // Assigned adventurer option
        }
        else
        {
            adventurerDropdown.value = 0; // "None" option
        }
    }
    
    private void UpdateDisplay()
    {
        bool hasAssignment = assignedAdventurer != null;
        
        // Show/hide panels
        if (assignedAdventurerPanel != null)
            assignedAdventurerPanel.SetActive(hasAssignment);
        if (emptySlotPanel != null)
            emptySlotPanel.SetActive(!hasAssignment);
            
        // Update stats display
        if (hasAssignment && adventurerStats != null)
        {
            adventurerStats.text = $"{assignedAdventurer.GetName()}\n" +
                                  $"Level {assignedAdventurer.GetLevel()}\n" +
                                  $"Combat: {assignedAdventurer.GetCombat()}\n" +
                                  $"Healing: {assignedAdventurer.GetHealing()}\n" +
                                  $"Social: {assignedAdventurer.GetSocial()}\n" +
                                  $"Subterfuge: {assignedAdventurer.GetSubterfuge()}\n" +
                                  $"Hunting: {assignedAdventurer.GetHunting()}\n" +
                                  $"Magic: {assignedAdventurer.GetMagic()}\n" +
                                  $"Craft: {assignedAdventurer.GetCraft()}";
        }
        
        // Update remove button
        if (removeButton != null)
            removeButton.gameObject.SetActive(hasAssignment);
    }
    
    private void OnDropdownChanged(int selectedIndex)
    {
        if (selectedIndex == 0) // "None" selected
        {
            if (assignedAdventurer != null)
            {
                OnRemoveAdventurer();
            }
        }
        else if (selectedIndex == 1 && assignedAdventurer != null)
        {
            // Already assigned adventurer selected, do nothing
            return;
        }
        else
        {
            // Calculate the actual adventurer index
            int adventurerIndex = assignedAdventurer != null ? selectedIndex - 2 : selectedIndex - 1;
            
            if (adventurerIndex >= 0 && adventurerIndex < availableAdventurers.Count)
            {
                Adventurer selectedAdventurer = availableAdventurers[adventurerIndex];
                AssignAdventurer(selectedAdventurer);
            }
        }
    }
    
    public void AssignAdventurer(Adventurer adventurer)
    {
        if (adventurer == null) return;
        
        // Remove current assignment if any
        if (assignedAdventurer != null)
        {
            OnRemoveAdventurer();
        }
        
        // Assign new adventurer
        assignedAdventurer = adventurer;
        
        // Notify parent
        if (parentMissionUI != null)
        {
            parentMissionUI.OnAdventurerAssigned(adventurer, slotIndex);
        }
        
        // Refresh all slots to update available lists
        RefreshAllSlots();
    }
    
    public void OnRemoveAdventurer()
    {
        if (assignedAdventurer == null) return;
        
        Adventurer removedAdventurer = assignedAdventurer;
        assignedAdventurer = null;
        
        // Notify parent
        if (parentMissionUI != null)
        {
            parentMissionUI.OnAdventurerRemoved(removedAdventurer, slotIndex);
        }
        
        // Refresh all slots to update available lists
        RefreshAllSlots();
    }
    
    private void RefreshAllSlots()
    {
        // Find all sibling slots and refresh them
        if (transform.parent != null)
        {
            UIAdventurerSlot[] allSlots = transform.parent.GetComponentsInChildren<UIAdventurerSlot>();
            foreach (UIAdventurerSlot slot in allSlots)
            {
                if (slot != null)
                {
                    slot.RefreshSlot();
                }
            }
        }
    }
    
    public Adventurer GetAssignedAdventurer()
    {
        return assignedAdventurer;
    }
    
    public bool IsEmpty()
    {
        return assignedAdventurer == null;
    }
}