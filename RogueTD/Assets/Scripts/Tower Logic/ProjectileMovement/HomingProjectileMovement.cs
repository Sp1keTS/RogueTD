using UnityEngine;

[CreateAssetMenu(fileName = "HomingMovement", menuName = "Tower Defense/Movement/Homing Movement")]
public class HomingMovement : ProjectileBehavior
{
    [SerializeField] private float homingRadius = 1f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool findNearestTarget = true;
    
    public float HomingRadius {get => homingRadius; set { homingRadius = value; } }
    public float RotationSpeed {get => rotationSpeed; set { rotationSpeed = value; } }
    
    public override void Move(TowerProjectile projectile, ProjectileTower tower)
    {
        Enemy findTarget = FindTarget(projectile.transform.position);
        
        if (findTarget)
        {
            var directionToTarget = (findTarget.transform.position - projectile.transform.position).normalized;
            var rotateAmount = Vector3.Cross(directionToTarget, projectile.transform.right).z;
            projectile.rb.angularVelocity = -rotateAmount * rotationSpeed * 100f;
            projectile.rb.linearVelocity = projectile.transform.right * projectile.rb.linearVelocity.magnitude;
        }
    }
    
    private Enemy FindTarget(Vector3 position)
    {
        var colliders = Physics2D.OverlapCircleAll(position, homingRadius);
        
        Enemy bestTarget = null;
        var bestDistance = float.MaxValue;
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                var enemy = EnemyManager.Enemies[collider.name];
                if (enemy)
                {
                    var distance = Vector3.Distance(position, enemy.transform.position);
                    
                    if (findNearestTarget)
                    {
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestTarget = enemy;
                        }
                    }
                    else
                    {
                        return enemy;
                    }
                }
            }
        }
        
        return bestTarget;
    }
}