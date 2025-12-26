using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionEffect", menuName = "Tower Defense/Effects/Explosion Effect")]
public class ExplosionEffect : ProjectileEffect
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damagePercentage = 50;
    [SerializeField] private GameObject explosionPrefab;
    
    public float ExplosionRadius
    {
        get => explosionRadius;
        set => explosionRadius = value;
    }

    public int DamagePercentage
    {
        get => damagePercentage;
        set => damagePercentage = value;
    }
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        CreateExplosion(projectile.transform.position, tower);
        return false;
    }

    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        CreateExplosion(projectile.transform.position, tower);
        return false;
    }
    
    private void CreateExplosion(Vector3 position, ProjectileTower tower)
    {
        var explosionDamage = Mathf.RoundToInt(tower.Damage * (damagePercentage / 100f));
        
        if (explosionPrefab)
        {
            GameObject explosion = Object.Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * explosionRadius;
        }
        
        ExplosionGenerator.CreateExplosion(position, explosionRadius, explosionDamage, tower);
    }
}