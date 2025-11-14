using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BurstShotBehavior", menuName = "Tower Defense/ShotBehavior/Burst Shot Behavior")]
public class BurstShotBehavior : SecondaryProjectileTowerBehavior
{
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstDelay = 0.1f;
    
    public override void Shoot(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior)
    {
        tower.StartCoroutine(BurstCoroutine(tower, shotData, nextBehavior));
    }
    
    private IEnumerator BurstCoroutine(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior)
    {
        for (int i = 0; i < burstCount; i++)
        {
            nextBehavior?.Invoke(shotData);
            yield return new WaitForSeconds(burstDelay);
        }
    }
}