using System.Collections;
using UnityEngine;

public class SlowStatusEffect : StatusEffect
{
    [SerializeField] private float slowPercent = 0.5f;
    public float SlowPercent
    {
        get => slowPercent; set => slowPercent = value;
    }

    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        if (!enemy) yield break;
        var originalSpeed = enemy.MoveSpeed;
        enemy.MoveSpeed = originalSpeed * slowPercent;
        yield return new WaitForSeconds(Duration);
        if (enemy)
        {
            enemy.MoveSpeed = originalSpeed;
        }
    }
}