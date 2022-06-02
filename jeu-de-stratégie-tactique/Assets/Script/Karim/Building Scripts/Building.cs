using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEngine;

public class Building : Unit
{
    public bool Placed { get; private set; }
    public BoundsInt area;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region Build Metods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }
     
    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.TakeArea(areaTemp);
    }


    #endregion
}
