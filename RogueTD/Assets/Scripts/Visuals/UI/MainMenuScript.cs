using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] GameState gameState;
    [SerializeField] int scene;
    void Start()
    {
        newGameButton = newGameButton.GetComponent<Button>();
        continueButton = continueButton.GetComponent<Button>();
        scene = SceneManager.GetActiveScene().buildIndex + 1;
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
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

}
