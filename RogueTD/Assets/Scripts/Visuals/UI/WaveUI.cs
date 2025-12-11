using System;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private GameState gameState;

    private void Awake()
    {
        gameState.OnCurrencyChanged += OnWaveChanged;
        text.text = gameState.Wave.ToString();
    }

    private void OnWaveChanged(int amount)
    {
        text.text = amount.ToString();
    }
}
