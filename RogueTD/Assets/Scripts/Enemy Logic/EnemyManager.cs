using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<Enemy> Enemies { get; private set; }
    
    void Awake()
    {
        Enemies = new List<Enemy>();
    }
    
    public static void RemoveEnemy(Enemy enemy)
    {
        if (Enemies != null && enemy != null)
        {
            Enemies.Remove(enemy);
        }
    }
}