using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowStatusEffect", menuName = "Tower Defense/Effects/Slow Status Effect")]
public class SlowStatusEffect : StatusEffect
{
    [SerializeField] private float slowPercent = 0.5f;
    
    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        if (!enemy) yield break;
        float originalSpeed = enemy.MoveSpeed;
        enemy.MoveSpeed = originalSpeed * slowPercent;
        yield return new WaitForSeconds(duration);
        if (enemy)
        {
            enemy.MoveSpeed = originalSpeed;
        }
    }
}