using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Random = UnityEngine.Random;

public class EnemyWaveGenerator : MonoBehaviour
{
    [SerializeField] private List<string> enemyIDs;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private float standardWaveBudget = 30;
    [SerializeField] private EnemyRegistry enemyRegistry;
    
    private EnemyWave currentWave;

    private void Start()
    {
        enemyIDs = enemyRegistry.EnemyPrefabs.Keys.ToList();
    }

    public EnemyWave GenerateWave(int waveNum)
    {
        currentWave = new EnemyWave
        {
            SpawnPoints = GenerateSpawnpoints(),
            SubWaves = new List<EnemyWave.SubWave>()
        };
        
        var subWaveCount = Random.Range(2, 5);
        var budget = Math.Pow(waveNum, 1.2) * standardWaveBudget;
        var budgetPerWave = budget / subWaveCount;
        
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
            enemiesPerSpawnpoint = new SerializedDictionary<Vector2, SerializedDictionary<string, int>>()
        };
        
        foreach (var spawnPoint in spawnPoints)
        {
            subWave.enemiesPerSpawnpoint[spawnPoint] = new SerializedDictionary<string, int>();
        }
        
        var remainingBudget = budget;
        var clusterSize = Random.Range(2, 32);
        
        var eligibleEnemyIDs = FilterEnemiesByWaveRank(waveNum);
        if (eligibleEnemyIDs.Count == 0) return subWave;
        
        while (remainingBudget > 0)
        {
            var selectedEnemyID = SelectEnemyWithWeight(waveNum, eligibleEnemyIDs);
            if (string.IsNullOrEmpty(selectedEnemyID)) break;
            
            var enemyPrefab = enemyRegistry.GetEnemyPrefab(selectedEnemyID);
            if (!enemyPrefab) break;
            
            int maxEnemiesByBudget = Mathf.FloorToInt((float)remainingBudget / enemyPrefab.Cost);
            if (maxEnemiesByBudget <= 0) break;
            
            int actualClusterSize = Math.Min(clusterSize, maxEnemiesByBudget);
            if (actualClusterSize <= 0) break;
            
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            
            int enemiesToCreate = Random.Range(1, actualClusterSize + 1);
            enemiesToCreate = Math.Min(enemiesToCreate, maxEnemiesByBudget);
            
            if (enemiesToCreate > 0)
            {
                var enemyDict = subWave.enemiesPerSpawnpoint[spawnPoint];
                
                if (enemyDict.ContainsKey(selectedEnemyID))
                {
                    enemyDict[selectedEnemyID] += enemiesToCreate;
                }
                else
                {
                    enemyDict[selectedEnemyID] = enemiesToCreate;
                }
                
                remainingBudget -= enemiesToCreate * enemyPrefab.Cost;
            }
            
            if (Random.value < 0.3f)
            {
                clusterSize = Random.Range(2, 32);
            }
            
            if (remainingBudget < GetMinEnemyCost(eligibleEnemyIDs))
                break;
        }
        
        return subWave;
    }

    private List<string> FilterEnemiesByWaveRank(int waveNum)
    {
        var eligibleEnemies = new List<string>();
        
        foreach (var enemyID in enemyIDs)
        {
            if (!string.IsNullOrEmpty(enemyID))
            {
                var enemyPrefab = enemyRegistry.GetEnemyPrefab(enemyID);
                if (enemyPrefab != null && enemyPrefab.Rank <= waveNum)
                {
                    eligibleEnemies.Add(enemyID);
                }
            }
        }
        
        return eligibleEnemies;
    }

    private string SelectEnemyWithWeight(int waveNum, List<string> eligibleEnemyIDs)
    {
        if (eligibleEnemyIDs.Count == 0) return null;
        
        var weightedEnemies = new List<(string enemyID, float weight)>();
        var totalWeight = 0f;
        
        foreach (var enemyID in eligibleEnemyIDs)
        {
            var enemyPrefab = enemyRegistry.GetEnemyPrefab(enemyID);
            if (enemyPrefab != null)
            {
                var weight = CalculateEnemyWeight(waveNum, enemyPrefab);
                weightedEnemies.Add((enemyID, weight));
                totalWeight += weight;
            }
        }
        
        if (weightedEnemies.Count == 0) return null;
        if (totalWeight <= 0) return weightedEnemies[Random.Range(0, weightedEnemies.Count)].enemyID;
        
        var randomValue = Random.Range(0f, totalWeight);
        var cumulativeWeight = 0f;
        
        foreach (var weightedEnemy in weightedEnemies)
        {
            cumulativeWeight += weightedEnemy.weight;
            if (randomValue <= cumulativeWeight)
            {
                return weightedEnemy.enemyID;
            }
        }
        
        return weightedEnemies[0].enemyID;
    }

    private float CalculateEnemyWeight(int waveNum, Enemy enemyPrefab)
    {
        var weight = 1f;
        
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
    
    private int GetMinEnemyCost(List<string> enemyIDs)
    {
        var minCost = int.MaxValue;
        foreach (var enemyID in enemyIDs)
        {
            var enemyPrefab = enemyRegistry.GetEnemyPrefab(enemyID);
            if (enemyPrefab != null && enemyPrefab.Cost < minCost)
            {
                minCost = enemyPrefab.Cost;
            }
        }
        return minCost == int.MaxValue ? 1 : minCost;
    }

    public List<Vector2> GenerateSpawnpoints()
    {
        var spawnpoints = new List<Vector2>();
        var size = mapManager.Size;
        
        var numberOfPoints = Random.Range(1, 5);
        
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
            var sqrDistance = (newPoint - existingPoint).sqrMagnitude;
            var minSqrDistance = minDistance * minDistance;
            
            if (sqrDistance < minSqrDistance)
            {
                return false; 
            }
        }
        
        return true; 
    }
}