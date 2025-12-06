using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button showTreebutton;
    [SerializeField] private Canvas treeUICanvas;

    private void Start()
    {
        if (showTreebutton != null)
        {
            showTreebutton.onClick.AddListener(ToggleCanvas);
        }
    }

    private void ToggleCanvas()
    {
        if (treeUICanvas != null)
        {
            treeUICanvas.gameObject.SetActive(!treeUICanvas.gameObject.activeSelf);
        }
    }

    private void OnDestroy()
    {
        if (showTreebutton != null)
        {
            showTreebutton.onClick.RemoveListener(ToggleCanvas);
        }
    }
}
