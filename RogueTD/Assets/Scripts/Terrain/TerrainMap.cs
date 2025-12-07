using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TerrainMap
{
    private Tilemap _tilemap;

    public Dictionary<string, CustomTile> Tiles
    {
        get;
        set;
    }

    public TerrainMap(Tilemap tilemap) => _tilemap = tilemap;

    public void CreateEmptyMap(Vector2Int mapSize)
    {
        for (int x = -mapSize.x; x < mapSize.x; x++)
        {
            for (int y = -mapSize.y; y < mapSize.y; y++)
            {
                _tilemap.SetTile(new Vector3Int(x, y, 0), Tiles["Base_tile"]);
            }
        }
    }

    public void CreatePaths(Vector2Int mapSize, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreatePath(mapSize);
        }
    }

    private void CreatePath(Vector2Int mapSize)
    {
        Vector2Int startDirection = PathDirections.Cardinal[Random.Range(0, PathDirections.Cardinal.Length)];
        Vector2Int currentPosition = Vector2Int.zero;
        Vector2Int currentDirection = startDirection;

        float mainDirectionChance = 0.85f; 

        while (Mathf.Abs(currentPosition.x) < mapSize.x && Mathf.Abs(currentPosition.y) < mapSize.y)
        {
            _tilemap.SetTile((Vector3Int)currentPosition, Tiles["Road_tile"]);
        
            if (Random.Range(0f, 1f) > mainDirectionChance)
            {
                Vector2Int newDirection = GetRandomDirectionExcluding(-currentDirection);
                currentDirection = newDirection;
            }
        
            currentPosition += currentDirection;
        }

        _tilemap.SetTile((Vector3Int)currentPosition, Tiles["Road_tile"]);
    }

    private Vector2Int GetRandomDirectionExcluding(Vector2Int exclude)
    {
        List<Vector2Int> availableDirections = new List<Vector2Int>();
    
        foreach (Vector2Int dir in PathDirections.Cardinal)
        {
            if (dir != exclude)
            {
                availableDirections.Add(dir);
            }
        }
    
        return availableDirections[Random.Range(0, availableDirections.Count)];
    }
    public void SetTile(int x, int y, TileBase tile)
    {
        _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }
}