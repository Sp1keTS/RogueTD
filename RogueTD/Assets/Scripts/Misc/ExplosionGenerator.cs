using System.Collections.Generic;
using UnityEngine;

public static class ExplosionGenerator
{
    public static void CreateExplosion(Vector3 position, float radius, int baseDamage, Tower damageSource = null)
    {
        ApplyDamageInRadius(position, radius, baseDamage, damageSource);
    }
    
    private static void ApplyDamageInRadius(Vector3 position, float radius, int baseDamage, Tower damageSource)
    {
        foreach (var enemyEntry in EnemyManager.Enemies)
        {
            Enemy enemy = enemyEntry.Value;
            
            if (enemy && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                
                if (distance <= radius)
                {
                    float damageMultiplier = CalculateDamageMultiplier(distance, radius);
                    int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
                    
                    if (finalDamage > 0)
                    {
                        enemy.TakeDamage(finalDamage, damageSource);
                    }
                }
            }
        }
    }
    
    public static float CalculateDamageMultiplier(float distance, float maxRadius, AnimationCurve falloffCurve = null)
    {
        if (distance > maxRadius) return 0f;
        
        float normalizedDistance = distance / maxRadius;
        
        if (falloffCurve != null && falloffCurve.length > 0)
        {
            return Mathf.Clamp01(falloffCurve.Evaluate(normalizedDistance));
        }
        
        return 1f - normalizedDistance;
    }
    
    
    public static List<Enemy> GetEnemiesInRadius(Vector3 position, float radius)
    {
        List<Enemy> enemiesInRadius = new List<Enemy>();
        
        foreach (var enemyEntry in EnemyManager.Enemies)
        {
            Enemy enemy = enemyEntry.Value;
            if (enemy && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance <= radius)
                {
                    enemiesInRadius.Add(enemy);
                }
            }
        }
        
        return enemiesInRadius;
    }
}