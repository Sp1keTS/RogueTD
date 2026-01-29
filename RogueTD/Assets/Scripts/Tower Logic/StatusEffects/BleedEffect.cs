using UnityEngine;
using System.Collections;


public class BleedEffect : StatusEffect
{
    [SerializeField] private Color bleedColor = Color.red;
    public int Damage { get; set; }

    
    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        if (!enemy) yield break;
        
        var renderer = enemy.EnemyRenderer;
        var originalColor = renderer.material.color;
        renderer.material.color += bleedColor/2;
        
        var endTime = Time.time + Duration;
        
        while (Time.time < endTime && enemy)
        {
            enemy.TakeDamage(Mathf.RoundToInt(Damage),null);
            yield return new WaitForSeconds(0.5f);
        }
        
        if (enemy)
        {
            renderer.material.color = originalColor;
        }
    }
}