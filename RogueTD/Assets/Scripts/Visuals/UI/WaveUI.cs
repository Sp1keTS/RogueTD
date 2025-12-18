using System;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    private void Awake()
    {
        GameState.Instance.OnCurrencyChanged += OnWaveChanged;
        text.text = GameState.Instance.Wave.ToString();
    }

    private void OnWaveChanged(int amount)
    {
        text.text = amount.ToString();
    }
}
