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
        missionList = new();
    }
    #endregion
    [SerializeField] private Transform TaskList;
    [SerializeField] private Transform ActionList;
    [SerializeField] private Transform InProgressList;
    [SerializeField] private Transform CompleteList;
    [SerializeField] private List<Mission> missionList;
    [SerializeField] private List<Mission> activeMissionList;

    public void InitaliseMissions()
    {
        PopulateTaskList();
        PopulateActionsList();
    }
    private void PopulateTaskList()
    {
        for (int i = 0; i < 10; i++)
        {
            missionList.Add(MissionGen.Instance.GenerateRandomMission(TaskList));
        }
    }
    private void PopulateActionsList()
    {

    }


    public List<Mission> GetTaskList()
    {
        List<Mission> listToReturn = new List<Mission>();

        return listToReturn;
    }
    public List<Mission> GetActionList()
    {
        List<Mission> listToReturn = new List<Mission>();

        return listToReturn;
    }
    public List<Mission> GetInProgressList()
    {
        List<Mission> listToReturn = new List<Mission>();

        return listToReturn;
    }
    public List<Mission> GetCompletedList()
    {
        List<Mission> listToReturn = new List<Mission>();

        return listToReturn;
    }
}
