using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] int scene;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject settingsCanvas;
    
    [SerializeField] AudioClip menuMusic;
    
    void Start()
    {
        GameState.Instance.LoadGameState();
        
        newGameButton = newGameButton.GetComponent<Button>();
        continueButton = continueButton.GetComponent<Button>();
        settingsButton = settingsButton.GetComponent<Button>();
        scene = SceneManager.GetActiveScene().buildIndex + 1;
        
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        
        continueButton.interactable = GameState.Instance.HasSavedData();
    }

    private void OnNewGameButtonClick()
    {
        GameState.Instance.IsANewRun = true;
        SceneManager.LoadScene(scene);
    }

    private void OnContinueButtonClick()
    {
        GameState.Instance.IsANewRun = false;
        SceneManager.LoadScene(scene);
    }

    private void OnSettingsButtonClick()
    {
        if (mainMenuCanvas != null && settingsCanvas != null)
        {
            settingsCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
        }
    }
}