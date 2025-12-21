using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
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
        bool isChildNodeAvailable = IsParentActive();

        if (!treeSaveNode.IsActive && (isRootNode || isChildNodeAvailable))
        {
            if (GameState.Instance.SpendCurrency(treeSaveNode.currentNode.Cost))
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
                GameState.Instance.SaveTreeToJson();
            }
        }
    }

    private void OnEnable()
    {
    }
    
    private void OnDisable()
    {
    }
    private bool IsRootNode()
    {
        return treeSaveNode.visitedNodes == null || treeSaveNode.visitedNodes.Count == 0;
    }
    private bool IsParentActive()
    {
        if (IsRootNode()) return true; 
        if (treeSaveNode.visitedNodes != null && treeSaveNode.visitedNodes.Count > 0)
        {
            var parentNode = treeSaveNode.visitedNodes[treeSaveNode.visitedNodes.Count - 1];
            return parentNode != null && parentNode.IsActive;
        }
    
        return false;
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}