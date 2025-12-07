using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float movementSmoothness = 5f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 2f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] Camera cam;
    
    private GameInput _gameInput;
    private Vector3 _targetPosition;
    
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
    }

    private void ReadMovement()
    {
        var inputDirection = _gameInput.Gameplay.CameraMovement.ReadValue<Vector2>();
        
        if (inputDirection != Vector2.zero)
        {
            _targetPosition += new Vector3(inputDirection.x, inputDirection.y, 0) * (cameraSpeed * Time.deltaTime);
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
}