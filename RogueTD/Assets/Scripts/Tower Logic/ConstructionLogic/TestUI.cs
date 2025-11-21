using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{

    [Header("UI Elements")] 
    [SerializeField] private Button homingButton;
    [SerializeField] private Button splitButton;
    [SerializeField] private Button explosionButton;
    [SerializeField] private Button slowButton;
    [SerializeField] private Button bleedButton;
    [SerializeField] private Button burstButton;
    [SerializeField] private Button crossButton;

    void Start()
    {
        homingButton.onClick.AddListener(() => 
        {
            GameHandler.AddHoming();
            UpdateButtonText(homingButton, "Homing", true);
        });
        
        splitButton.onClick.AddListener(() => 
        {
            GameHandler.AddSplit();
            UpdateButtonText(splitButton, "Split", true);
        });
        
        explosionButton.onClick.AddListener(() => 
        {
            GameHandler.AddExplosion();
            UpdateButtonText(explosionButton, "Explosion", true);
        });
        
        slowButton.onClick.AddListener(() => 
        {
            GameHandler.AddSlow();
            UpdateButtonText(slowButton, "Slow", true);
        });
        
        bleedButton.onClick.AddListener(() => 
        {
            GameHandler.AddBleed();
            UpdateButtonText(bleedButton, "Bleed", true);
        });
        
        burstButton.onClick.AddListener(() => 
        {
            GameHandler.AddBurst();
            UpdateButtonText(burstButton, "Burst", true);
        });
        
        crossButton.onClick.AddListener(() => 
        {
            GameHandler.AddCross();
            UpdateButtonText(crossButton, "Cross", true);
        });
    }

    private void UpdateButtonText(Button button, string baseText, bool isActive)
    {
        if (button != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = isActive ? $"{baseText} ✓" : baseText;
            }
        }
    }
}