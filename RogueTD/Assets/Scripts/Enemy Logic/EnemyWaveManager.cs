using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private Button waveButton;
    [SerializeField] private EnemyWaveGenerator waveGenerator;
    [SerializeField] private float timeBetweenSubWaves = 5f;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private EnemyRegistry enemyRegistry;
    
    private bool isWaveActive = false;
    private int currentWaveNumber = 0;
    private EnemyWave currentWave;
    private int currentSubWaveIndex = 0;
    private Coroutine waveCoroutine;
    
    private Dictionary<Vector2, Queue<KeyValuePair<string, int>>> spawnQueues = 
        new Dictionary<Vector2, Queue<KeyValuePair<string, int>>>();
    
    void Start()
    {
        currentWaveNumber = GameState.Instance.Wave;
        
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
        
        if (GameState.Instance.CurrentWave != null)
        {
            currentWave = GameState.Instance.CurrentWave;
        }
        else
        {
            currentWave = waveGenerator.GenerateWave(currentWaveNumber);
        }
        
        if (currentWave == null || currentWave.SubWaves == null || currentWave.SubWaves.Count == 0)
        {
            Debug.LogError("Не удалось создать или загрузить волну!");
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
        
        Debug.Log($"Начата волна {currentWaveNumber}");
        GameState.Instance.CurrentWave = currentWave;
        GameState.Instance.SaveToJson();
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
        
        Debug.Log("Все подволны запущены");
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
        Debug.Log($"Запуск подволны {subWaveIndex + 1}/{currentWave.SubWaves.Count}");
        
        foreach (var spawnPointData in subWave.enemiesPerSpawnpoint)
        {
            var spawnPoint = spawnPointData.Key;
            var enemyDict = spawnPointData.Value; 
            
            if (!spawnQueues.ContainsKey(spawnPoint))
            {
                spawnQueues[spawnPoint] = new Queue<KeyValuePair<string, int>>();
            }
            
            foreach (var enemyData in enemyDict)
            {
                spawnQueues[spawnPoint].Enqueue(enemyData);
                Debug.Log($"Добавлен в очередь: {enemyData.Key} x{enemyData.Value} в точку {spawnPoint}");
            }
        }
        
        StartCoroutine(SpawnEnemiesFromQueues());
    }
    
    private IEnumerator SpawnEnemiesFromQueues()
    {
        while (HasEnemiesInQueues())
        {
            bool spawnedThisFrame = false;
            
            foreach (var spawnPoint in spawnQueues.Keys)
            {
                if (spawnQueues[spawnPoint].Count > 0)
                {
                    var enemyData = spawnQueues[spawnPoint].Dequeue();
                    string enemyId = enemyData.Key;
                    int count = enemyData.Value;
                    
                    Enemy enemyPrefab = enemyRegistry.GetEnemyPrefab(enemyId);
                    
                    if (enemyPrefab)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Vector2 offset = UnityEngine.Random.insideUnitCircle * 2f;
                            EnemyManager.SpawnEnemy(enemyPrefab, spawnPoint + offset);
                            
                            spawnedThisFrame = true;
                            yield return new WaitForSeconds(spawnDelay);
                        }
                        
                    }
                }
            }
            
            if (!spawnedThisFrame)
            {
                yield return null;
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
                spawnQueues[spawnPoint] = new Queue<KeyValuePair<string, int>>();
            }
        }
    }
    
    private void OnEnemyDied(Enemy enemy)
    {
        if (enemy)
        {
            GameState.Instance.AddCurrency(enemy.Reward);
        }
    }
    
    private void CheckWaveCompletion()
    {
        if (!isWaveActive) return;
        
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
        
        Debug.Log($"Волна {currentWaveNumber} завершена!");
        
        int waveReward = CalculateWaveReward(currentWaveNumber);
        GameState.Instance.AddCurrency(waveReward);
        
        GameState.Instance.CurrentWave = currentWave;
        
        currentWaveNumber++;
        GameState.Instance.ChangeWave(currentWaveNumber);
        
        isWaveActive = false;
        EnemyManager.OnEnemyDied -= OnEnemyDied;
        
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            waveCoroutine = null;
        }
        
        spawnQueues.Clear();
        GameState.Instance.CurrentWave = null;
        GameState.Instance.SaveToJson();
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