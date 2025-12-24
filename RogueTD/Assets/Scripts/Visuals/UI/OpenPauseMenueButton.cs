using System;
using UnityEngine;
using UnityEngine.UI;


public class OpenPauseMenueButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameInput _gameInput;

    private void Start()
    {
        _gameInput = GameInputManager.Instance.GameInput;
        if (pauseButton == null)
            pauseButton = GetComponent<Button>();
        
        pauseButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        _gameInput.Gameplay.Disable();
    }
}
