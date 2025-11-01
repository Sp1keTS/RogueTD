using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [SerializeField] protected float targetingRange = 5f;
    [SerializeField] protected float damageMult = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] protected float projectileLifetime = 3f;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected GameObject projectilePrefab;
    
    protected Enemy target;

    protected void GetTarget()
    {
        var enemies = EnemyManager.Enemies;
        if (enemies == null || enemies.Count == 0)
        {
            target = null;
            return;
        }
        
        float lowestDistance = Mathf.Infinity;
        target = null;
        Vector3 myPosition = transform.position;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            
            float distance = (enemy.transform.position - myPosition).sqrMagnitude;
            float rangeSqr = targetingRange * targetingRange;
            
            if (distance <= rangeSqr && distance < lowestDistance)
            {
                lowestDistance = distance;
                target = enemy;
            }
        }
    }

    protected TowerProjectile CreateProjectile()
    {
        if (projectilePrefab == null) return null;
        
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        return newProjectile.GetComponent<TowerProjectile>();
    }
}