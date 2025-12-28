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
    static public Dictionary<Vector2, Building> BuildingsPos
    {
        get{ return buildingsPos; }
        set => buildingsPos = value;
    }

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
        Building.onBuildingDestroyed -= OnBuildingDestroyed;
    }

    private void OnBuildingDestroyed(Vector2Int gridPos)
    {
        RemoveBuildingFromGrid(gridPos);
        RemoveSavedBuilding(gridPos);
    }

    static public void RemoveBuilding(Building buildingToRemove)
    {
        if (buildingToRemove)
        {
            RemoveBuildingFromGrid(buildingToRemove.GridPosition);
            
            foreach (var pair in BuildingsSpace.Where(p => p.Value == buildingToRemove).ToList())
            {
                BuildingsSpace.Remove(pair.Key);
            }
            foreach (var pair in BuildingsPos.Where(p => p.Value == buildingToRemove).ToList())
            {
                BuildingsPos.Remove(pair.Key);
            }
        }
    }
    
    static private void RemoveBuildingFromGrid(Vector2Int gridPos)
    {
        if (BuildingsPos.TryGetValue(gridPos, out Building building))
        {
            var saveData = savePoses.FirstOrDefault(data => data.Position == gridPos);
            
            var sizeToClear = Vector2Int.one;
            if (saveData != null && saveData.Blueprint != null)
            {
                sizeToClear = new Vector2Int((int)saveData.Blueprint.Size.x, (int)saveData.Blueprint.Size.y);
            }
            
            for (var x = 1; x < sizeToClear.x + 1; x++)
            {
                for (var y = 1; y < sizeToClear.y + 1; y++)
                {
                    Vector2 occupiedPos = gridPos + new Vector2(x, y);
                    if (BuildingsSpace.ContainsKey(occupiedPos))
                    {
                        BuildingsSpace.Remove(occupiedPos);
                    }
                }
            }
            
            BuildingsPos.Remove(gridPos);
        }
    }

    public void OnBuildingCreated(Vector2Int gridPos, BuildingBlueprint buildingBlueprint, Building building = null)
    {
        if (isRecreatingBuildings) return;
    
        var alreadyExists = savePoses.Any(data => data.Position == gridPos);
    
        if (!alreadyExists)
        {
            savePoses.Add(new BuildingSaveData(gridPos, buildingBlueprint, building));
        
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
                var blueprint = saveData.Blueprint;
                if (blueprint == null)
                {
                    continue;
                }
            
                var healthToUse = saveData.CurrentHealth > 0 ? saveData.CurrentHealth : blueprint.MaxHealthPoints;
            
                if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
                {
                    BuildingFactory.CreateProjectileTower(saveData.Position, projectileBlueprint, healthToUse);
                }
                else
                {
                    BuildingFactory.CreateBuilding(saveData.Position, blueprint, healthToUse);
                }
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
        buildingsSpace.Clear();
        buildingsPos.Clear();
        
        if (GameState.Instance != null)
        {
            GameState.Instance.Buildings = new List<BuildingSaveData>();
        }
    }
    
    public void RemoveSavedBuilding(Vector2Int position)
    {
        RemoveBuildingFromGrid(position);
        savePoses.RemoveAll(data => data.Position == position);
        
        if (GameState.Instance != null)
        {
            GameState.Instance.Buildings = new List<BuildingSaveData>(savePoses);
        }
    }
}