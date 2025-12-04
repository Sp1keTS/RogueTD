using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConstructionGridManager
{
    static public Grid constructionGrid;
    static public Dictionary<Vector2,Building> buildingsSpace = new Dictionary<Vector2,Building>();
    static public Dictionary<Vector2, Building> buildingsPos = new Dictionary<Vector2,Building>();
    static public List<Building> buildings;
    
    
    /// <summary>
    /// Removes a building from the grid state
    /// </summary>
    /// <param name="buildingToRemove"></param>
    public static void RemoveBuilding(Building buildingToRemove)
    {
        buildings.Remove(buildingToRemove);
        foreach (var pair in buildingsSpace.Where(p => p.Value == buildingToRemove).ToList())
        {
            buildingsSpace.Remove(pair.Key);
        }
        foreach (var pair in buildingsPos.Where(p => p.Value == buildingToRemove).ToList())
        {
            buildingsPos.Remove(pair.Key);
        }
    }

    public static bool  CanPlaceBuilding(Vector2 gridPos, BuildingBlueprint buildingBlueprint)
    {
        for (int x = 0; x < buildingBlueprint.Size.x; x++)
        {
            for (int y = 0; y < buildingBlueprint.Size.y; y++)
            {
                Vector2 checkPos = gridPos + new Vector2(x, y);
                if (buildingsSpace.ContainsKey(checkPos) && buildingsSpace[checkPos] != null)
                {
                    return false; 
                }
            }
        }
        return true;
    }
    
    public static void UpdateGridWithBuilding(Vector2 gridPos, Building building, BuildingBlueprint buildingBlueprint)
    {
        buildingsPos[gridPos] = building;
        for (int x = 0; x < buildingBlueprint.Size.x; x++)
        {
            for (int y = 0; y < buildingBlueprint.Size.y; y++)
            {
                // Debug.Log(gridPos + new Vector2(x, y) + buildingBlueprint.buildingName);
                Vector2 occupiedPos = gridPos + new Vector2(x, y);
                ConstructionGridManager.buildingsSpace[occupiedPos] = building;
            }
        }
    }
}
