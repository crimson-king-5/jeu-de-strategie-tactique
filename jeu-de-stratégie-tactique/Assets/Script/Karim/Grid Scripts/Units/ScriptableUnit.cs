using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction faction;
    public UnitClass unitUnitClass;
    public string unitsName;
    public Sprite renderUnit;
    [Range(0, 99)] public float classBonus = 1.5f; 
    [SerializeField] public UnitStats unitStats;
    public ScriptableUnit GetCloneUnit()
    {
       return Instantiate(this);
    }
}

[System.Serializable]
public class UnitStats
{
    public float life;
    [Range(0, 99)] public float lifemax;
    [Range(0, 99)] public int armor;
    [Range(0, 99)] public int atk;
    [Range(0, 99)] public int mv;
    [Range(0, 99)] public int range;
}
public enum Faction
{
    Hero = 0,
    Enemy = 1,
    Building = 3
}

public enum UnitClass
{
    Tank,Warrior,Mage,Neutral
}
