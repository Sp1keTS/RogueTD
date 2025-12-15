using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Button goBackButton;
    [SerializeField] Button soundSettingsButton;
    [SerializeField] Button saveSettingsButton;
    
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] Slider globalVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] TMP_InputField globalVolumeInputField;
    [SerializeField] TMP_InputField effectsInputField;
    [SerializeField] TMP_InputField musicInputField;
    
    [SerializeField] int defaultVolume = 100;
    [SerializeField] int defaultEffectsVolume = 100;
    [SerializeField] int defaultMusicVolume = 100;
    
    private int globalVolume;
    private int effectsVolume;
    private int musicVolume;
    
    void Start()
    {
        goBackButton = goBackButton.GetComponent<Button>();
        soundSettingsButton = soundSettingsButton.GetComponent<Button>();
        saveSettingsButton = saveSettingsButton.GetComponent<Button>();
        
        globalVolumeSlider = globalVolumeSlider.GetComponent<Slider>();
        effectsVolumeSlider = effectsVolumeSlider.GetComponent<Slider>();
        musicVolumeSlider = musicVolumeSlider.GetComponent<Slider>();
        globalVolumeInputField = globalVolumeInputField.GetComponent<TMP_InputField>();
        effectsInputField = effectsInputField.GetComponent<TMP_InputField>();
        musicInputField = musicInputField.GetComponent<TMP_InputField>();
        
        soundSettingsButton.interactable = false;
        
        goBackButton.onClick.AddListener(OnGoBackClicked);
        saveSettingsButton.onClick.AddListener(SaveSettings);
        
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeSliderValueChanged);
        globalVolumeInputField.onValueChanged.AddListener(OnGlobalVolumeInputFieldValueChanged);
        
        effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeSliderValueChanged);
        effectsInputField.onValueChanged.AddListener(OnEffectsInputFieldValueChanged);
        
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        musicInputField.onValueChanged.AddListener(OnMusicInputFieldValueChanged);
        
        LoadSettings();
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

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("GlobalVolume"))
            globalVolume = PlayerPrefs.GetInt("GlobalVolume");
        else globalVolume = defaultVolume;
        if (PlayerPrefs.HasKey("EffectsVolume"))
            effectsVolume = PlayerPrefs.GetInt("EffectsVolume");
        else effectsVolume = defaultEffectsVolume;
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolume = PlayerPrefs.GetInt("MusicVolume");
        else musicVolume = defaultMusicVolume;
        
        UpdateGlobalVolumeUI();
        UpdateEffectsVolumeUI();
        UpdateMusicVolumeUI();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("GlobalVolume", globalVolume);
        PlayerPrefs.SetInt("EffectsVolume", effectsVolume);
        PlayerPrefs.SetInt("MusicVolume", musicVolume);
    }

    private void OnGlobalVolumeSliderValueChanged(float value)
    {
        globalVolume = (int)value;
        UpdateGlobalVolumeUI();
    }
    private void OnGlobalVolumeInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        globalVolume = currentVolume;
        UpdateGlobalVolumeUI();
    }
    private void UpdateGlobalVolumeUI()
    {
        globalVolumeSlider.value = globalVolume;
        globalVolumeInputField.text = globalVolume.ToString();
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        musicVolume = (int)value;
        UpdateMusicVolumeUI();
    }
    private void OnMusicInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        musicVolume = currentVolume;
        UpdateMusicVolumeUI();
    }
    private void UpdateMusicVolumeUI()
    {
        musicVolumeSlider.value = musicVolume;
        musicInputField.text = musicVolume.ToString();
    }
    
    private void OnEffectsVolumeSliderValueChanged(float value)
    {
        effectsVolume = (int)value;
        UpdateEffectsVolumeUI();
    }
    private void OnEffectsInputFieldValueChanged(string value)
    {
        int.TryParse(value, out int currentVolume);
        effectsVolume = currentVolume;
        UpdateEffectsVolumeUI();
    }
    private void UpdateEffectsVolumeUI()
    {
        effectsVolumeSlider.value = effectsVolume;
        effectsInputField.text = effectsVolume.ToString();
    }
}
