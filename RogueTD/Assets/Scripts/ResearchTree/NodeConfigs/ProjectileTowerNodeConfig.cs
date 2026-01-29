
using UnityEngine;

public abstract class ProjectileTowerNodeConfig : TowerNodeConfig
{
    
    [SerializeField] private GameObject projectileTower;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float spread ;
    [SerializeField] protected float projectileLifetime;
    [SerializeField] protected int penetrationCount;
    [SerializeField] protected float projectileScale;
    [SerializeField] protected int projectileCount;
    public override Tower Tower => projectileTower.GetComponent<ProjectileTower>();
    public TowerProjectile ProjectilePrefab => projectilePrefab.GetComponent<TowerProjectile>();
    public float ProjectileSpeed => projectileSpeed;
    public float Spread => spread;
    public float ProjectileLifetime => projectileLifetime;
    public int PenetrationCount => penetrationCount;
    public float ProjectileScale => projectileScale;
    public int ProjectileCount => projectileCount;
}
