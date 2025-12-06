using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }
    
    private GameInput _gameInput;
    public GameInput GameInput => _gameInput;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _gameInput = new GameInput();
            _gameInput.Gameplay.Enable(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SwitchToGameplay()
    {
        _gameInput.TreeUI.Disable();
        _gameInput.Gameplay.Enable();
    }
    
    public void SwitchToTreeUI()
    {
        _gameInput.Gameplay.Disable();
        _gameInput.TreeUI.Enable();
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            _gameInput?.Dispose();
            Instance = null;
        }
    }
}