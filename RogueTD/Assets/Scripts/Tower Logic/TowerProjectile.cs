using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] ProjectileTower projectileTower;

    
    private float lifeTime;
    private GameObject enemy;
    private Enemy enemyBase;

    public void Initialize(ProjectileTower tower)
    {
        Debug.Log(tower);
        projectileTower = tower;
        lifeTime = tower.ProjectileLifetime;
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
            Debug.Log(projectileTower);
            foreach (ProjectileEffect effect in projectileTower.effects)
            {
                effect.OnLifeSpanEnd(this, projectileTower);
            }
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.gameObject;
        enemyBase = enemy.GetComponent<Enemy>();
        if (enemy.CompareTag("Enemy"))
        {
            if (enemyBase != null)
            {
                foreach (ProjectileEffect effect in projectileTower.effects)
                {
                    effect.OnCollision(enemyBase,this, projectileTower);
                }
                enemyBase.TakeDamage(projectileTower);
                if(projectileTower.ProjectileFragile)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}