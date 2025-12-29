using UnityEngine;

public abstract class ProjectileBehavior : Resource
{
    public Enemy target;
    public Vector2 targetPoint;
    public abstract void Move(TowerProjectile projectile, ProjectileTower tower);
}
