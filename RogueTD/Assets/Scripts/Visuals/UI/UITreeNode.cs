using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    ResearchTree.TreeSaveData.TreeSaveNode treeSaveNode;
    public ResearchTree.TreeSaveData.TreeSaveNode  TreeSaveNode {get => treeSaveNode; set => treeSaveNode = value; }
    public int Rank {get; set;} 
    public Button Button => button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetImage()
    {
        image.sprite = treeSaveNode.currentNodeConfig.Icon;
    }

    private void OnButtonClick()
    {
        var isRootNode = treeSaveNode.visitedNodes == null || treeSaveNode.visitedNodes.Count == 0;
        var isChildNodeAvailable = IsParentActive();

        if (!treeSaveNode.IsActive && (isRootNode || isChildNodeAvailable))
        {
            if (GameState.Instance.SpendCurrency(treeSaveNode.currentNode.GetDynamicCost(Rank)))
            {
                treeSaveNode.currentNode.OnActivate(treeSaveNode.currentNode.CurrentRank);
                treeSaveNode.IsActive = true;
                button.interactable = false;
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIToolTipManager.Instance.ShowTooltip(TreeSaveNode.currentNodeConfig.name, TreeSaveNode.currentNodeConfig.TooltipText +"/n" + treeSaveNode.currentNode.GetStats(Rank), 
            new Vector2(transform.position.x, transform.position.y) );
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIToolTipManager.Instance.HideTooltip();
    }
}