using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMissionInProgress : MonoBehaviour
{
    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionLevel;
    [SerializeField] private TextMeshProUGUI timeRemaining;
    [SerializeField] private TextMeshProUGUI assignedAdventurers;
    [SerializeField] private TextMeshProUGUI successProbability;
    
    private Mission missionReference;
    
    public void SetMission(Mission mission)
    {
        missionReference = mission;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (missionReference == null) return;
        
        if (missionName != null)
            missionName.text = missionReference.GetMissionName();
        if (missionLevel != null)
            missionLevel.text = "Level " + missionReference.GetMissionLevel().ToString();
        if (timeRemaining != null)
            timeRemaining.text = FormatTime(missionReference.GetRemainingTime());
        if (assignedAdventurers != null)
        {
            List<Adventurer> adventurers = missionReference.GetAdventurersOnMission();
            string adventurerNames = "";
            foreach (Adventurer adv in adventurers)
            {
                if (adv != null)
                {
                    if (adventurerNames.Length > 0) adventurerNames += ", ";
                    adventurerNames += adv.GetName();
                }
            }
            assignedAdventurers.text = adventurerNames;
        }
        if (successProbability != null)
        {
            MissionResult result = missionReference.CalculateMissionSuccess();
            successProbability.text = $"{result.successRate:F1}%";
        }
    }
    
    private void Update()
    {
        // Update time remaining in real-time
        if (missionReference != null && missionReference.GetMissionState() == MissionState.InProgress)
        {
            if (timeRemaining != null)
            {
                float remaining = missionReference.GetRemainingTime();
                timeRemaining.text = FormatTime(remaining);
            }
        }
    }
    
    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{minutes:00}:{secs:00}";
    }
}