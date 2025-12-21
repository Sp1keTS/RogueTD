using UnityEngine;

public abstract class ProjectileBehavior : ScriptableObject
{
    public Enemy target;
    public Vector2 targetPoint;
    public abstract void Move(TowerProjectile projectile, ProjectileTower tower);
}
