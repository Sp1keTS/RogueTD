using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenueUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] GameObject PauseMenuCanvas;
    [SerializeField] GameObject settingsCanvas;
    private GameInput _gameInput;

    private void Awake()
    {
        _gameInput = GameInputManager.Instance.GameInput;
        if (pauseButton == null)
            pauseButton = GetComponent<Button>();
        if (mainMenuButton == null)
            mainMenuButton = GetComponent<Button>();
        if (settingsButton == null)
            settingsButton = GetComponent<Button>();
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnPauseButtonClick()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        _gameInput.Gameplay.Enable();
    }

    private void OnMainMenuButtonClick()
    {
        _gameInput.Gameplay.Enable();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void OnSettingsButtonClick()
    {
        if (settingsCanvas && PauseMenuCanvas)
        {
            settingsCanvas.SetActive(true);
            PauseMenuCanvas.SetActive(false);
        }
    }
}