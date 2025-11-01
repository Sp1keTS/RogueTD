using System.Collections;
using UnityEngine;

public class BasicTurret : ProjectileTower
{
    private Coroutine shootCoroutine;
    
    void Awake()
    {
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        float attackDelay = 1f / attackSpeed;
        
        while (true)
        {
            GetTarget();
            if (target != null)
            {
                TowerProjectile projectile = CreateProjectile();
                if (projectile != null)
                {
                    projectile.Initialize(
                        newDamage: Mathf.RoundToInt(damageMult), 
                        newVelocity: projectileSpeed,
                        newLifeTime: projectileLifetime,
                        newFragile: projectileFragile
                    );
                    
                    float randomAngle = Random.Range(-spread, spread);
                    Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * (target.transform.position - transform.position).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Rigidbody2D rb = projectile.rb;
                    rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
                }
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    void OnDestroy()
    {
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);
    }
    
    void OnDisable()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }
}