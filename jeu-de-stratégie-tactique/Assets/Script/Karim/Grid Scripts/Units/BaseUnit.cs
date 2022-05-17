using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile;
    [SerializeField]private ScriptableUnit refScriptableUnit;
    [HideInInspector]public ScriptableUnit scriptableUnit;

    void Start()
    {
        InitUnit(refScriptableUnit, ref scriptableUnit);
    }

    static void InitUnit(ScriptableUnit refUnit, ref ScriptableUnit unit)
    {
        unit = refUnit;
    }
}
