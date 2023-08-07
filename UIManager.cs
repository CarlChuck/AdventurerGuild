using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
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

    [SerializeField] private GameObject rosterTab;
    [SerializeField] private GameObject taskTab;
    [SerializeField] private GameObject actionTab;
    [SerializeField] private GameObject inProgressTab;
    [SerializeField] private GameObject completedTab;
    [SerializeField] private GameObject marketTab;
    [SerializeField] private GameObject recruitTab;

    private void Start()
    {
        SetAllInactive();
    }
    public void SetTabActive(int tabNum)
    {
        SetAllInactive();
        switch (tabNum)
        {
            case 1:
                rosterTab.SetActive(true);
                break;
            case 2:
                taskTab.SetActive(true);
                break;
            case 3:
                actionTab.SetActive(true);
                break;
            case 4:
                inProgressTab.SetActive(true);
                break;
            case 5:
                completedTab.SetActive(true);
                break;
            case 6:
                marketTab.SetActive(true);
                break;
            case 7:
                recruitTab.SetActive(true);
                break;
            default:
                rosterTab.SetActive(true);
                break;
        }
    }
    private void SetAllInactive()
    {
        rosterTab.SetActive(false);
        taskTab.SetActive(false);
        actionTab.SetActive(false);
        inProgressTab.SetActive(false);
        completedTab.SetActive(false);
        marketTab.SetActive(false);
        recruitTab.SetActive(false);
    }
}
