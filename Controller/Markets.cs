using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markets : MonoBehaviour
{
    [SerializeField] List<Adventurer> marketRecruits;
    [SerializeField] List<Item> marketItems;

    public void GenerateAdventurerList()
    {

    }
    public void GenerateItemList()
    {

    }
    public List<Adventurer> GetAdventurerList()
    {
        return marketRecruits;
    }
    public List<Item> GetItemList()
    {
        return marketItems;
    }
}
