using UnityEngine;

public class StraightMovement : ProjectileBehavior
{
    public override void Move(TowerProjectile projectile, ProjectileTower tower)
    {
        projectile.rb.linearVelocity = projectile.transform.right * tower.ProjectileSpeed;
    }
}
