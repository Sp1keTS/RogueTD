using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStraightMovementBehaviour", menuName = "Enemies/Movement/Straight")]
public class EnemyStraightMovementBehaviour : EnemyMovementBehavior
{
    
    public override void Move(Enemy enemy, Rigidbody2D rb, Vector2 currentPosition, float deltaTime)
    {
        if (!enemy || !rb) 
        {
            Stop(rb);
            return;
        }
        
        var currentTarget = enemy.GetCurrentTarget();
        if (!currentTarget) 
        {
            Stop(rb);
            return;
        }
        
        Vector2 targetPos = currentTarget.transform.position;
        Vector2 direction = (targetPos - currentPosition).normalized;
        
        rb.AddForce(direction * (enemy.MoveSpeed * deltaTime), ForceMode2D.Force);
        if (rb.linearVelocity.magnitude > enemy.MoveSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemy.MoveSpeed;
        }
        
        // Устанавливаем вращение на 0, чтобы текстура всегда была вертикальной
        rb.rotation = 0f;
    }
}