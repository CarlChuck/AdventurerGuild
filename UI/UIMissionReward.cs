using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionReward : MonoBehaviour
{
    [SerializeField] private Mission missionReference;

    public void ShowRewards(Mission mission)
    {
        missionReference = mission;
        DisplayMissionOutcomes();
    }

    private void DisplayMissionOutcomes()
    {
        //TODO: Implement UI display for mission outcomes
    }

    public void OnButtonClaimReward()
    {
        UIManager.Instance.OnButtonClaimReward(missionReference);
    }
}
