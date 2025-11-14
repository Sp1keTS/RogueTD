using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionEffect", menuName = "Tower Defense/Effects/Explosion Effect")]
public class ExplosionEffect : ProjectileEffect
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionEffectPrefab;
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        Explode(projectile, tower, target);

        return false;
    }

    private void Explode(TowerProjectile projectile, ProjectileTower tower, Enemy target = null)
    {
        // визуальный эффект взрыва
        if (explosionEffectPrefab)
        {
            explosionEffectPrefab.transform.localScale = new Vector3(explosionRadius, explosionRadius, 0);
            Instantiate(explosionEffectPrefab, projectile.transform.position, Quaternion.identity);
        }
        
        // Находим всех врагов в радиусе взрыва
        Collider2D[] colliders = Physics2D.OverlapCircleAll(projectile.transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy && enemy != target)
            {
                enemy.TakeDamage(tower.Damage/2, tower.statusEffects);
            }
        }
    }

    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        Explode(projectile, tower);
        return false; 
    }
}