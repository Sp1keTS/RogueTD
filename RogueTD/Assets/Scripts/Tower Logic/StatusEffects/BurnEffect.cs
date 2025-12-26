using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Tower Defense/Effects/Burn Effect")]
public class BurnEffect : StatusEffect
{
    [SerializeField] private int baseDamagePerTick = 5;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private Color burnColor = new Color(1f, 0.5f, 0f, 1f);
    [SerializeField] private float reapplyCooldown = 0.5f; 
    
    private class BurnData
    {
        public int stacks = 0;
        public int maxStacks = 5;
        public float lastTickTime = 0f;
        public float lastReapplyTime = 0f;
        public Coroutine activeCoroutine;
        public Color originalColor;
    }
    
    private Dictionary<Enemy, BurnData> enemyBurns = new Dictionary<Enemy, BurnData>();
    
    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        if (!enemy) yield break;
        
        BurnData burnData;
        
        if (!enemyBurns.ContainsKey(enemy))
        {
            burnData = new BurnData();
            enemyBurns[enemy] = burnData;
            
            var enemyRenderer = enemy.EnemyRenderer;
            if (enemyRenderer)
            {
                burnData.originalColor = enemyRenderer.material.color;
            }
        }
        else
        {
            burnData = enemyBurns[enemy];
        }
        
        burnData.lastReapplyTime = Time.time;
        
        if (Time.time >= burnData.lastReapplyTime + reapplyCooldown)
        {
            burnData.stacks = Mathf.Min(burnData.stacks + 1, burnData.maxStacks);
        }
        
        var enemyRendererForColor = enemy.EnemyRenderer;
        if (enemyRendererForColor)
        {
            enemyRendererForColor.material.color = Color.Lerp(burnData.originalColor, burnColor, burnData.stacks * 0.2f);
        }
        
        var endTime = Time.time + duration;
        
        while (Time.time < endTime && enemy && enemy.gameObject.activeInHierarchy)
        {
            if (Time.time >= burnData.lastTickTime + tickInterval)
            {
                burnData.lastTickTime = Time.time;
                
                int damage = baseDamagePerTick * burnData.stacks;
                enemy.TakeDamage(damage, null);
            }
            
            yield return null;
        }
        
        CleanupEffect(enemy, burnData);
    }
    
    public override void OnReapply(Enemy enemy, Coroutine existingCoroutine)
    {
        if (!enemy) return;
        enemy.StopCoroutine(existingCoroutine);
        
        BurnData burnData;
        if (enemyBurns.ContainsKey(enemy))
        {
            burnData = enemyBurns[enemy];
            if (Time.time >= burnData.lastReapplyTime + reapplyCooldown)
            {
                burnData.stacks = Mathf.Min(burnData.stacks + 1, burnData.maxStacks);
                burnData.lastReapplyTime = Time.time;
            }
            
            var enemyRenderer = enemy.EnemyRenderer;
            if (enemyRenderer)
            {
                enemyRenderer.material.color = Color.Lerp(burnData.originalColor, burnColor, burnData.stacks * 0.2f);
            }
        }
        else
        {
            burnData = new BurnData();
            enemyBurns[enemy] = burnData;
            
            var enemyRenderer = enemy.EnemyRenderer;
            if (enemyRenderer)
            {
                burnData.originalColor = enemyRenderer.material.color;
            }
            
            burnData.stacks = 1;
            burnData.lastReapplyTime = Time.time;
        }
        
        burnData.lastTickTime = Time.time;
        
        Coroutine newCoroutine = enemy.StartCoroutine(ApplyEffect(enemy));
        
        burnData.activeCoroutine = newCoroutine;
        
        System.Type effectType = GetType();
        enemy.ReplaceEffectCoroutine(effectType, newCoroutine);
    }
    
    private void CleanupEffect(Enemy enemy, BurnData burnData)
    {
        if (enemy && enemy.gameObject.activeInHierarchy)
        {
            var enemyRenderer = enemy.EnemyRenderer;
            if (enemyRenderer)
            {
                enemyRenderer.material.color = burnData.originalColor;
            }
        }
        
        if (enemyBurns.ContainsKey(enemy))
        {
            enemyBurns.Remove(enemy);
        }
    }
}