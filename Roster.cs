using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roster : MonoBehaviour
{
    #region Singleton
    public static Roster Instance;
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

    [SerializeField] List<Adventurer> adventurers;


}