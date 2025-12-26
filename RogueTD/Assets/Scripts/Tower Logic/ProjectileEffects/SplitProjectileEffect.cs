using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitEffect", menuName = "Tower Defense/Effects/Split Effect")]
public class SplitEffect : ProjectileEffect
{
    [SerializeField] private int splitCount = 2;
    [SerializeField] private float splitAngle = 30f;
    [SerializeField] private float spawnOffset = 0.5f;

    public int SplitCount {get =>  splitCount; set { splitCount = value; } }

    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        if (splitCount <= 0) return false;
        
        var baseDirection = projectile.transform.right;
        
        var spawnPosition = projectile.transform.position;
        if (target)
        {
            var awayFromTarget = (spawnPosition - target.transform.position).normalized;
            spawnPosition += awayFromTarget * spawnOffset;
        }
        
        var angleStep = splitAngle / Mathf.Max(1, splitCount - 1);
        var startAngle = -splitAngle / 2f;
        
        for (var i = 0; i < splitCount; i++)
        {
            var currentAngle = startAngle + (i * angleStep);
            var direction = RotateVector(baseDirection, currentAngle);
            
            CreateSplitProjectile(spawnPosition, tower, direction, projectile);
        }
        
        return true;
    }
    
    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        return false;
    }
    
    private void CreateSplitProjectile(Vector3 spawnPosition, ProjectileTower tower, Vector2 direction, TowerProjectile originalProjectile)
    {
        var newProjectile = tower.CreateProjectile(spawnPosition);
        
        if (newProjectile)
        {
            var collider = newProjectile.GetComponent<Collider2D>();
            if (collider)
            {
                collider.enabled = false;
                MonoBehaviour projectileMono = newProjectile;
                projectileMono.StartCoroutine(EnableColliderAfterDelay(collider, 0.1f));
            }
            
            newProjectile.rb.linearVelocity = direction.normalized * tower.ProjectileSpeed;
            newProjectile.transform.right = direction;
            
            var effectsQueue = new Queue<ProjectileEffect>();
            if (originalProjectile.EffectsToProcess != null)
            {
                foreach (var effect in originalProjectile.EffectsToProcess)
                {
                    if (effect != null && !(effect is SplitEffect))
                    {
                        effectsQueue.Enqueue(effect);
                    }
                }
            }
            
            newProjectile.EffectsToProcess = effectsQueue;
        }
    }
    
    private System.Collections.IEnumerator EnableColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider)
        {
            collider.enabled = true;
        }
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