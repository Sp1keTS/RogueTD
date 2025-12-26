using UnityEngine;

public class UIToolTipManager : MonoBehaviour
{
    public static UIToolTipManager Instance;
    [SerializeField] private UITooltip tooltipWindow;
    
    public void ShowTooltip(string header, string content, Vector2 tTPosition)
    {
        tooltipWindow.transform.position =  tTPosition - new Vector2(tooltipWindow.Size.x / 2 + 5,tooltipWindow.Size.y / 2);
        tooltipWindow.Header = header;
        tooltipWindow.Content = content;
        tooltipWindow.SetActive(true);
    }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        tooltipWindow.SetActive(false);
    }
    public void HideTooltip()
    {
        tooltipWindow.SetActive(false);
    }
}
