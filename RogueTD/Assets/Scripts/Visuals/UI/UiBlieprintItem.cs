using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBlueprintItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    
    private BuildingBlueprint blueprint;
    
    public static event Action<BuildingBlueprint> SelectBlueprint;
    public TMP_Text NameText => nameText;
    public TMP_Text CostText => costText;
    
    private void Start()
    {
        if (!button)
            button = GetComponent<Button>();
            
        button.onClick.AddListener(OnButtonClick);
    }
    
    public void Initialize(BuildingBlueprint newBlueprint)
    {
        blueprint = newBlueprint;
        
        if (nameText && blueprint != null)
            nameText.text = blueprint.BuildingName;
        
        if (costText && blueprint != null)
            costText.text = blueprint.Cost.ToString();
    }
    
    private void OnButtonClick()
    {
        if (blueprint != null)
        {
            Debug.Log($"UIBlueprintItem clicked: {blueprint.BuildingName}");
            SelectBlueprint?.Invoke(blueprint);
        }
        else
        {
            Debug.LogWarning("Blueprint is null in UiBlueprintItem!");
        }
    }
    
    private void OnDestroy()
    {
        if (button)
            button.onClick.RemoveListener(OnButtonClick);
    }
}