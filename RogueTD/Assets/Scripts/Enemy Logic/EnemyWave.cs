using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class EnemyWave 
{
    private List<SubWave> subWaves;
    private List<Vector2> spawnPoints;

    public List<SubWave> SubWaves 
    { 
        get => subWaves; 
        set => subWaves = value; 
    }
    
    public List<Vector2> SpawnPoints 
    { 
        get => spawnPoints; 
        set => spawnPoints = value; 
    }
    
    [Serializable]
    public class SubWave
    {
        public AYellowpaper.SerializedCollections.SerializedDictionary<Vector2, AYellowpaper.SerializedCollections.SerializedDictionary<string, int>> enemiesPerSpawnpoint;
    }
}