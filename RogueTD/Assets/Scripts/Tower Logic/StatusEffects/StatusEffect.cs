using System.Collections;
using UnityEngine;

public abstract class StatusEffect : Resource
{
    public string effectName;
    public float duration = 2;
    
    public abstract IEnumerator ApplyEffect(Enemy enemy);
    
    public virtual void OnReapply(Enemy enemy, Coroutine existingCoroutine)
    {
        enemy.StopCoroutine(existingCoroutine);
        var newCoroutine = enemy.StartCoroutine(ApplyEffect(enemy));
        
        var effectType = GetType();
        enemy.ReplaceEffectCoroutine(effectType, newCoroutine);
    }
}