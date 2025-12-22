using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    private static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
    
    public static event Action<Enemy> OnEnemyDied;
    
    public static IReadOnlyDictionary<string, Enemy> Enemies => enemies;
    public static int EnemyCount => enemies.Count;
    
    public static Enemy SpawnEnemy(Enemy enemyPrefab, Vector2 position, Transform parent = null)
    {
        if (!enemyPrefab)
        {
            Debug.LogWarning("Cannot spawn enemy: prefab is null!");
            return null;
        }
    
        var enemyInstance = GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
        
        if (parent)
            enemyInstance.transform.SetParent(parent);
    
        enemyInstance.InitializeImmediate();
        RegisterEnemy(enemyInstance);
    
        return enemyInstance;
    }
    
    public static void RegisterEnemy(Enemy enemy)
    {
        if (!enemy) return;
        
        string id = enemy.Id;
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning($"Cannot register enemy with null or empty ID: {enemy.EnemyName}");
            return;
        }
        
        if (!enemies.ContainsKey(id))
        {
            enemies[id] = enemy;
            enemy.OnDeath += HandleEnemyDeath;
        }
    }
    
    private static void HandleEnemyDeath(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
        UnregisterEnemy(enemy);
    }
    
    public static void UnregisterEnemy(Enemy enemy)
    {
        if (!enemy) return;
        
        string id = enemy.Id;
        if (!string.IsNullOrEmpty(id) && enemies.ContainsKey(id))
        {
            enemies[id].OnDeath -= HandleEnemyDeath;
            enemies.Remove(id);
        }
    }
    
    public static void ClearAllEnemies()
    {
        foreach (var enemy in enemies.Values)
        {
            if (enemy)
            {
                enemy.OnDeath -= HandleEnemyDeath;
                GameObject.Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
    }
}