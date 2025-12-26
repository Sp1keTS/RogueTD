using System.Collections;
using UnityEngine;

public class BasicTurret : ProjectileTower
{
    private Coroutine shootCoroutine;
    
    public override void InitializeFromBlueprint(TowerBlueprint blueprint)
    {
        base.InitializeFromBlueprint(blueprint);
        
        StartShooting();
    }

    void StartShooting()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
        
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        Debug.Log("BasicTurret attackSpeed: " + attackSpeed);
        float attackDelay = 1f / attackSpeed;
        
        while (true)
        {
            GetTarget();
            if (target && currentAmmo >= 1)
            {
                var shotData = GetShotData(); 
                ExecuteShootChain();
                yield return new WaitForSeconds(attackDelay);
                currentAmmo -= 1;
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