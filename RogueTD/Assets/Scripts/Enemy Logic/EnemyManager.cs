using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    private static Dictionary<string, Enemy> _enemies;
    public static Dictionary<string,Enemy> Enemies { get => _enemies;  set => _enemies = value; }
    static EnemyManager()
    {
        _enemies = new Dictionary<string, Enemy>();
    }

}