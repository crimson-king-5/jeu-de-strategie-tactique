using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEngine;

public class Building : Unit
{
    [SerializeField] private Productor _productor = new Productor();

    public Faction Faction
    {
        get => _scrUnit.faction;
    }

    public ResourceType resourceType;

    public UprgadeList UpgradeList
    {
        get
        {
            ScriptableBuilding building = (ScriptableBuilding)_scrUnit;
            return building.upgrades;
        }

    }

    public GridBuildingSystem GridBuildingSystem
    {
        get => _gameManager.GridBuildingSystem;
    }

    public int currentlevelBuilding = 0;

    public void Upgrade()
    {
        if (currentlevelBuilding < UpgradeList.upgrades.Count)
        {
            currentlevelBuilding++;
            SwitchObject(currentlevelBuilding);
        }
    }

    public float GainResourcePerTurn(float currentResource)
    {
        return (currentResource + _productor.resource * UpgradeList.upgrades[currentlevelBuilding].gainBonus);
    }

    void SwitchObject(int lvl)
    {
        for (int i = 0; i < UpgradeList.upgrades.Count; i++)
        {
            if (i == lvl)
            {
                GetComponent<SpriteRenderer>().sprite = UpgradeList.upgrades[i].upgradeRender;
            }
        }
    }

}