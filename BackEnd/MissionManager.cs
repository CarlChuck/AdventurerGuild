using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    #region Singleton
    public static MissionManager Instance;
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
        taskList = new();
        actionList = new();
        activeTaskList = new();
        completedMissionList = new();
    }
    #endregion
    [SerializeField] private Transform TaskList;
    [SerializeField] private Transform ActionList;
    [SerializeField] private Transform InProgressList;
    [SerializeField] private Transform CompleteList;
    [SerializeField] private List<Mission> taskList;
    [SerializeField] private List<Mission> actionList;
    [SerializeField] private List<Mission> activeTaskList;
    [SerializeField] private List<Mission> completedMissionList;
    private bool missionSystemActive = false;

    public void InitaliseMissions()
    {
        PopulateTaskList();
        PopulateActionsList();
        missionSystemActive = true;
        StartCoroutine(MissionHeartbeat());
    }
    private void PopulateTaskList()
    {
        for (int i = 0; i < 10; i++)
        {
            taskList.Add(MissionGen.Instance.GenerateRandomMission(TaskList));
        }
    }
    private void PopulateActionsList()
    {
        // TODO: Implement action missions if needed
    }
    
    private IEnumerator MissionHeartbeat()
    {
        while (missionSystemActive)
        {
            yield return new WaitForSeconds(30f); // Check missions every 30 seconds
            CheckActiveMissions();
        }
    }
    
    private void CheckActiveMissions()
    {
        List<Mission> missionsToComplete = new List<Mission>();
        
        foreach (Mission mission in activeTaskList)
        {
            if (mission.IsTimeExpired())
            {
                missionsToComplete.Add(mission);
            }
        }
        
        foreach (Mission mission in missionsToComplete)
        {
            CompleteMission(mission);
        }
    }


    public List<Mission> GetTaskList()
    {
        List<Mission> listToReturn = new List<Mission>();
        foreach(Mission mission in taskList)
        {
            listToReturn.Add(mission);
        }
        return listToReturn;
    }
    public List<Mission> GetActionList()
    {
        List<Mission> listToReturn = new List<Mission>();
        foreach (Mission mission in actionList)
        {
            //TODO
        }
        return listToReturn;
    }
    public List<Mission> GetInProgressList()
    {
        List<Mission> listToReturn = new List<Mission>();
        foreach (Mission mission in activeTaskList)
        {
            if (mission.GetMissionState() == MissionState.InProgress)
            {
                listToReturn.Add(mission);
            }
        }
        //TODO add Actions
        return listToReturn;
    }
    public List<Mission> GetCompletedList()
    {
        List<Mission> listToReturn = new List<Mission>();
        foreach (Mission mission in completedMissionList)
        {
            if (mission.GetMissionState() == MissionState.Completed)
            {
                listToReturn.Add(mission);
            }
        }        
        //TODO add Actions
        return listToReturn;
    }
    
    public void StartMission(Mission mission)
    {
        if (mission.GetMissionState() == MissionState.Available && mission.GetAssignedAdventurerCount() > 0)
        {
            mission.BeginMission();
            
            // Move from available to active list
            if (taskList.Contains(mission))
            {
                activeTaskList.Add(mission);
                taskList.Remove(mission);
                mission.transform.SetParent(InProgressList);
            }
            
            // Trigger UI refresh to update all mission tabs immediately
            if (UIManager.Instance != null)
            {
                UIManager.Instance.RefreshMissionLists();
            }
        }
    }
    
    private void CompleteMission(Mission mission)
    {
        // Calculate mission results
        MissionResult result = mission.CalculateMissionSuccess();
        MissionRewards rewards = mission.CalculateRewards(result);
        
        // Distribute rewards through Guild
        Guild.Instance.DistributeMissionRewards(rewards);
        
        // Update mission state
        mission.OnMissionEnd();
        
        // Move mission to completed list
        activeTaskList.Remove(mission);
        completedMissionList.Add(mission);
        
        // Update Transform hierarchy
        if (mission.transform != null && CompleteList != null)
        {
            mission.transform.SetParent(CompleteList);
        }
        
        // Trigger UI update for mission completion
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshMissionLists();
        }
    }
    
    public List<Mission> GetCompletedMissions()
    {
        return new List<Mission>(completedMissionList);
    }
    
    public void RefreshAvailableMissions()
    {
        // Remove old completed missions (keep last 20)
        while (completedMissionList.Count > 20)
        {
            Mission missionToRemove = completedMissionList[0];
            completedMissionList.RemoveAt(0);
            
            // Clean up the mission GameObject
            if (missionToRemove != null && missionToRemove.gameObject != null)
            {
                Destroy(missionToRemove.gameObject);
            }
        }
        
        // Add new missions to task list
        while (taskList.Count < 10)
        {
            taskList.Add(MissionGen.Instance.GenerateRandomMission(TaskList));
        }
    }
    
    public void ClearCompletedMissions()
    {
        foreach (Mission mission in completedMissionList)
        {
            if (mission != null && mission.gameObject != null)
            {
                Destroy(mission.gameObject);
            }
        }
        completedMissionList.Clear();
        
        // Trigger UI refresh
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshMissionLists();
        }
    }
}
