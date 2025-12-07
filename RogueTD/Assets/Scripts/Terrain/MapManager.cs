using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] Tilemap  tilemap;
    [SerializeField] Vector2Int size;
    [SerializeField] int PathCount;
    [SerializedDictionary("tile_id", "tile")] 
    public SerializedDictionary<string,CustomTile> tiles;
    private TerrainMap _terrainMap;
    public Vector2Int Size => size;
    void Awake()
    {
        _terrainMap = new TerrainMap(tilemap);
        _terrainMap.Tiles = tiles;
    }
    [ContextMenu("CreateMap")]
    public void CreateMap()
    {
        _terrainMap.CreateEmptyMap(size);
        _terrainMap.CreatePaths(size, PathCount);
    }
}
