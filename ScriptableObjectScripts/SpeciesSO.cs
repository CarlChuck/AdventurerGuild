using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Species", menuName = "ScriptableObject/Species")]
public class SpeciesSO : ScriptableObject
{
    public SpeciesFamily specFamily;
    public int combat;
    public int healing;
    public int social;
    public int subterfuge;
    public int hunting;
}
public enum SpeciesFamily { Human, Sylvan, Thrakx}