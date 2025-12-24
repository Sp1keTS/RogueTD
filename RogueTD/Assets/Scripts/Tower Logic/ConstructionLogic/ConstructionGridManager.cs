using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionGridManager : MonoBehaviour
{
    public static Grid ConstructionGrid { get; set; }
    static public Dictionary<Vector2, Building> buildingsSpace = new Dictionary<Vector2, Building>();
    static public Dictionary<Vector2, Building> buildingsPos = new Dictionary<Vector2, Building>();
    
    [SerializeField] private static List<BuildingSaveData> savePoses = new List<BuildingSaveData>();
    
    private bool isRecreatingBuildings = false; 
    
    static public Dictionary<Vector2, Building> BuildingsSpace => buildingsSpace;
    static public Dictionary<Vector2, Building> BuildingsPos {get => buildingsPos;set => buildingsPos = value; }
    public static List<BuildingSaveData> SavePoses { get => savePoses; set => savePoses = value; }

    private void Awake()
    {
        BuildingFactory.OnBuildingCreated += OnBuildingCreated;
        Building.onBuildingDestroyed += OnBuildingDestroyed;
        
        if (GameState.Instance != null && GameState.Instance.Buildings != null)
        {
            savePoses = new List<BuildingSaveData>(GameState.Instance.Buildings);
            
            buildingsSpace.Clear();
            buildingsPos.Clear();
            
            RecreateBuildings();
        }
    }
    
    private void OnDestroy()
    {
        BuildingFactory.OnBuildingCreated -= OnBuildingCreated;
    }

    private void OnBuildingDestroyed(Vector2Int gridPos)
    {
        RemoveSavedBuilding(gridPos);
        GameState.Instance.SaveBuildingsToJson();
    }

    static public void RemoveBuilding(Building buildingToRemove)
    {
        foreach (var pair in BuildingsSpace.Where(p => p.Value == buildingToRemove).ToList())
        {
            BuildingsSpace.Remove(pair.Key);
        }
        foreach (var pair in BuildingsPos.Where(p => p.Value == buildingToRemove).ToList())
        {
            BuildingsPos.Remove(pair.Key);
        }
    }

    public void OnBuildingCreated(Vector2Int gridPos, BuildingBlueprint buildingBlueprint)
    {
        if (isRecreatingBuildings) return;
        
        bool alreadyExists = savePoses.Any(data => data.Position == gridPos);
        
        if (!alreadyExists)
        {
            savePoses.Add(new BuildingSaveData(gridPos, buildingBlueprint));
            
            if (GameState.Instance != null)
            {
                GameState.Instance.Buildings = new List<BuildingSaveData>(savePoses); 
            }
        }
    }

    static public void TryCreateBlueprint(BuildingBlueprint blueprint, Vector2Int gridPosition)
    {
        if (blueprint == null) return;
        
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        { 
            BuildingFactory.CreateProjectileTower(gridPosition, projectileBlueprint);
        }
        else
        {
            BuildingFactory.CreateBuilding(gridPosition, blueprint);
        }
    }
    
    public void RecreateBuildings()
    {
    
        if (GameState.Instance == null || GameState.Instance.Buildings == null || GameState.Instance.Buildings.Count == 0)
        {
            return;
        }
    
        isRecreatingBuildings = true;
    
        try
        {
            var buildingsToRecreate = new List<BuildingSaveData>(GameState.Instance.Buildings);
        
        
            foreach (var saveData in buildingsToRecreate)
            {
                if (saveData.BlueprintName == "MainBuildingBlueprint")
                {
                    continue;
                }
            
                var blueprint = saveData.Blueprint;
                if (blueprint == null)
                {
                    continue;
                }
            
                TryCreateBlueprint(blueprint, saveData.Position);
            }
        
        }
        finally
        {
            isRecreatingBuildings = false;
        }
    }
    
    public void ClearAllSavedBuildings()
    {
        savePoses.Clear();
        
        if (GameState.Instance != null)
        {
            GameState.Instance.Buildings = new List<BuildingSaveData>();
        }
        
    }
    
    public void RemoveSavedBuilding(Vector2Int position)
    {
        savePoses.RemoveAll(data => data.Position == position);
        
        if (GameState.Instance != null)
        {
            GameState.Instance.Buildings = new List<BuildingSaveData>(savePoses);
        }
        
    }
}