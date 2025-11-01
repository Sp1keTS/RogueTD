using UnityEngine;

public abstract class ProjectileProfile
{
    public abstract void OnCollisionEffect(GameObject target = null);
    public abstract void OnLifeSpan();
    public abstract void onUpdate();
}
