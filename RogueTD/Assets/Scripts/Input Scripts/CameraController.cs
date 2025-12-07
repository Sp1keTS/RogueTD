using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float movementSmoothness = 5f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 2f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] Camera cam;
    [SerializeField] MapManager mapManager;
    
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;
    
    private GameInput _gameInput;
    private Vector3 _targetPosition;
    private float _mapWidth;
    private float _mapHeight;
    
    private void Start()
    {
        _gameInput = GameInputManager.Instance.GameInput;
        if (cam == null)
            cam = GetComponent<Camera>();
            
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_gameInput.Gameplay.enabled) return;
        
        ReadMovement();
        ReadZoom();
        ApplyMovement();
        ApplyZoom();
    }

    private void ReadMovement()
    {
        var inputDirection = _gameInput.Gameplay.CameraMovement.ReadValue<Vector2>();
        
        if (inputDirection != Vector2.zero)
        {
            Vector3 newPosition = _targetPosition + new Vector3(inputDirection.x, inputDirection.y, 0) * (cameraSpeed * Time.deltaTime);
            
            _targetPosition = ClampCameraPosition(newPosition);
        }
    }

    private void ApplyMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            _targetPosition, 
            movementSmoothness * Time.deltaTime
        );
    }

    private void ReadZoom()
    {
        var zoomDirection = _gameInput.Gameplay.Zoom.ReadValue<float>();
        if (zoomDirection != 0)
        {
            float newSize = cam.orthographicSize - zoomDirection * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
    
    private void ApplyZoom()
    {
        _targetPosition = ClampCameraPosition(_targetPosition);
    }

    private Vector3 ClampCameraPosition(Vector3 position)
    {
        var cameraHeight = cam.orthographicSize;
        var cameraWidth = cameraHeight * cam.aspect;
        
        var clampedX = Mathf.Clamp(position.x, minX + cameraWidth, maxX - cameraWidth);
        var clampedY = Mathf.Clamp(position.y, minY + cameraHeight, maxY - cameraHeight);
        
        return new Vector3(clampedX, clampedY, position.z);
    }
    
}