using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GameState
{
    public static GameState Instance;
    private bool isANewRun;
    public ResearchTree.TreeSaveData TreeSaveData { get; set; }
    public bool IsANewRun { get => isANewRun; set  => isANewRun = value; }
    
    public EnemyWave CurrentWave {get; set;}
    [SerializeField] private List<BuildingSaveData> buildings = new List<BuildingSaveData>();
    public List<BuildingSaveData> Buildings 
    { 
        get => buildings; 
        set => buildings = value ?? new List<BuildingSaveData>(); 
    }

    private int _currency;
    private int _wave;
    
    public int Wave
    {
        get => _wave;
        set => _wave = value;
    }

    public int Currency
    {
        get => _currency;
    }

    public Action<int> OnWaveChanged;
    public Action<int> OnCurrencyChanged;
    private static object get;

    public void ChangeWave(int newWave)
    {
        if (newWave < 0) return;
        
        int oldWave = _wave;
        _wave = newWave;
        
    }
    
    public void IncrementWave()
    {
        ChangeWave(_wave + 1);
    }
    
    public void ResetWave()
    {
        ChangeWave(0);
    }

    public void ChangeCurrency(int amount)
    {
        
        int newCurrency = _currency + amount;
        OnCurrencyChanged.Invoke(newCurrency);
        _currency = newCurrency;
    }
    
    public void AddCurrency(int amount)
    {
        if (amount < 0){ return; }
        ChangeCurrency(amount);
    }
    
    public bool SpendCurrency(int amount)
    {
        if (amount < 0){ return false; }
        
        if (HasEnoughCurrency(amount))
        {
            ChangeCurrency(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool HasEnoughCurrency(int amount)
    {
        return _currency >= amount;
    }
    
    public void ResetGameState()
    {
        Buildings = new List<BuildingSaveData>();
        CurrentWave = null;
        _currency = 0;
        _wave = 0;
    }
    
    public void Initialize(int startingCurrency = 100, int startingWave = 0)
    {
        _currency = Mathf.Max(0, startingCurrency);
        _wave = Mathf.Max(0, startingWave);
        
    }
    
}
