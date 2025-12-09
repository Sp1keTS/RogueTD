using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class EnemyManager
{
    private static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
    private static int enemyCounter = 0;
    
    public static event Action<Enemy> OnEnemyDied;
    
    public static IReadOnlyDictionary<string, Enemy> Enemies => enemies;
    public static int EnemyCount => enemies.Count;
    
    public static Enemy CreateEnemy(EnemyData data, Vector2 position, Transform parent = null)
    {
        if (data == null)
        {
            Debug.LogError("Cannot create enemy: EnemyData is null!");
            return null;
        }
    
        string enemyId = $"{data.EnemyName}_{enemyCounter++}";
    
        GameObject enemyObj = new GameObject(enemyId);
        enemyObj.transform.position = position;
    
        if (parent != null)
            enemyObj.transform.SetParent(parent);
    
        var enemy = enemyObj.AddComponent<Enemy>();
        var collider2D = enemyObj.AddComponent<BoxCollider2D>();
        collider2D.size = data.Size;
    
        var rb = enemyObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    
        var spriteRenderer = enemyObj.AddComponent<SpriteRenderer>();
        if (data.Texture != null)
        {
            spriteRenderer.sprite = Sprite.Create(data.Texture, 
                new Rect(0, 0, data.Texture.width, data.Texture.height), 
                new Vector2(0.5f, 0.5f));
        }
        spriteRenderer.color = Color.white;
    
        enemy.Initialize(data);
        RegisterEnemy(enemy);
    
        return enemy;
    }
    
    
    public static void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null || enemy.Model == null) return;
        
        string id = enemy.Model.Id;
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
        if (!enemy || enemy.Model == null) return;
        
        string id = enemy.Model.Id;
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
            if (enemy != null)
            {
                enemy.OnDeath -= HandleEnemyDeath;
                UnityEngine.Object.Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
    }
}
