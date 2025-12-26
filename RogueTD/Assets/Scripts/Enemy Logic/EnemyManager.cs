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
    
        string uniqueName = GenerateUniqueEnemyName(enemyInstance.EnemyName);
        enemyInstance.gameObject.name = uniqueName;
        
        enemyInstance.InitializeImmediate();
        RegisterEnemy(enemyInstance);
    
        return enemyInstance;
    }
    
    public static void RegisterEnemy(Enemy enemy)
    {
        if (!enemy) return;
        
        string enemyName = enemy.gameObject.name;
        
        if (string.IsNullOrEmpty(enemyName))
        {
            enemyName = GenerateUniqueEnemyName(enemy.EnemyName);
            enemy.gameObject.name = enemyName;
        }
        
        if (!enemies.ContainsKey(enemyName))
        {
            enemies[enemyName] = enemy;
            enemy.OnDeath += HandleEnemyDeath;
            Debug.Log($"Registered enemy: {enemyName}");
        }
        else
        {
            Debug.LogWarning($"Enemy with name '{enemyName}' already registered!");
        }
    }
    
    private static void HandleEnemyDeath(Enemy enemy, Tower tower)
    {
        OnEnemyDied?.Invoke(enemy);
        UnregisterEnemy(enemy);
    }
    
    public static void UnregisterEnemy(Enemy enemy)
    {
        if (!enemy) return;
        
        string enemyName = enemy.gameObject.name;
        if (!string.IsNullOrEmpty(enemyName) && enemies.ContainsKey(enemyName))
        {
            enemies[enemyName].OnDeath -= HandleEnemyDeath;
            enemies.Remove(enemyName);
            Debug.Log($"Unregistered enemy: {enemyName}");
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
    
    private static string GenerateUniqueEnemyName(string baseName)
    {
        string uniqueName;
        int counter = 0;
        
        do
        {
            uniqueName = $"{baseName}_{System.Guid.NewGuid().ToString("N").Substring(0, 8)}";
            if (counter > 0)
            {
                uniqueName = $"{baseName}_{counter:000}_{System.Guid.NewGuid().ToString("N").Substring(0, 4)}";
            }
            counter++;
        }
        while (enemies.ContainsKey(uniqueName) && counter < 1000);
        
        return uniqueName;
    }
    
    public static bool TryGetEnemy(GameObject gameObject, out Enemy enemy)
    {
        if (gameObject != null && enemies.ContainsKey(gameObject.name))
        {
            enemy = enemies[gameObject.name];
            return true;
        }
        
        enemy = null;
        return false;
    }
    
    public static bool TryGetEnemyByName(string enemyName, out Enemy enemy)
    {
        if (!string.IsNullOrEmpty(enemyName) && enemies.ContainsKey(enemyName))
        {
            enemy = enemies[enemyName];
            return true;
        }
        
        enemy = null;
        return false;
    }
}