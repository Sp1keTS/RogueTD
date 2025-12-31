using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] ProjectileTower projectileTower;
    [SerializeField] SpriteRenderer spriteRenderer;
    private Queue<ProjectileEffect> effectsToProcess;
    private float lifeTime;
    private GameObject enemy;
    private Enemy _enemyBase;

    public Queue<ProjectileEffect> EffectsToProcess  {get => effectsToProcess; set => effectsToProcess = value;}
    public void Initialize(ProjectileTower tower)
    {
        transform.localScale = Vector3.one * tower.ProjectileScale;
        projectileTower = tower;
        lifeTime = tower.ProjectileLifetime;
        effectsToProcess = new Queue<ProjectileEffect>();
        if (tower.effects != null)
        {
            foreach (var effect in tower.effects)
            {
                if (effect != null)
                    effectsToProcess.Enqueue(effect);
            }
        }
    }
    
    void Update()
    {
        foreach (ProjectileBehavior movement in projectileTower.movements)
        {
            movement.Move(this, projectileTower);
        }
        
        lifeTime -= Time.deltaTime;                        
        if (lifeTime <= 0)                                 
        {
            while (effectsToProcess.Count > 0)
            {
                if (effectsToProcess.Dequeue().OnLifeSpanEnd(this, projectileTower))
                {
                    break;
                }
            }
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.gameObject;
        _enemyBase = enemy.GetComponent<Enemy>();
        if (enemy.CompareTag("Enemy"))
        {
            if (_enemyBase)
            {
                while (effectsToProcess.Count > 0)
                {
                    if (effectsToProcess.Dequeue().OnCollision(_enemyBase,this, projectileTower))
                    {
                        break;
                    }
                }
                _enemyBase.TakeDamage(projectileTower.Damage, projectileTower);
                if(projectileTower.ProjectileFragile)
                {
                    Destroy(gameObject);
                    
                }
            }
        }
    }
}