using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEngine;

public class Building : Unit
{
    public BoundsInt area;
    public GridBuildingSystem GridBuildingSystem
    {
        get => _gameManager.GridBuildingSystem;
    }

    //private ScriptableBuildings _updateGradeBuilding;
    //public ScriptableBuildings UpdateGradeBuilding
    //{
    //    get => _updateGradeBuilding;
    //}
}
