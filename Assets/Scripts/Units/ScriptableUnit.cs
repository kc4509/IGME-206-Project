//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Different types of units

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction faction;
    public BaseUnit unitPrefab;
}

//list types of sprites
public enum Faction
{
    Hero = 0,
    Enemy = 1
}