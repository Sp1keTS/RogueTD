using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public static Dictionary<string, BuildingBlueprint> blueprints = new Dictionary<string, BuildingBlueprint>();
    public static event Action<ProjectileTowerBlueprint, string> ProjectileTowerChanged = delegate { };
    
    public static void InvokeBuildingChanged(ProjectileTowerBlueprint blueprint, string buildingName)
    {
        ProjectileTowerChanged.Invoke(blueprint, buildingName);
    }
    
    
    void Awake()
    {
        if (blueprints == null)
        {
            blueprints = new Dictionary<string, BuildingBlueprint>();
        }
    }
    
    public static void InsertBlueprint(ProjectileTowerBlueprint blueprint)
    {
        if (blueprints == null)
        {
            blueprints = new Dictionary<string, BuildingBlueprint>();
        }

        if (blueprints.ContainsKey(blueprint.buildingName))
        {
            blueprints[blueprint.buildingName] = blueprint;
            InvokeBuildingChanged(blueprint, blueprint.buildingName);
        }
        else
        {
            blueprints.Add(blueprint.buildingName, blueprint);
            InvokeBuildingChanged(blueprint, blueprint.buildingName);
        }
    }
}