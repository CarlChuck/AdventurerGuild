using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Profession", menuName = "ScriptableObject/Mission")]
public class MissionSO : ScriptableObject
{
    public IfCrafter ifCrafter;
    public int combat;
    public int healing;
    public int social;
    public int subterfuge;
    public int hunting;
    public int magic;
    public int craft;
}
