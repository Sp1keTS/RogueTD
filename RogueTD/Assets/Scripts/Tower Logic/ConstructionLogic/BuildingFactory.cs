using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildingFactory : MonoBehaviour
{
    
    public static Building CreateProjectileTower(Vector2 gridPos, ProjectileTowerBlueprint blueprint)
    {

        // можно ли разместить постройку
        if (!ConstructionGridManager.CanPlaceBuilding(gridPos, blueprint))
        {
            Debug.LogWarning($"Cannot place building at position {gridPos} - not enough space");
            return null;
        }
        
        // Рассчет мировой позиции
        Vector3 worldPosition = GetWorldPosition(gridPos, blueprint);
        
        // создание объекта
        GameObject buildingObj = Instantiate(blueprint.BuildingPrefab, worldPosition, Quaternion.identity);
        Building building = buildingObj.GetComponent<Building>();
        
        // Инициализация Building
        building.InitializeFromBlueprint(blueprint);
        
        // дочерний объект башни
        var tower = CreateProjectileTowerChild(buildingObj, blueprint, worldPosition);
        tower.MainBuilding = building;
        building.Tower = tower;
        
        // Обновление сетки со ссылкой на созданное здание
        ConstructionGridManager.UpdateGridWithBuilding(gridPos, building, blueprint);
        return building;
    }

    public static Building CreateBuilding(Vector2 gridPos, BuildingBlueprint buildingBlueprint)
    {
        if (!ConstructionGridManager.CanPlaceBuilding(gridPos, buildingBlueprint))
        {
            Debug.LogWarning($"Cannot place building at position {gridPos} - not enough space");
            return null;
        }
        Vector3 worldPosition = GetWorldPosition(gridPos, buildingBlueprint);
        GameObject buildingObj = Instantiate(buildingBlueprint.BuildingPrefab, worldPosition, Quaternion.identity);
        Building building = buildingObj.GetComponent<Building>();
        building.InitializeFromBlueprint(buildingBlueprint);
        ConstructionGridManager.UpdateGridWithBuilding(gridPos, building, buildingBlueprint);
        return building;
    }
    
    public static void DestroyBuilding(Building buildingToDestroy)
    {
        if (buildingToDestroy != null)
        {
            ConstructionGridManager.RemoveBuilding(buildingToDestroy);
            Destroy(buildingToDestroy.gameObject);
        }
    }
    
    private static Vector3 GetWorldPosition(Vector2 gridPos, BuildingBlueprint buildingBlueprint)
    {
        Vector3 baseWorldPos = ConstructionGridManager.constructionGrid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0));
        
        float offsetX = (buildingBlueprint.Size.x - 1) * ConstructionGridManager.constructionGrid.cellSize.x * 0.5f;
        float offsetY = (buildingBlueprint.Size.y - 1) * ConstructionGridManager.constructionGrid.cellSize.y * 0.5f;
        
        return new Vector3(baseWorldPos.x + offsetX, baseWorldPos.y + offsetY, baseWorldPos.z);
    }
    
    private static ProjectileTower CreateProjectileTowerChild(GameObject buildingObj, ProjectileTowerBlueprint blueprint, Vector3 position)
    {
        position.z = -1;
        if (blueprint.TowerPrefab != null)
        {
            GameObject towerObj = Instantiate(blueprint.TowerPrefab, position, Quaternion.identity, buildingObj.transform);
            ProjectileTower projectileTower = towerObj.GetComponent<ProjectileTower>();
            Debug.Log(projectileTower + " турель создана");
            if (projectileTower != null)
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

    public static void UpdateProjectileTowers(ProjectileTowerBlueprint blueprint, string name)
    {
        Debug.Log($"Апдейт происходит");
        
        Debug.Log(ConstructionGridManager.buildingsSpace.Count);
        foreach (var buildingPosition in ConstructionGridManager.buildingsSpace)
        {
            var building = buildingPosition.Value;
            Debug.Log(building.GetType());
            Debug.Log(building.BuildingName);
            if (building.BuildingName == name)
            {
                ProjectileTower projectileTower = building.transform.GetComponentInChildren<ProjectileTower>();
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