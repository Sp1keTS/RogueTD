using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    ResearchTree.TreeSaveData.TreeSaveNode treeSaveNode;
    public ResearchTree.TreeSaveData.TreeSaveNode  TreeSaveNode {get => treeSaveNode; set => treeSaveNode = value; }
    
    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null) button = GetComponent<Button>();
        
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetImage()
    {
        image.sprite = treeSaveNode.currentNode.Icon;
    }
    
    private void OnButtonClick()
    {
        if (!treeSaveNode.IsActive && treeSaveNode.visitedNodes[0].IsActive)
        {
            treeSaveNode.currentNode.OnActivate();
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