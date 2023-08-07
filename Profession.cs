using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Profession", menuName = "ScriptableObject/Profession")]
public class Profession : ScriptableObject
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
public enum IfCrafter { NA, Weaponsmith, Armoursmith, Leatherworker, Tailor, Fletcher, Jeweller, Carpenter}