using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }
    
    private GameInput _gameInput;
    public GameInput GameInput => _gameInput;
    
    private bool _isInitialized = false;
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        if (_isInitialized) return;
        
        if (Instance == null)
        {
            Instance = this;
            _isInitialized = true;
            
            InitializeGameInput();
            
            // Не делаем DontDestroyOnLoad для всех синглтонов
            // Только если действительно нужно сохранять между сценами
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void InitializeGameInput()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Gameplay.Enable();
        }
    }
    
    private void Start()
    {
        // Дополнительная инициализация, если нужна
        EnsureGameInputEnabled();
    }
    
    private void EnsureGameInputEnabled()
    {
        if (_gameInput != null && !_gameInput.Gameplay.enabled)
        {
            _gameInput.Gameplay.Enable();
        }
    }
    
    public void SwitchToGameplay()
    {
        if (_gameInput == null) return;
        
        _gameInput.TreeUI.Disable();
        _gameInput.Gameplay.Enable();
    }
    
    public void SwitchToTreeUI()
    {
        if (_gameInput == null) return;
        
        _gameInput.Gameplay.Disable();
        _gameInput.TreeUI.Enable();
    }
    
    private void OnEnable()
    {
        // Включаем инпут при активации объекта
        if (_gameInput != null)
        {
            _gameInput.Gameplay.Enable();
        }
    }
    
    private void OnDisable()
    {
        // Отключаем инпут при деактивации
        if (_gameInput != null)
        {
            _gameInput.Disable();
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Cleanup();
            Instance = null;
        }
    }
    
    private void Cleanup()
    {
        if (_gameInput != null)
        {
            _gameInput.Dispose();
            _gameInput = null;
        }
        
        _isInitialized = false;
    }
    
    public static GameInput GetGameInput()
    {
        if (Instance != null && Instance.GameInput != null)
        {
            return Instance.GameInput;
        }
        
        Debug.LogWarning("GameInputManager not initialized, creating temporary input");
        var tempInput = new GameInput();
        tempInput.Gameplay.Enable();
        return tempInput;
    }
}