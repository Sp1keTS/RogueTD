using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitEffect", menuName = "Tower Defense/Effects/Split Effect")]
public class SplitEffect : ProjectileEffect
{
    [SerializeField] private int splitCount = 2;
    [SerializeField] private float splitAngle = 30f;
    [SerializeField] private float spawnOffset = 0.5f;
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        if (splitCount <= 0) return false;
        
        Vector2 baseDirection = projectile.transform.right;
        
        Vector3 spawnPosition = projectile.transform.position;
        if (target)
        {
            Vector2 awayFromTarget = (spawnPosition - target.transform.position).normalized;
            spawnPosition += (Vector3)awayFromTarget * spawnOffset;
        }
        
        float angleStep = splitAngle / Mathf.Max(1, splitCount - 1);
        float startAngle = -splitAngle / 2f;
        
        for (int i = 0; i < splitCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Vector2 direction = RotateVector(baseDirection, currentAngle);
            
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
        TowerProjectile newProjectile = tower.CreateProjectile(spawnPosition);
        
        if (newProjectile)
        {
            Collider2D collider = newProjectile.GetComponent<Collider2D>();
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
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}