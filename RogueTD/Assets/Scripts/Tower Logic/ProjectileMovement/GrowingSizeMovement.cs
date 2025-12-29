using UnityEngine;

[CreateAssetMenu(fileName = "GrowingSizeMovement", menuName = "Tower Defense/Movement/Growing Size")]
public class GrowingSizeMovement : ProjectileBehavior
{
    [SerializeField] private float growthRate = 0.5f; 
    [SerializeField] private float maxSize = 3f; 
    [SerializeField] private float startSize = 0.5f; 
    
    public float GrowthRate { get => growthRate; set => growthRate = value; }
    public float MaxSize { get => maxSize; set => maxSize = value; }
    public float StartSize { get => startSize; set => startSize = value; }
    
    public override void Move(TowerProjectile projectile, ProjectileTower tower)
    {
        if (projectile.transform.localScale.x < maxSize)
        {
            var growth = growthRate * Time.deltaTime;
            var newSize = Mathf.Min(projectile.transform.localScale.x + growth, maxSize);
            projectile.transform.localScale = new Vector3(newSize, newSize, 1f);
        }
    }
}