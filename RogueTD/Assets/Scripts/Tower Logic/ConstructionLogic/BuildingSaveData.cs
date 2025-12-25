using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public Vector2Int Position;
    public string BlueprintName;
    public float CurrentHealth;
    
    public BuildingSaveData(Vector2Int position, BuildingBlueprint blueprint, Building building = null)
    {
        Position = position;
        BlueprintName = blueprint.buildingName;
        CurrentHealth = building != null ? building.CurrentHealthPoints : blueprint.MaxHealthPoints;
    }
    
    public BuildingSaveData()
    {
        Position = Vector2Int.zero;
        BlueprintName = string.Empty;
        CurrentHealth = 0f;
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
    }
}