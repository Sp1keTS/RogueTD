using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTargetingBehavior : ScriptableObject
{
    public abstract Building SelectTarget(EnemyModel enemy);
}

