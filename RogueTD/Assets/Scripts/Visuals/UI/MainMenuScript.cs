using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] GameState gameState;
    [SerializeField] int scene;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject settingsCanvas;
    void Start()
    {
        newGameButton = newGameButton.GetComponent<Button>();
        continueButton = continueButton.GetComponent<Button>();
        settingsButton = settingsButton.GetComponent<Button>();
        scene = SceneManager.GetActiveScene().buildIndex + 1;
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnNewGameButtonClick()
    {
        gameState.IsANewRun = true;
        SceneManager.LoadScene(scene);
    }

    private void OnContinueButtonClick()
    {
        gameState.IsANewRun = false;
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
