using UnityEngine;

[CreateAssetMenu(fileName = "nemyStraightMovementBehaviour", menuName = "Enemies/Movement/Straight")]
public class EnemyStraightMovementBehaviour : EnemyMovementBehavior
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    
    public override void Move(EnemyModel enemy, Rigidbody2D rb, Vector2 currentPosition, float deltaTime)
    {
        if (!enemy.currentTarget || rb == null) 
        {
            Stop(rb);
            return;
        }
        
        Vector2 targetPos = enemy.currentTarget.transform.position;
        Vector2 direction = (targetPos - currentPosition).normalized;
        
        rb.AddForce(direction * acceleration * deltaTime, ForceMode2D.Force);
        
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetRotation.eulerAngles.z, rotationSpeed * deltaTime);
        }
    }
}