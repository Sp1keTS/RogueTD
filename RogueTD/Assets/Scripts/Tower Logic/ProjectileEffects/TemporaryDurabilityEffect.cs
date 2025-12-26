using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryDurabilityEffect", menuName = "Tower Defense/Effects/Temporary Durability")]
public class TemporaryDurabilityEffect : ProjectileEffect
{
    [SerializeField] private int maxPenetrations = 3;
    [SerializeField] private float durabilityDuration = 2f;
    
    public override bool OnCollision(Enemy target, TowerProjectile projectile, ProjectileTower tower)
    {
        if (!projectile.GetComponent<ProjectilePenetrationData>())
        {
            var data = projectile.gameObject.AddComponent<ProjectilePenetrationData>();
            data.maxPenetrations = maxPenetrations;
            data.remainingPenetrations = maxPenetrations;
            data.expirationTime = Time.time + durabilityDuration;
        }
        
        var penetrationData = projectile.GetComponent<ProjectilePenetrationData>();
        
        if (Time.time > penetrationData.expirationTime)
        {
            tower.ProjectileFragile = true;
            return false;
        }
        
        target.TakeDamage(tower.Damage, tower);
        
        penetrationData.remainingPenetrations--;
        
        if (penetrationData.remainingPenetrations <= 0)
        {
            tower.ProjectileFragile = true;
            return false;
        }
        
        return true;
    }
    
    public override bool OnLifeSpanEnd(TowerProjectile projectile, ProjectileTower tower)
    {
        return false;
    }
}

public class ProjectilePenetrationData : MonoBehaviour
{
    public int maxPenetrations;
    public int remainingPenetrations;
    public float expirationTime;
}