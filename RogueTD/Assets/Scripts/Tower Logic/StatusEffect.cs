using System.Collections;

public abstract class StatusEffect
{
    public string name;
    public float duration;

    public abstract IEnumerator ApplyEffect(Enemy enemy);
}