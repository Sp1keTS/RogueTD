using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveGenerator : MonoBehaviour
{
    [SerializeField] private List<Enemy> availableEnemyPrefabs;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private float standardWaveBudget = 30;
    
    
    private EnemyWave currentWave;

    public EnemyWave GenerateWave(int waveNum)
    {
        currentWave = new EnemyWave();
        currentWave.SpawnPoints = GenerateSpawnpoints();
        
        var subWaveCount = Random.Range(2, 5);
        var budget = Math.Pow(waveNum, 1.2) * standardWaveBudget;
        var budgetPerWave = budget / subWaveCount;
        
        currentWave.SubWaves = new List<EnemyWave.SubWave>();
        
        for (int i = 0; i < subWaveCount; i++)
        {
            var subWave = GenerateSubWave(waveNum, budgetPerWave, currentWave.SpawnPoints);
            currentWave.SubWaves.Add(subWave);
        }
        
        return currentWave;
    }

    private EnemyWave.SubWave GenerateSubWave(int waveNum, double budget, List<Vector2> spawnPoints)
    {
        var subWave = new EnemyWave.SubWave
        {
            enemiesPerSpawnpoint = new Dictionary<Vector2, List<(Enemy prefab, int count)>>()
        };
        
        foreach (var spawnPoint in spawnPoints)
        {
            subWave.enemiesPerSpawnpoint[spawnPoint] = new List<(Enemy prefab, int count)>();
        }
        
        double remainingBudget = budget;
        int clusterSize = Random.Range(2, 32);
        
        var eligibleEnemies = FilterEnemiesByWaveRank(waveNum);
        if (eligibleEnemies.Count == 0) return subWave;
        
        while (remainingBudget > 0)
        {
            var selectedEnemyPrefab = SelectEnemyPrefabWithWeight(waveNum, eligibleEnemies);
            if (!selectedEnemyPrefab) break;
            
            var enemyData = selectedEnemyPrefab.GetComponent<Enemy>();
            if (!enemyData) break;
            
            int maxEnemiesByBudget = Mathf.FloorToInt((float)remainingBudget / enemyData.Cost);
            if (maxEnemiesByBudget <= 0) break;
            
            int actualClusterSize = Math.Min(clusterSize, maxEnemiesByBudget);
            if (actualClusterSize <= 0) break;
            
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            
            int enemiesToCreate = Random.Range(1, actualClusterSize + 1);
            enemiesToCreate = Math.Min(enemiesToCreate, maxEnemiesByBudget);
            
            if (enemiesToCreate > 0)
            {
                var enemyList = subWave.enemiesPerSpawnpoint[spawnPoint];
                bool found = false;
                
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i].prefab == selectedEnemyPrefab)
                    {
                        enemyList[i] = (enemyList[i].prefab, enemyList[i].count + enemiesToCreate);
                        found = true;
                        break;
                    }
                }
                
                if (!found)
                {
                    enemyList.Add((selectedEnemyPrefab, enemiesToCreate));
                }
                
                remainingBudget -= enemiesToCreate * enemyData.Cost;
            }
            
            if (Random.value < 0.3f)
            {
                clusterSize = Random.Range(2, 32);
            }
            
            if (remainingBudget < GetMinEnemyCost(eligibleEnemies))
                break;
        }
        
        return subWave;
    }

    private List<Enemy> FilterEnemiesByWaveRank(int waveNum)
    {
        var eligibleEnemies = new List<Enemy>();
        
        foreach (var enemyPrefab in availableEnemyPrefabs)
        {
            if (enemyPrefab && enemyPrefab.Rank <= waveNum)
            {
                eligibleEnemies.Add(enemyPrefab);
            }
        }
        
        return eligibleEnemies;
    }

    private Enemy SelectEnemyPrefabWithWeight(int waveNum, List<Enemy> eligibleEnemies)
    {
        if (eligibleEnemies.Count == 0) return null;
        
        var weightedEnemies = new List<(Enemy enemyPrefab, float weight)>();
        float totalWeight = 0f;
        
        foreach (var enemyPrefab in eligibleEnemies)
        {
            float weight = CalculateEnemyWeight(waveNum, enemyPrefab);
            weightedEnemies.Add((enemyPrefab, weight));
            totalWeight += weight;
        }
        
        if (totalWeight <= 0) return eligibleEnemies[Random.Range(0, eligibleEnemies.Count)];
        
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        
        foreach (var weightedEnemy in weightedEnemies)
        {
            cumulativeWeight += weightedEnemy.weight;
            if (randomValue <= cumulativeWeight)
            {
                return weightedEnemy.enemyPrefab;
            }
        }
        
        return weightedEnemies[0].enemyPrefab;
    }

    private float CalculateEnemyWeight(int waveNum, Enemy enemyPrefab)
    {
        float weight = 1f;
        
        int rankDifference = waveNum - enemyPrefab.Rank + 1;
        
        if (rankDifference > 0)
        {
            weight /= Mathf.Pow(2f, rankDifference - 1);
        }
        else if (rankDifference < 0)
        {
            weight /= Mathf.Pow(4f, Mathf.Abs(rankDifference));
        }
        else
        {
            weight *= 1.5f;
        }
        
        weight *= Mathf.Lerp(1.2f, 0.8f, Mathf.InverseLerp(5, 50, enemyPrefab.Cost));
        
        return Mathf.Max(0.1f, weight);
    }
    
    private int GetMinEnemyCost(List<Enemy> enemies)
    {
        int minCost = int.MaxValue;
        foreach (var enemyPrefab in enemies)
        {
            if (enemyPrefab.Cost < minCost)
                minCost = enemyPrefab.Cost;
        }
        return minCost == int.MaxValue ? 1 : minCost;
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
            0 => new Vector2(Random.Range(-size.x, size.x + 1), size.y),
            1 => new Vector2(size.x, Random.Range(-size.y, size.y + 1)),
            2 => new Vector2(Random.Range(-size.x, size.x + 1), -size.y),
            3 => new Vector2(-size.x, Random.Range(-size.y, size.y + 1)),
            _ => new Vector2(Random.Range(-size.x, size.x + 1), size.y)
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