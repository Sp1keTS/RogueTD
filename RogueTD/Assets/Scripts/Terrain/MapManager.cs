using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] Tilemap  tilemap;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int PathCount;
    [SerializedDictionary("tile_id", "tile")] 
    public SerializedDictionary<string,CustomTile> tiles;
    private TerrainMap _terrainMap;
    
    void Awake()
    {
        _terrainMap = new TerrainMap(tilemap);
        _terrainMap.Tiles = tiles;
    }
    [ContextMenu("CreateMap")]
    void CreateMap()
    {
        _terrainMap.CreateEmptyMap(width, height);
        _terrainMap.CreatePaths(width, height, PathCount);
    }
}
