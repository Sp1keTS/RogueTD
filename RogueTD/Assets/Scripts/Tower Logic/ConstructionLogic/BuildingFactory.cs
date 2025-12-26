using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildingFactory : MonoBehaviour
{
    static public Action<Vector2Int, BuildingBlueprint, Building> OnBuildingCreated;
    public static Building CreateProjectileTower(Vector2Int gridPos, ProjectileTowerBlueprint blueprint, float customHealth = -1)
    {
        if (!CanPlaceBuilding(gridPos, blueprint))
        {
            Debug.LogWarning($"Cannot place building at position {gridPos} - not enough space");
            return null;
        }
    
        // Рассчет мировой позиции
        var worldPosition = GetWorldPosition(gridPos);
    
        // создание объекта
        var buildingObj = Instantiate(blueprint.BuildingPrefab, worldPosition, Quaternion.identity);
        var building = buildingObj.GetComponent<Building>();
    
        building.buildingName = blueprint.buildingName;
        building.GridPosition = gridPos; // Добавляем установку GridPosition
    
        // Инициализация Building с кастомным здоровьем если указано
        building.Initialize(blueprint.MaxHealthPoints, customHealth);
    
        // дочерний объект башни
        var tower = CreateProjectileTowerChild(buildingObj, blueprint, worldPosition);
    
        // Обновление сетки с ссылкой на созданное здание
        UpdateGridWithBuilding(gridPos, building, blueprint);
    
        // Передаем здание в событие
        OnBuildingCreated?.Invoke(gridPos, blueprint, building);
    
        return building;
    }

    public static Building CreateBuilding(Vector2Int gridPos, BuildingBlueprint blueprint, float customHealth = -1)
    {
        if (!CanPlaceBuilding(gridPos, blueprint))
        {
            Debug.LogWarning($"Cannot place building at position {gridPos} - not enough space");
            return null;
        }
    
        var worldPosition = GetWorldPosition(gridPos);
        var buildingObj = Instantiate(blueprint.BuildingPrefab, worldPosition, Quaternion.identity);
        var building = buildingObj.GetComponent<Building>();
        building.buildingName = blueprint.buildingName;
        building.GridPosition = gridPos;
    
        building.Initialize(blueprint.MaxHealthPoints, customHealth);
    
        OnBuildingCreated?.Invoke(gridPos, blueprint, building);
        UpdateGridWithBuilding(gridPos, building, blueprint);
    
        return building;
    }
    
    public static void DestroyBuilding(Building buildingToDestroy)
    {
        if (buildingToDestroy)
        {
            ConstructionGridManager.RemoveBuilding(buildingToDestroy);
            Destroy(buildingToDestroy.gameObject);
        }
    }
    
    public static bool  CanPlaceBuilding(Vector2 gridPos, BuildingBlueprint buildingBlueprint)
    {
        for (var x = 1; x < buildingBlueprint.Size.x +1; x++)
        {
            for (var y = 1; y < buildingBlueprint.Size.y +1; y++)
            {
                var checkPos = gridPos + new Vector2(x, y);
                if (ConstructionGridManager.BuildingsSpace.ContainsKey(checkPos) && ConstructionGridManager.BuildingsSpace[checkPos] != null)
                {
                    return false; 
                }
            }
        }
        return true;
    }
    
    public static Vector3 GetWorldPosition(Vector2 gridPos)
    {
        var baseWorldPos = ConstructionGridManager.ConstructionGrid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0));
        
        var offsetX = ConstructionGridManager.ConstructionGrid.cellSize.x * 0.5f;
        var offsetY = ConstructionGridManager.ConstructionGrid.cellSize.y * 0.5f;
        
        return new Vector3(baseWorldPos.x + offsetX, baseWorldPos.y + offsetY, baseWorldPos.z);
    }
    
    private static ProjectileTower CreateProjectileTowerChild(GameObject buildingObj, ProjectileTowerBlueprint blueprint, Vector3 position)
    {
        position.z = -1;
        if (blueprint.TowerPrefab)
        {
            var towerObj = Instantiate(blueprint.TowerPrefab, position, Quaternion.identity, buildingObj.transform);
            var projectileTower = towerObj.GetComponent<ProjectileTower>();
            Debug.Log(projectileTower);
            if (projectileTower)
            {
                projectileTower.InitializeFromBlueprint(blueprint);
                return projectileTower;
            }
            else
            {
                Debug.LogWarning("TowerPrefab doesn't have ProjectileTower component");
            }
        }

        return new ProjectileTower();
    }
    
    private static void UpdateGridWithBuilding(Vector2 gridPos, Building building, BuildingBlueprint buildingBlueprint)
    {
        ConstructionGridManager.BuildingsPos[gridPos] = building;
        for (var x = 0; x < buildingBlueprint.Size.x; x++)
        {
            for (var y = 0; y < buildingBlueprint.Size.y; y++)
            {
                Vector2 occupiedPos = gridPos + new Vector2(x, y);
                ConstructionGridManager.BuildingsSpace[occupiedPos] = building;
            }
        }
    }

    public static void UpdateProjectileTowers(ProjectileTowerBlueprint blueprint, string name)
    {
        foreach (var buildingPosition in ConstructionGridManager.BuildingsSpace)
        {
            var building = buildingPosition.Value;
            Debug.Log(building.GetType());
            Debug.Log(building.buildingName);
            if (building.buildingName == name)
            {
                var projectileTower = building.transform.GetComponentInChildren<ProjectileTower>();
                Debug.Log(projectileTower);
                projectileTower.InitializeFromBlueprint(blueprint);
            }
        }
    }
    
    static BuildingFactory()
    {
        BlueprintManager.ProjectileTowerChanged += UpdateProjectileTowers;
    }
}