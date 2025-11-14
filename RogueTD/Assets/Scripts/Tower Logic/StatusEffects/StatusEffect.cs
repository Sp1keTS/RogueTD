using System.Collections;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public float duration = 2;

    public abstract IEnumerator ApplyEffect(Enemy enemy);
}