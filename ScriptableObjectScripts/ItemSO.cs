using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public int combat;
    public int healing;
    public int social;
    public int subterfuge;
    public int hunting;
    public int magic;
    public int craft;
}
