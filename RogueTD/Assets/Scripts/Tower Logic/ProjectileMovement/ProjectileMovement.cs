using UnityEngine;

public abstract class ProjectileMovement : MonoBehaviour
{
    public Enemy target;
    public Vector2 targetPoint;
    public abstract void Move(Rigidbody2D rb, float Velocity);
}
