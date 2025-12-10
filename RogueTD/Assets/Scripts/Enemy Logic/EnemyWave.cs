using System;
using System.Collections.Generic;
using UnityEngine;

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
        [NonSerialized]
        public Dictionary<Vector2, List<(EnemyModel model, int count)>> enemiesPerSpawnpoint;
    }
}