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
        missionTemplates = new();
    }
    #endregion
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private List<MissionSO> missionTemplates;


    public void GenerateRandomMission()
    {

    }

}
