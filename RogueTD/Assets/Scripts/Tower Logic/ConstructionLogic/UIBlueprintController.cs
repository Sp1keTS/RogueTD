using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIBlueprintController : MonoBehaviour
{
    [SerializeField] private LayerMask gridLayer;
    private BuildingBlueprint selectedBlueprint;
    private GameInput _gameInput;
    
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

    private void OnBlueprintSelected(BuildingBlueprint blueprint)
    {
        if (blueprint == null)
        {
            Debug.LogWarning("Received null blueprint in OnBlueprintSelected!");
            return;
        }
        
        selectedBlueprint = blueprint;
        Debug.Log($"Blueprint selected: {blueprint.buildingName}");
    }

    private void OnLeftMouseClick(InputAction.CallbackContext context)
    {
        if (selectedBlueprint == null) 
        {
            Debug.Log("No blueprint selected!");
            return;
        }
        
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 gridPos = GetGridPosition(mousePosition);
        
        TryCreateBlueprint(selectedBlueprint, gridPos);
    }

    private void TryCreateBlueprint(BuildingBlueprint blueprint, Vector2 gridPosition)
    {
        if (blueprint == null)
        {
            return;
        }
        
        
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        {
            BuildingFactory.CreateProjectileTower(gridPosition, projectileBlueprint);
        }
        else
        {
            BuildingFactory.CreateBuilding(gridPosition, blueprint);
        }
        
    }
    
    private Vector2 GetGridPosition(Vector2 worldPosition)
    {
        if (ConstructionGridManager.constructionGrid == null)
        {
            return worldPosition;
        }
        
        Vector3Int cellPosition = ConstructionGridManager.constructionGrid.WorldToCell(worldPosition);
        return new Vector2(cellPosition.x, cellPosition.y);
    }

    private void OnRightMouseClick(InputAction.CallbackContext context)
    {
        selectedBlueprint = null;
    }
    
    private void OnDestroy()
    {
        UiBlueprintItem.SelectBlueprint -= OnBlueprintSelected;
        
        if (_gameInput != null)
        {
            _gameInput.Gameplay.RightMouseClick.started -= OnRightMouseClick;
            _gameInput.Gameplay.LeftMouseClick.started -= OnLeftMouseClick;
        }
    }
    
}