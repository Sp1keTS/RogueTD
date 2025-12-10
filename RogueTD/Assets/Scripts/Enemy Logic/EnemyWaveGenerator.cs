using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveGenerator : MonoBehaviour
{
    [SerializedDictionary] private Dictionary<string, Enemy> availableEnemies;
    [SerializeField] MapManager mapManager;
    [SerializeField] private float standartWaveBudget = 30;
    private EnemyWave currentWave;

    public EnemyWave GenerateWave(int waveNum)
    {
        currentWave = new EnemyWave();
        currentWave.SpawnPoints = GenerateSpawnpoints();
        var subWaveCount = Random.Range(2, 5);
        var budget = Math.Pow(waveNum, 1.2) * standartWaveBudget;
    }

    public List<Vector2> GenerateSpawnpoints()
    {
        var spawnpoints = new List<Vector2>();
        var size = mapManager.Size;
        
        int numberOfPoints = Random.Range(1, 5);
        
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector2 spawnpoint;
            int attempts = 0;
            const int maxAttempts = 50;
            bool validPointFound = false;
            
            do
            {
                int side = Random.Range(0, 4);
                
                spawnpoint = GeneratePointOnSide(side, size);
                
                if (IsPointValid(spawnpoint, spawnpoints, 10f))
                {
                    validPointFound = true;
                }
                
                attempts++;
                
            } while (!validPointFound && attempts < maxAttempts);
            
            if (!validPointFound)
            {
                int side = Random.Range(0, 4);
                spawnpoint = GeneratePointOnSide(side, size);
            }
            
            spawnpoints.Add(spawnpoint);
        }
        
        return spawnpoints;
    }

    private Vector2 GeneratePointOnSide(int side, Vector2Int size)
    {
        return side switch
        {
            0 => new Vector2(Random.Range(-size.x, size.x + 1), size.y),    // Верх
            1 => new Vector2(size.x, Random.Range(-size.y, size.y + 1)),    // Право
            2 => new Vector2(Random.Range(-size.x, size.x + 1), -size.y),   // Низ
            3 => new Vector2(-size.x, Random.Range(-size.y, size.y + 1)),   // Лево
        };
    }

    private bool IsPointValid(Vector2 newPoint, List<Vector2> existingPoints, float minDistance)
    {
        if (existingPoints.Count == 0)
            return true;
        
        foreach (Vector2 existingPoint in existingPoints)
        {
            float sqrDistance = (newPoint - existingPoint).sqrMagnitude;
            float minSqrDistance = minDistance * minDistance;
            
            if (sqrDistance < minSqrDistance)
            {
                return false; 
            }
        }
        
        return true; 
    }
}
