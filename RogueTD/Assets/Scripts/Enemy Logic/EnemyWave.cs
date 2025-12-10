using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyWave 
{
    private List<SubWave> subWaves;
    private List<Vector2> spawnPoints;

    public List<SubWave> SubWaves { get; set; }
    public List<Vector2> SpawnPoints { get; set; }
    
    [Serializable]
    public class SubWave
    {
        public Dictionary<Vector2, List<Enemy>> enemiesPerSpawnpoint;
    }
}
