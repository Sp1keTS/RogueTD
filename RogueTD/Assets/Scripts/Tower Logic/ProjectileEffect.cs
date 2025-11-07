using UnityEngine;
using System.Collections.Generic;

public abstract class ProjectileEffect
{

    public abstract void OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower);

    public abstract void OnLifeSpanEnd( TowerProjectile projectile, ProjectileTower tower);

    public abstract void OnUpdate(TowerProjectile projectile, ProjectileTower tower);
}