using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "HomingRicochetEffect", menuName = "Tower Defense/Effects/Homing Ricochet")]
public class HomingRicochetEffect : ProjectileEffect
{
    [SerializeField] private int maxRicochets = 3;
    [SerializeField] private float homingRadius = 5f;
    [SerializeField] private bool becomesFragileAfterRicochet = true;

    public int MaxRicochets
    {
        get => maxRicochets;
        set => maxRicochets = value;
    }

    public float HomingRadius
    {
        get => homingRadius;
        set => homingRadius = value;
    }

    private Dictionary<TowerProjectile, int> remainingRicochets = new Dictionary<TowerProjectile, int>();
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        if (!remainingRicochets.ContainsKey(projectile))
        {
            remainingRicochets[projectile] = maxRicochets;
            tower.ProjectileFragile = false;
        }
        
        target.TakeDamage(tower.Damage, tower);
        
        if (remainingRicochets[projectile] > 0)
        {
            remainingRicochets[projectile]--;
            
            var nextTarget = FindNextTarget(projectile.transform.position, target);
            if (nextTarget)
            {
                Vector2 direction = (nextTarget.transform.position - projectile.transform.position).normalized;
                projectile.transform.right = direction;
                projectile.rb.linearVelocity = direction * projectile.rb.linearVelocity.magnitude;
            }
            else
            {
                Vector2 currentDirection = projectile.transform.right;
                float randomAngle = Random.Range(-45f, 45f);
                Vector2 newDirection = RotateVector(currentDirection, randomAngle);
                projectile.transform.right = newDirection;
                projectile.rb.linearVelocity = newDirection * projectile.rb.linearVelocity.magnitude;
            }
            
            if (remainingRicochets[projectile] == 0 && becomesFragileAfterRicochet)
            {
                tower.ProjectileFragile = true;
            }
            
            projectile.EffectsToProcess = new Queue<ProjectileEffect>(
                new ProjectileEffect[] { this }.Concat(projectile.EffectsToProcess ?? new Queue<ProjectileEffect>())
            );
            
            return false;
        }
        
        return true;
    }
    
    private Enemy FindNextTarget(Vector3 position, Enemy excludeTarget)
    {
        var closestDistance = float.MaxValue;
        Enemy closestEnemy = null;
        
        foreach (var enemyEntry in EnemyManager.Enemies)
        {
            var enemy = enemyEntry.Value;
            if (enemy && enemy.gameObject.activeInHierarchy && enemy != excludeTarget)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance <= homingRadius && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        
        return closestEnemy;
    }
    
    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        return false;
    }
    
    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        var radians = degrees * Mathf.Deg2Rad;
        var cos = Mathf.Cos(radians);
        var sin = Mathf.Sin(radians);
        
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}