using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIEndScreen : MonoBehaviour
{
    public static UIEndScreen Instance;
    
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endText;
    
    private CanvasGroup endCanvasGroup;
    [SerializeField] private float slowDownDuration = 1.5f; 
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private AnimationCurve slowDownCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.5f); 
    [SerializeField] private AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private GameInput _gameInput;
    private Coroutine _endGameCoroutine;
    private Coroutine _waitForInputCoroutine;
    
    private GameInput GameInput
    {
        get
        {
            if (_gameInput == null)
            {
                InitializeGameInput();
            }
            return _gameInput;
        }
    }
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        if (mainMenuButton)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        }
        else
        {
            Debug.LogWarning("mainMenuButton is not assigned in UIEndScreen!");
            
            mainMenuButton = GetComponentInChildren<Button>();
            if (mainMenuButton)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            }
        }
        
        if (endPanel)
        {
            endCanvasGroup = endPanel.GetComponent<CanvasGroup>();
            if (!endCanvasGroup)
            {
                endCanvasGroup = endPanel.AddComponent<CanvasGroup>();
            }
            endPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("endPanel is not assigned in UIEndScreen!");
        }
    }
    
    private void Start()
    {
        InitializeGameInput();
    }
    
    private void InitializeGameInput()
    {
        if (_gameInput != null) return;
        
        if (GameInputManager.Instance)
        {
            _gameInput = GameInputManager.Instance.GameInput;
        }
        else
        {
            if (_waitForInputCoroutine == null)
            {
                _waitForInputCoroutine = StartCoroutine(WaitForGameInputRoutine());
            }
        }
    }
    
    private IEnumerator WaitForGameInputRoutine()
    {
        Debug.Log("Waiting for GameInputManager initialization...");
        
        while (!GameInputManager.Instance)
        {
            yield return null;
        }
        
        yield return new WaitForEndOfFrame();
        
        if (GameInputManager.Instance)
        {
            _gameInput = GameInputManager.Instance.GameInput;
            Debug.Log("GameInput initialized successfully");
        }
        else
        {
            Debug.LogError("Failed to initialize GameInput!");
        }
        
        _waitForInputCoroutine = null;
    }
    
    private void OnDestroy()
    {
        if (_endGameCoroutine != null)
        {
            StopCoroutine(_endGameCoroutine);
        }
        
        if (_waitForInputCoroutine != null)
        {
            StopCoroutine(_waitForInputCoroutine);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
        }
        
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    public void EndGame(string message)
    {
        if (endText)
        {
            endText.text = message;
        }
        
        gameObject.SetActive(true);
        
        if (_endGameCoroutine != null)
        {
            StopCoroutine(_endGameCoroutine);
        }
        
        _endGameCoroutine = StartCoroutine(EndGameSequence());
        
        if (GameState.Instance != null)
        {
            GameState.Instance.DeleteAllSaveFiles();
        }
    }
    
    private IEnumerator EndGameSequence()
    {
        if (_gameInput == null)
        {
            Debug.Log("Waiting for GameInput before ending game...");
            yield return WaitForGameInputRoutine();
        }
        
        if (_gameInput != null)
        {
            _gameInput.Disable();
        }
        else
        {
            Debug.LogWarning("GameInput is still null, proceeding without input disable");
        }
        
        var slowDown = StartCoroutine(SlowDownTimeRoutine());
        var fadeIn = StartCoroutine(FadeInEndScreenRoutine());
        
        yield return slowDown;
        yield return fadeIn;
    }
    
    private IEnumerator SlowDownTimeRoutine()
    {
        float timer = 0f;
        float startTimeScale = Time.timeScale;
        
        while (timer < slowDownDuration)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(timer / slowDownDuration);
            
            float curveValue = slowDownCurve.Evaluate(progress);
            Time.timeScale = Mathf.Lerp(startTimeScale, 0.5f, curveValue);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            yield return null;
        }
        
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    
    private IEnumerator FadeInEndScreenRoutine()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        
        if (!endPanel || !endCanvasGroup)
        {
            Debug.LogError("Cannot fade in end screen - endPanel or endCanvasGroup is null!");
            yield break;
        }
        
        endPanel.SetActive(true);
        endCanvasGroup.alpha = 0f;
        endCanvasGroup.interactable = false;
        endCanvasGroup.blocksRaycasts = false;
        
        float timer = 0f;
        
        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(timer / fadeInDuration);
            
            float curveValue = fadeInCurve.Evaluate(progress);
            endCanvasGroup.alpha = curveValue;
            
            if (progress > 0.7f)
            {
                endCanvasGroup.interactable = true;
                endCanvasGroup.blocksRaycasts = true;
            }
            
            yield return null;
        }
        
        endCanvasGroup.alpha = 1f;
        endCanvasGroup.interactable = true;
        endCanvasGroup.blocksRaycasts = true;
    }
    
    private void OnMainMenuButtonClick()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        _gameInput?.Enable();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > 0)
        {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}