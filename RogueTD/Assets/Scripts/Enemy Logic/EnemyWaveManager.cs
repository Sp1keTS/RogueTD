using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private Button waveButton;
    [SerializeField] private EnemyWaveGenerator waveGenerator;
    [SerializeField] private GameState gameState;
    [SerializeField] private float timeBetweenSubWaves = 5f;
    
    private bool isWaveActive = false;
    private int currentWaveNumber = 0;
    private EnemyWave currentWave;
    private int currentSubWaveIndex = 0;
    private Coroutine waveCoroutine;
    
    private Dictionary<Vector2, Queue<(Enemy prefab, int count)>> spawnQueues = 
        new Dictionary<Vector2, Queue<(Enemy prefab, int count)>>();
    
    void Start()
    {
        currentWaveNumber = gameState.Wave;
        
        if (waveButton)
        {
            waveButton.onClick.AddListener(StartWave);
        }
    }
    
    void Update()
    {
        if (isWaveActive)
        {
            CheckWaveCompletion();
        }
    }
    
    public void StartWave()
    {
        if (isWaveActive) return;
        if (gameState.CurrentWave != null)
        {
            currentWave = gameState.CurrentWave;
        }
        else {currentWave = waveGenerator.GenerateWave(currentWaveNumber);}
        
        
        if (currentWave == null || currentWave.SubWaves == null || currentWave.SubWaves.Count == 0)
        {
            return;
        }
        
        isWaveActive = true;
        currentSubWaveIndex = 0;
        
        InitializeSpawnQueues();
        
        EnemyManager.OnEnemyDied += OnEnemyDied;
        
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }
        waveCoroutine = StartCoroutine(WaveSequence());
    }
    
    private IEnumerator WaveSequence()
    {
        SpawnSubWave(currentSubWaveIndex);
        currentSubWaveIndex++;
        
        while (currentSubWaveIndex < currentWave.SubWaves.Count)
        {
            yield return new WaitForSeconds(timeBetweenSubWaves);
            
            SpawnSubWave(currentSubWaveIndex);
            currentSubWaveIndex++;
        }
    }
    
    private void SpawnSubWave(int subWaveIndex)
    {
        if (currentWave == null || 
            currentWave.SubWaves == null || 
            subWaveIndex >= currentWave.SubWaves.Count)
        {
            return;
        }
        
        var subWave = currentWave.SubWaves[subWaveIndex];
        
        foreach (var spawnData in subWave.enemiesPerSpawnpoint)
        {
            var spawnPoint = spawnData.Key;
            var enemyList = spawnData.Value;
            
            if (!spawnQueues.ContainsKey(spawnPoint))
            {
                spawnQueues[spawnPoint] = new Queue<(Enemy prefab, int count)>();
            }
            
            foreach (var enemyData in enemyList)
            {
                spawnQueues[spawnPoint].Enqueue(enemyData);
            }
        }
        
        StartCoroutine(SpawnEnemiesFromQueues());
    }
    
    private IEnumerator SpawnEnemiesFromQueues()
    {
        const float spawnDelay = 0.2f;
        
        while (HasEnemiesInQueues())
        {
            foreach (var spawnPoint in spawnQueues.Keys)
            {
                if (spawnQueues[spawnPoint].Count > 0)
                {
                    var enemyData = spawnQueues[spawnPoint].Dequeue();
                    
                    for (int i = 0; i < enemyData.count; i++)
                    {
                        EnemyManager.SpawnEnemy(enemyData.prefab, spawnPoint);
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
            }
        }
    }
    
    private bool HasEnemiesInQueues()
    {
        foreach (var queue in spawnQueues.Values)
        {
            if (queue.Count > 0) return true;
        }
        return false;
    }
    
    private void InitializeSpawnQueues()
    {
        spawnQueues.Clear();
        
        if (currentWave.SpawnPoints != null)
        {
            foreach (var spawnPoint in currentWave.SpawnPoints)
            {
                spawnQueues[spawnPoint] = new Queue<(Enemy prefab, int count)>();
            }
        }
    }
    
    private void OnEnemyDied(Enemy enemy)
    {
        if (enemy)
        {
            gameState.AddCurrency(enemy.Reward);
        }
    }
    
    private void CheckWaveCompletion()
    {
        bool allSubWavesSpawned = currentSubWaveIndex >= currentWave.SubWaves.Count;
        bool noSpawnQueues = !HasEnemiesInQueues();
        bool noEnemiesAlive = EnemyManager.EnemyCount == 0;
        
        if (allSubWavesSpawned && noSpawnQueues && noEnemiesAlive)
        {
            EndWave();
        }
    }
    
    private void EndWave()
    {
        if (!isWaveActive) return;
        
        int waveReward = CalculateWaveReward(currentWaveNumber);
        gameState.AddCurrency(waveReward);
        
        currentWaveNumber++;
        gameState.ChangeWave(currentWaveNumber);
        
        isWaveActive = false;
        EnemyManager.OnEnemyDied -= OnEnemyDied;
        
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            waveCoroutine = null;
        }
        
        spawnQueues.Clear();
    }
    
    private int CalculateWaveReward(int waveNumber)
    {
        return 50 + (waveNumber * 25);
    }
    
    public bool IsWaveActive() => isWaveActive;
    public int GetCurrentWaveNumber() => currentWaveNumber;
    
    void OnDestroy()
    {
        if (isWaveActive)
        {
            EnemyManager.OnEnemyDied -= OnEnemyDied;
        }
        
        if (waveButton)
        {
            waveButton.onClick.RemoveListener(StartWave);
        }
    }
}