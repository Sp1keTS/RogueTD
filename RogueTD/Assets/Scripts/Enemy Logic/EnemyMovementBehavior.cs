using UnityEngine;

public abstract class EnemyMovementBehavior : ScriptableObject
{
    public abstract void Move(EnemyModel enemy, Rigidbody2D rb, Vector2 currentPosition, float deltaTime);
    public virtual void Stop(Rigidbody2D rb) 
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}