using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    [SerializeField] private GameState gameState;
    ResearchTree.TreeSaveData.TreeSaveNode treeSaveNode;
    public ResearchTree.TreeSaveData.TreeSaveNode  TreeSaveNode {get => treeSaveNode; set => treeSaveNode = value; }
    public ProjectileTowerNode towerToUpgrade {get; set;}
    public Button Button => button;
    private void Awake()
    {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetImage()
    {
        image.sprite = treeSaveNode.currentNode.Icon;
    }
    
    private void OnButtonClick()
    {
        bool isRootNode = treeSaveNode.visitedNodes == null || treeSaveNode.visitedNodes.Count == 0;
        bool isChildNodeAvailable = !isRootNode && treeSaveNode.visitedNodes[0].IsActive;
        
        Debug.Log(gameState.Currency);
        Debug.Log(TreeSaveNode.currentNode.Cost);
        Debug.Log(gameState.SpendCurrency(TreeSaveNode.currentNode.Cost));
        if (!treeSaveNode.IsActive && (isRootNode || isChildNodeAvailable) && gameState.SpendCurrency(TreeSaveNode.currentNode.Cost))
        {
            if (treeSaveNode.currentNode is ProjectileTowerUpgradeTreeNode upgradeNode)
            {
                if (towerToUpgrade != null && towerToUpgrade.TowerBlueprint != null)
                {
                    upgradeNode.ApplyUpgrade(towerToUpgrade.TowerBlueprint, upgradeNode.CurrentRank);
                }
            }
            
            treeSaveNode.currentNode.OnActivate();
            treeSaveNode.IsActive = true;
            button.interactable = false;
        }
    }
    
    private void OnEnable()
    {
    }
    
    private void OnDisable()
    {
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}