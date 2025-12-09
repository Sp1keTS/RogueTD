using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Tower Defense/Game State")]
public class GameState : ScriptableObject
{
    
    public ResearchTree.TreeSaveData TreeSaveData { get; set; }
    
    private int _currency;
    private int _wave;
    
    public int Wave
    {
        get => _wave;
    }

    public int Currency
    {
        get => _currency;
    }

    public Action<int> OnWaveChanged;
    public Action<int> OnCurrencyChanged;

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
        int oldCurrency = _currency;
        int oldWave = _wave;
        
        _currency = 0;
        _wave = 0;
        TreeSaveData = null;
    }
    
    public void Initialize(int startingCurrency = 100, int startingWave = 0)
    {
        _currency = Mathf.Max(0, startingCurrency);
        _wave = Mathf.Max(0, startingWave);
        
    }
    
}
