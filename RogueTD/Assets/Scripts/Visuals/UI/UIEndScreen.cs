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
    
    private void OnEnable()
    {
        if (_endGameCoroutine != null)
            StopCoroutine(_endGameCoroutine);
            
        _endGameCoroutine = StartCoroutine(EndGameSequence());
    }
    
    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        _gameInput = GameInputManager.Instance.GameInput;
        
        if (mainMenuButton)
            mainMenuButton = mainMenuButton.GetComponent<Button>();
            
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        
        if (!endCanvasGroup && endPanel)
            endCanvasGroup = endPanel.GetComponent<CanvasGroup>();
        
        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (_endGameCoroutine != null)
            StopCoroutine(_endGameCoroutine);
    }
    
    public void EndGame(string message)
    {
        endText.text = message;
        gameObject.SetActive(true);
        
        GameState.Instance.DeleteAllSaveFiles();
    }
    
    private IEnumerator EndGameSequence()
    {
        _gameInput.Disable();
        
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
        
        if (endPanel && endCanvasGroup)
        {
            endPanel.SetActive(true);
            
            if (!endCanvasGroup)
                endCanvasGroup = endPanel.GetComponent<CanvasGroup>();
            
            if (endCanvasGroup)
            {
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
        }
    }
    
    private void OnMainMenuButtonClick()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        _gameInput.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}