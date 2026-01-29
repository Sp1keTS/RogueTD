using System;
using System.Reflection;
using UnityEngine;

public abstract class BuildingNode<TBlueprint> : TreeNode where TBlueprint : BuildingBlueprint, new()
{
    protected BuildingBlueprint _buildingBlueprint;
    public BuildingBlueprint BuildingBlueprint
    {
        get => _buildingBlueprint;
        set => _buildingBlueprint = value;
    }

    public BuildingNode(float rankMultiplier, BuildingNodeConfig towerConfig)
    {
        if (_buildingBlueprint == null)
        {
            _buildingBlueprint = new TBlueprint();
        }
        BuildingBlueprint.Cost = towerConfig.BuildingCost;
        Cost = towerConfig.Cost;
        BuildingBlueprint.BuildingName = towerConfig.BuildingName;
        BuildingBlueprint.BuildingPrefab = towerConfig.BuildingPrefab;
        BuildingBlueprint.MaxHealthPoints = (int)(towerConfig.MaxHealthPoints * rankMultiplier);
        BuildingBlueprint.Size = towerConfig.Size;
    }


    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.25f));
    }

}