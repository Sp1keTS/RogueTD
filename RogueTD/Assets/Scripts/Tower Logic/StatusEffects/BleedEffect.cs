using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PoisonStatusEffect", menuName = "Tower Defense/Effects/Poison Status Effect")]
public class BleedEffect : StatusEffect
{
    [SerializeField] private int totalDamage = 15;
    [SerializeField] private Color bleedColor = Color.red;
    
    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        if (!enemy) yield break;
        
        var renderer = enemy.EnemyRenderer;
        Color originalColor = renderer.material.color;
        
        renderer.material.color += bleedColor/2;
        
        float damagePerTick = totalDamage / (duration / 0.5f);
        float endTime = Time.time + duration;
        
        while (Time.time < endTime && enemy)
        {
            enemy.TakeDamage(Mathf.RoundToInt(damagePerTick),null);
            yield return new WaitForSeconds(0.5f);
        }
        
        if (enemy)
        {
            renderer.material.color = originalColor;
        }
    }
}