using System;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public Vector2Int Position;
    public BuildingBlueprint Blueprint;
    
    public BuildingSaveData(Vector2Int position, BuildingBlueprint blueprint)
    {
        Position = position;
        Blueprint = blueprint;
    }
}
