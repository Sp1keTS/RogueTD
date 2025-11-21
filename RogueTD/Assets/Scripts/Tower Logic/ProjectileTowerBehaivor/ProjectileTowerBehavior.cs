
using System;
using UnityEngine;

public abstract class ProjectileTowerBehavior : Resource
{
    public abstract void Shoot(ProjectileTower projectileTower,ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior = null);
}