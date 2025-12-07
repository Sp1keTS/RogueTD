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
        Coroutine newCoroutine = enemy.StartCoroutine(ApplyEffect(enemy));
        
        System.Type effectType = GetType();
        enemy.ReplaceEffectCoroutine(effectType, newCoroutine);
    }
}