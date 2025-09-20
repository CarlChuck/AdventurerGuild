using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMissionCompleted : MonoBehaviour
{
    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionLevel;
    [SerializeField] private TextMeshProUGUI missionResult;
    [SerializeField] private TextMeshProUGUI participantAdventurers;
    [SerializeField] private TextMeshProUGUI rewardsReceived;
    
    private Mission missionReference;
    
    public void SetMission(Mission mission)
    {
        missionReference = mission;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (!missionReference)
        {
            return;
        }

        if (missionName)
        {
            missionName.text = missionReference.GetMissionName();
        }

        if (missionLevel)
        {
            missionLevel.text = "Level " + missionReference.GetMissionLevel().ToString();
        }

        // Calculate results for display
        MissionResult result = missionReference.CalculateMissionSuccess();
        MissionRewards rewards = missionReference.CalculateRewards(result);
        
        if (missionResult)
        {
            missionResult.text = $"Result: {result.grade}\nSuccess Rate: {result.successRate:F1}%";
        }
        
        if (participantAdventurers)
        {
            List<Adventurer> adventurers = missionReference.GetAdventurersOnMission();
            string adventurerNames = "";
            foreach (Adventurer adv in adventurers)
            {
                if (!adv)
                {
                    continue;
                }

                if (adventurerNames.Length > 0)
                {
                    adventurerNames += ", ";
                }

                adventurerNames += adv.GetName();
            }
            participantAdventurers.text = "Participants: " + adventurerNames;
        }

        if (!rewardsReceived)
        {
            return;
        }

        string rewardText = $"Gold: {rewards.gold}";
        if (rewards.experiencePerAdventurer > 0)
        {
            rewardText += $"\nXP per Adventurer: {rewards.experiencePerAdventurer}";
        }
        if (rewards.items is { Count: > 0 })
        {
            rewardText += $"\nItems: {rewards.items.Count}";
        }
        rewardsReceived.text = rewardText;
    }

    public void OnCompleteButton()
    {
        UIManager.Instance.OnButtonClaimReward(missionReference);
    }

    public void OnAcceptRewardButton()
    {
        if (missionReference != null)
        {
            missionReference.CompleteMission();
        }
    }
}