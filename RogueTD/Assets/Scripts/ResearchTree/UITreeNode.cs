using UnityEngine;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private Text titleText;
    [SerializeField] private Color activeColor = new Color(220,220,220);
    [SerializeField] private Color availableColor = new Color(169,169,169);
    [SerializeField] private Color unavailableColor = new Color(100,100,100);
    [SerializeField] private TreeSolver treeSolver;
    private TreeNode selectedNode;
    
    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (selectedNode == null || selectedNode.IsActive) return;
        
        if (treeSolver != null || !treeSolver.CanActivateNode(selectedNode)) return;
            
        selectedNode.OnActivate();
        selectedNode.IsActive = true; 
        
        
        UpdateVisualState();
    }

    public void SetNode(TreeNode node)
    {
        selectedNode = node;
        if (titleText != null) titleText.text = node.name;
        UpdateVisualState();
    }

    public void UpdateButtonState(bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;
        }
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (selectedNode == null || background == null) return;

        if (selectedNode.IsActive)
        {
            background.color = activeColor;
            if (button != null) button.interactable = false;
        }
        else if (treeSolver != null && treeSolver.CanActivateNode(selectedNode))
        {
            background.color = availableColor;
            if (button != null) button.interactable = true;
        }
        else
        {
            background.color = unavailableColor;
            if (button != null) button.interactable = false;
        }
    }

    private void OnEnable()
    {
        UpdateVisualState();
    }
}