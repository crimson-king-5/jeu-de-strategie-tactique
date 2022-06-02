using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction faction;
    public string unitsName;
    public Sprite renderUnit;
    [SerializeField] public UnitStats unitStats;
    public ScriptableUnit GetCloneUnit()
    {
       return Instantiate(this);
    }
}

[System.Serializable]
public class UnitStats
{
    [Range(0, 99)] public int lifeMax;
    public int life;
    [Range(0, 99)] public int atk;
    [Range(0, 99)] public int mv;
    [Range(0, 99)] public int range;
}
public enum Faction
{
    Hero = 0,
    Enemy = 1,
    Neutral = 3
}
