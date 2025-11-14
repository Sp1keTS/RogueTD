using UnityEngine;

[CreateAssetMenu(fileName = "StraightMovement", menuName = "Tower Defense/ProjectileMovement/Straight Movement")]
public class StraightMovement : ProjectileBehavior
{
    public override void Move(TowerProjectile projectile, ProjectileTower tower)
    {
        projectile.rb.linearVelocity = projectile.transform.right * tower.ProjectileSpeed;
    }
}
