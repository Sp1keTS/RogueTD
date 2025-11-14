using System;
using UnityEngine;

public abstract class SecondaryProjectileTowerBehavior : ScriptableObject
{
    public abstract void Shoot(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior = null);
}