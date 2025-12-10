using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    private static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
    
    public static event Action<Enemy> OnEnemyDied;
    
    public static IReadOnlyDictionary<string, Enemy> Enemies => enemies;
    public static int EnemyCount => enemies.Count;
    
    public static Enemy CreateEnemy(EnemyModel model, Vector2 position, Transform parent = null)
    {
        if (model == null)
        {
            Debug.LogError("Cannot create enemy: EnemyModel is null!");
            return null;
        }
    
        GameObject enemyObj = new GameObject(model.EnemyName);
        enemyObj.transform.position = position;
    
        if (parent != null)
            enemyObj.transform.SetParent(parent);
    
        var enemy = enemyObj.AddComponent<Enemy>();
        
        var collider2D = enemyObj.AddComponent<BoxCollider2D>();
        collider2D.size = model.Size;
    
        var spriteRenderer = enemyObj.AddComponent<SpriteRenderer>();
        if (model.Texture != null)
        {
            spriteRenderer.sprite = Sprite.Create(model.Texture, 
                new Rect(0, 0, model.Texture.width, model.Texture.height), 
                new Vector2(0.5f, 0.5f));
        }
        spriteRenderer.color = Color.white;
    
        RegisterEnemy(enemy);
    
        return enemy;
    }
    
    public static void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        
        string id = enemy.Id;
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
        if (enemies.ContainsKey(id))
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
                UnityEngine.Object.Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
    }
}