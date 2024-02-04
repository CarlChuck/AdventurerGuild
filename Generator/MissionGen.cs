using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGen : MonoBehaviour
{
    #region Singleton
    public static MissionGen Instance;
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
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private List<MissionSO> missionTemplates;
    [SerializeField] private List<MissionSO> taskTemplates;


    public Mission GenerateRandomMission(Transform parent)
    {
        GameObject newItemObject = Instantiate(missionPrefab, parent);
        Mission newMission = newItemObject.GetComponent<Mission>();
        newMission.SetMission(GetRandomMissionTemplate());
        return newMission;
    }
    private MissionSO GetRandomMissionTemplate()
    {
        int rand = Random.Range(1, missionTemplates.Count);
        return missionTemplates[rand];
    }
}
