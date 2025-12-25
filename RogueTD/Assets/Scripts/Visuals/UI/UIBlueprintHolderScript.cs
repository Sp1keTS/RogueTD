using System;
using System.Collections.Generic;
using UnityEngine;

public class UIBlueprintHolderScript : MonoBehaviour
{
    [SerializeField] private UiBlueprintItem uiBlueprintItemPrefab;
    private Dictionary<string, BuildingBlueprint> blueprints = new Dictionary<string, BuildingBlueprint>();
    private Dictionary<string, UiBlueprintItem> blueprintItems = new Dictionary<string, UiBlueprintItem>();
    
    private void Start()
    {
        BlueprintManager.ProjectileTowerChanged += AddUiBlueprint;
    }
    
    private void OnDestroy()
    {
        BlueprintManager.ProjectileTowerChanged -= AddUiBlueprint;
    }

    private void AddUiBlueprint(BuildingBlueprint blueprint, string name)
    {
        if (!blueprint || blueprint.buildingName == "MainBuilding") return;
        
        
        if (!blueprints.ContainsKey(name))
        {
            blueprints.Add(name, blueprint);
            
            var item = Instantiate(uiBlueprintItemPrefab, transform);
            item.Initialize(blueprint); 
            blueprintItems.Add(name, item);
            
            if (item.NameText != null)
                item.NameText.text = blueprint.buildingName;
            
            if (item.CostText != null)
                item.CostText.text = blueprint.Cost.ToString();
        }
        else
        {
            blueprints[name] = blueprint;
            
            if (blueprintItems.TryGetValue(name, out var existingItem))
            {
                existingItem.Initialize(blueprint);
                
                if (existingItem.NameText != null)
                    existingItem.NameText.text = blueprint.buildingName;
                
                if (existingItem.CostText != null)
                    existingItem.CostText.text = blueprint.Cost.ToString();
            }
        }
    }
    
    public void LoadExistingBlueprints()
    {
        foreach (var kvp in BlueprintManager.blueprints)
        {
            AddUiBlueprint(kvp.Value, kvp.Key);
        }
    }
}