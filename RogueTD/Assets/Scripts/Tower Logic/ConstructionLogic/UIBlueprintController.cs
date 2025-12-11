using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIBlueprintController : MonoBehaviour
{
    [SerializeField] private LayerMask gridLayer;
    private BuildingBlueprint selectedBlueprint;
    private GameInput _gameInput;
    private GameObject _buildingPreview;
    private SpriteRenderer _previewRenderer;
    private Color _originalPreviewColor;
    [SerializeField] GameState gameState;
    
    private void Start()
    {
        UiBlueprintItem.SelectBlueprint += OnBlueprintSelected;
        
        if (GameInputManager.Instance != null)
        {
            _gameInput = GameInputManager.Instance.GameInput;
            _gameInput.Gameplay.RightMouseClick.started += OnRightMouseClick;
            _gameInput.Gameplay.LeftMouseClick.started += OnLeftMouseClick;
        }
    }

    private void Update()
    {
        if (_buildingPreview && selectedBlueprint)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 gridPos = GetGridPosition(mouseWorldPos);
            Vector3 previewWorldPos = BuildingFactory.GetWorldPosition(gridPos);
            _buildingPreview.transform.position = previewWorldPos;
        }
    }

    private void OnBlueprintSelected(BuildingBlueprint blueprint)
    {
        if (blueprint == null)
        {
            Debug.LogWarning("Received null blueprint in OnBlueprintSelected");
            return;
        }
        
        selectedBlueprint = blueprint;
        Debug.Log($"Blueprint selected: {blueprint.buildingName}");
        
        ClearPreview();
        
        CreateBuildingPreview(blueprint);
    }

    private void CreateBuildingPreview(BuildingBlueprint blueprint)
    {
        if (!blueprint.BuildingPrefab) return;
        
        _buildingPreview = new GameObject($"{blueprint.buildingName}_Preview");
        
        var prefabSpriteRenderer = blueprint.BuildingPrefab.GetComponent<SpriteRenderer>();
        if (prefabSpriteRenderer != null)
        {
            _previewRenderer = _buildingPreview.AddComponent<SpriteRenderer>();
            _previewRenderer.sprite = prefabSpriteRenderer.sprite;
            _originalPreviewColor = new Color(1f, 1f, 1f, 0.5f);
            _previewRenderer.color = _originalPreviewColor;
            _previewRenderer.sortingOrder = 10; 
            _buildingPreview.transform.localScale = blueprint.BuildingPrefab.transform.localScale;
        }
    }

    private void OnLeftMouseClick(InputAction.CallbackContext context)
    {
        if (!selectedBlueprint) 
        {
            Debug.Log("No blueprint selected!");
            return;
        }
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2Int gridPos = GetGridPosition(mousePosition);
        if (BuildingFactory.CanPlaceBuilding(gridPos, selectedBlueprint) && gameState.SpendCurrency(selectedBlueprint.Cost))
        {
            ConstructionGridManager.TryCreateBlueprint(selectedBlueprint, gridPos);
        }
        
    }

    
    
    private Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        
        Vector3Int cellPosition = ConstructionGridManager.ConstructionGrid.WorldToCell((Vector2) worldPosition);
        return new Vector2Int(cellPosition.x, cellPosition.y);
    }
    

    private void OnRightMouseClick(InputAction.CallbackContext context)
    {
        selectedBlueprint = null;
        ClearPreview();
        Debug.Log("Blueprint selection cleared");
    }
    
    private void ClearPreview()
    {
        if (_buildingPreview)
        {
            Destroy(_buildingPreview);
            _buildingPreview = null;
            _previewRenderer = null;
        }
    }
    
    private void OnDestroy()
    {
        UiBlueprintItem.SelectBlueprint -= OnBlueprintSelected;
        
        if (_gameInput != null)
        {
            _gameInput.Gameplay.RightMouseClick.started -= OnRightMouseClick;
            _gameInput.Gameplay.LeftMouseClick.started -= OnLeftMouseClick;
        }
        
        ClearPreview();
    }
}