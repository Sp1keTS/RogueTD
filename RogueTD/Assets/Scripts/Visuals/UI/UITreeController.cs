using UnityEngine;
using UnityEngine.UI;

public class UITreeController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Canvas treeUICanvas;
    [SerializeField] private GameObject treeUI;
    [SerializeField] float treeSpeed = 5f;
    [SerializeField] float movementSmoothness = 10f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 0.1f;
    [SerializeField] float maxZoom = 3f;
    
    private GameInput _gameInput;
    private Vector3 _targetPosition;
    private float _targetZoom = 1f;
    private Vector3 _originalTreePosition;
    private bool _isTreeActive = false;

    private void Start()
    {
        
        _gameInput = GameInputManager.Instance.GameInput;
        
        _originalTreePosition = treeUI.transform.position;
        _targetPosition = _originalTreePosition;
        
        if (button != null)
        {
            button.onClick.AddListener(ToggleCanvas);
        }
        
        if (treeUICanvas != null)
        {
            treeUICanvas.gameObject.SetActive(false);
            _isTreeActive = false;
        }
    }

    private void Update()
    {
        if (!_isTreeActive) return;
        
        ReadMovement();
        ReadZoom();
        ApplyMovement();
        ApplyZoom();
    }

    private void ReadMovement()
    {
        var inputDirection = _gameInput.TreeUI.TreeMovement.ReadValue<Vector2>();
        
        if (inputDirection != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(inputDirection.x, inputDirection.y, 0);
            _targetPosition += moveDirection * (treeSpeed * Time.deltaTime);
        }
    }

    private void ApplyMovement()
    {
        if (treeUI)
        {
            treeUI.transform.position = Vector3.Lerp(
                treeUI.transform.position, 
                _targetPosition, 
                movementSmoothness * Time.deltaTime
            );
        }
    }

    private void ReadZoom()
    {
        var zoomDirection = _gameInput.TreeUI.Zoom.ReadValue<float>();
        if (zoomDirection != 0)
        {
            float zoomChange = -zoomDirection * zoomSpeed * Time.deltaTime;
            _targetZoom = Mathf.Clamp(_targetZoom + zoomChange, minZoom, maxZoom);
        }
    }

    private void ApplyZoom()
    {
        if (treeUI)
        {
            treeUI.transform.localScale = Vector3.one * Mathf.Lerp(
                treeUI.transform.localScale.x, 
                _targetZoom, 
                movementSmoothness * Time.deltaTime
            );
        }
    }

    private void ToggleCanvas()
    {
        if (treeUICanvas == null) return;
        
        _isTreeActive = !treeUICanvas.gameObject.activeSelf;
        treeUICanvas.gameObject.SetActive(_isTreeActive);
        
        if (_isTreeActive)
        {
            GameInputManager.Instance.SwitchToTreeUI();
            ResetTreeView();
        }
        else
        {
            GameInputManager.Instance.SwitchToGameplay();
        }
    }
    
    private void ResetTreeView()
    {
        _targetPosition = _originalTreePosition;
        _targetZoom = 1f;
        
        if (treeUI != null)
        {
            treeUI.transform.position = _originalTreePosition;
            treeUI.transform.localScale = Vector3.one;
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(ToggleCanvas);
        }
    }
}