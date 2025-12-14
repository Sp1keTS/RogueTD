using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Button goBackButton;
    [SerializeField] Button soundSettingsButton;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] Slider globalVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] TMP_InputField globalVolumeInputField;
    [SerializeField] TMP_InputField effectsInputField;
    [SerializeField] TMP_InputField musicInputField;
    
    void Start()
    {
        goBackButton = goBackButton.GetComponent<Button>();
        soundSettingsButton = soundSettingsButton.GetComponent<Button>();
        globalVolumeSlider = globalVolumeSlider.GetComponent<Slider>();
        effectsVolumeSlider = effectsVolumeSlider.GetComponent<Slider>();
        musicVolumeSlider = musicVolumeSlider.GetComponent<Slider>();
        globalVolumeInputField = globalVolumeInputField.GetComponent<TMP_InputField>();
        effectsInputField = effectsInputField.GetComponent<TMP_InputField>();
        musicInputField = musicInputField.GetComponent<TMP_InputField>();
        
        soundSettingsButton.interactable = false;
        
        goBackButton.onClick.AddListener(OnGoBackClicked);
        
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeSliderValueChanged);
        globalVolumeInputField.onValueChanged.AddListener(OnGlobalVolumeInputFieldValueChanged);
        
        effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeSliderValueChanged);
        effectsInputField.onValueChanged.AddListener(OnEffectsInputFieldValueChanged);
        
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        musicInputField.onValueChanged.AddListener(OnMusicInputFieldValueChanged);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGoBackClicked()
    {
        if (mainMenuCanvas != null && settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
        }
    }

    private void OnGlobalVolumeSliderValueChanged(float value)
    {
        globalVolumeInputField.text = value.ToString();
    }

    private void OnGlobalVolumeInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        globalVolumeSlider.value = currentVolume;
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        musicInputField.text = value.ToString();
    }

    private void OnMusicInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        musicVolumeSlider.value = currentVolume;
    }

    private void OnEffectsVolumeSliderValueChanged(float value)
    {
        effectsInputField.text = value.ToString();
    }

    private void OnEffectsInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        effectsVolumeSlider.value = currentVolume;
    }
}
