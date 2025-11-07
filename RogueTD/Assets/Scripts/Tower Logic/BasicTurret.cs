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
            if (target != null && currentAmmo >= 1)
            {
                var shotData = GetShotData(); 
                towerBehavior?.Shoot(this, shotData);
                yield return new WaitForSeconds(attackDelay);
                currentAmmo -= 1;
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    private void Update()
    {
        RestoreAmmo();
        RotateTowardsTarget();
    }

    private void RestoreAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo += Time.deltaTime * ammoRegeneration;   
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