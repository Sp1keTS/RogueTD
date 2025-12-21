using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public Vector2Int Position;
    public string BlueprintName;
    
    public BuildingSaveData(Vector2Int position, BuildingBlueprint blueprint)
    {
        Position = position;
        BlueprintName = blueprint.name;
    }
    public BuildingSaveData()
    {
        Position = Vector2Int.zero;
        BlueprintName = string.Empty;
    }
    [JsonIgnore] 
    public BuildingBlueprint Blueprint
    {
        get
        {
            if (string.IsNullOrEmpty(BlueprintName))
                return null;
                
            if (BlueprintManager.blueprints != null && 
                BlueprintManager.blueprints.TryGetValue(BlueprintName, out var blueprint))
            {
                return blueprint;
            }
            
            Debug.LogWarning($"Blueprint '{BlueprintName}' not found in BlueprintManager!");
            return null;
        }
        set
        {
            if (value != null)
            {
                BlueprintName = value.name;
                if (BlueprintManager.blueprints != null)
                {
                    BlueprintManager.blueprints[BlueprintName] = value;
                }
            }
        }
    }
}