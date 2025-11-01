using UnityEngine;

public class StraightMovement : ProjectileMovement
{
    public override void Move(Rigidbody2D rb, float Velocity)
    {
        rb.linearVelocity = transform.right * Velocity;
    }
}
