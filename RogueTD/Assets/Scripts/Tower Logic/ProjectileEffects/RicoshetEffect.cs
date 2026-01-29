using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "RicochetEffect", menuName = "Tower Defense/Effects/Ricochet Effect")]
public class RicochetEffect : ProjectileEffect
{
    [SerializeField] private int maxRicochets = 1;
    [SerializeField] private float angleChange = 15f;
    [SerializeField] private bool becomesFragileAfterRicochet = true;
    
    public int MaxRicochets {get => maxRicochets; set => maxRicochets = value; }
    public float AngleChange {get => angleChange; set => angleChange = value; }
    
    private Dictionary<TowerProjectile, int> remainingRicochets = new Dictionary<TowerProjectile, int>();
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        if (!remainingRicochets.ContainsKey(projectile))
        {
            remainingRicochets[projectile] = maxRicochets;
        }
        
        target.TakeDamage(tower.Damage, tower);
        
        if (remainingRicochets[projectile] > 0)
        {
            remainingRicochets[projectile]--;
            
            var currentDirection = projectile.transform.right;
            var randomAngle = Random.Range(-angleChange, angleChange);
            var newDirection = RotateVector(currentDirection, randomAngle);
            
            projectile.transform.right = newDirection;
            projectile.rb.linearVelocity = newDirection * projectile.rb.linearVelocity.magnitude;
            
            if (remainingRicochets[projectile] == 0 && becomesFragileAfterRicochet)
            {
            }
            
            projectile.EffectsToProcess = new Queue<ProjectileEffect>(
                new ProjectileEffect[] { this }.Concat(projectile.EffectsToProcess ?? new Queue<ProjectileEffect>())
            );
            
            return true;
        }
        
        return false;
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