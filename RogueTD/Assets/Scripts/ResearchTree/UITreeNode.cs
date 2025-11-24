using UnityEngine;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour
{
    private Button button;
    private TreeNode selectedNode;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (selectedNode.IsActive)
            return;
            
        selectedNode.OnActivate();
        UpdateButtonState();
    }

    public void SetNode(TreeNode node)
    {
        selectedNode = node;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (selectedNode != null)
        {
            button.interactable = !selectedNode.IsActive;
        }
    }

    private void OnEnable()
    {
        UpdateButtonState();
    }
}