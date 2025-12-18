using System;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    private void Awake()
    {
        GameState.Instance.OnCurrencyChanged += OnCurrencyChanged;
        text.text = GameState.Instance.Currency.ToString();
    }

    private void OnCurrencyChanged(int amount)
    {
        text.text = amount.ToString();
    }
}
