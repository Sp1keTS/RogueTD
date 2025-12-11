using System;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private GameState gameState;

    private void Awake()
    {
        gameState.OnCurrencyChanged += OnCurrencyChanged;
        text.text = gameState.Currency.ToString();
    }

    private void OnCurrencyChanged(int amount)
    {
        text.text = amount.ToString();
    }
}
