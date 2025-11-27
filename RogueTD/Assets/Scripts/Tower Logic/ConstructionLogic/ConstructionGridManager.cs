using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConstructionGridManager
{
    static public Grid constructionGrid;
    static public Dictionary<Vector2,Building> buildingsSpace = new Dictionary<Vector2,Building>();
    static public Dictionary<Vector2, Building> buildingsPos = new Dictionary<Vector2,Building>();
    
    /// <summary>
    /// Removes a building from the grid state
    /// </summary>
    /// <param name="buildingToRemove"></param>
    static public void RemoveBuilding(Building buildingToRemove)
    {
        foreach (var pair in buildingsSpace.Where(p => p.Value == buildingToRemove).ToList())
        {
            buildingsSpace.Remove(pair.Key);
        }
        foreach (var pair in buildingsPos.Where(p => p.Value == buildingToRemove).ToList())
        {
            buildingsPos.Remove(pair.Key);
        }
    }
}
