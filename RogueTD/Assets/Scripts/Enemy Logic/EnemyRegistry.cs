using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    [SerializedDictionary("Enemy ID", "Enemy Prefab")]
    public SerializedDictionary<string, Enemy> EnemyPrefabs = new SerializedDictionary<string, Enemy>();
    
    public Enemy GetEnemyPrefab(string enemyId)
    {
        if (EnemyPrefabs.TryGetValue(enemyId, out var enemy))
        {
            return enemy;
        }
        
        return null;
    }
    
    public bool ContainsEnemy(string enemyId)
    {
        return EnemyPrefabs.ContainsKey(enemyId);
    }
}