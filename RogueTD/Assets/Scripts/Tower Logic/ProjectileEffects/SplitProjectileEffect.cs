using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitEffect", menuName = "Tower Defense/Effects/Split Effect")]
public class SplitEffect : ProjectileEffect
{
    [SerializeField] private int splitCount = 2;
    [SerializeField] private float splitAngle = 30f;
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        // Создаем новые снаряды при столкновении
        Vector2 baseDirection = projectile.transform.right;
        Debug.Log("снаряд разделился");
        for (int i = 0; i < splitCount; i++)
        {
            float angle = -splitAngle/2 + (i * (splitAngle / (splitCount - 1)));
            Vector2 direction = Quaternion.Euler(0, 0, angle) * baseDirection;
            
            TowerProjectile newProjectile = tower.CreateProjectile(projectile.transform.position + new Vector3(0.5f,0.5f,0));
            newProjectile.EffectsToProcess = new Queue<ProjectileEffect>(projectile.EffectsToProcess);
            if (newProjectile != null)
            {
                newProjectile.rb.linearVelocity = direction * tower.ProjectileSpeed;
                newProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
        }
        
        return true; 
    }
    
    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        return false; 
    }
}

