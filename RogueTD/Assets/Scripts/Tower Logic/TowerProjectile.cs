using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float velocity = 10;
    [SerializeField] float lifeTime = 1f;
    [SerializeField] bool fragile = true;

    [SerializeField] public Rigidbody2D rb;

    //то какие эффекты есть у пули при прикосновении и уничтожении по времени
    [SerializeField] ProjectileProfile profile;    
    //кастомный файл движения пули, задавать через файл башни
    public ProjectileMovement Movement {get; set; } 

    public void Initialize(int newDamage, float newVelocity, 
        float newLifeTime = 10f, bool newFragile = true)
    {
        this.damage = newDamage;
        this.velocity = newVelocity;
        this.lifeTime = newLifeTime;
        this.fragile = newFragile;
    }
    
    void Update()
    {
        Movement?.Move(rb,velocity);
        lifeTime -= Time.deltaTime;                        
        if (lifeTime <= 0)                                 
        {                                                  
            profile?.OnLifeSpan();
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject;
        var enemyBase = enemy.GetComponent<Enemy>();
        if (enemy.CompareTag("Enemy"))
        {
            if (enemyBase != null)
            {
                profile?.OnCollisionEffect(enemy);
                enemyBase.TakeDamage(damage);
                if(fragile){Destroy(gameObject);}
            }
        }
    }
}
    
