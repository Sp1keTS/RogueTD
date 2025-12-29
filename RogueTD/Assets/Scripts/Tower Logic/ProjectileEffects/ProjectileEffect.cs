using UnityEngine;
using System.Collections.Generic;

public abstract class ProjectileEffect : Resource
{

    public abstract bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower); //True значит что нужно прекратить цепочку вызовов

    public abstract bool OnLifeSpanEnd( TowerProjectile projectile, ProjectileTower tower);

    // public abstract void OnUpdate(TowerProjectile projectile, ProjectileTower tower);
}