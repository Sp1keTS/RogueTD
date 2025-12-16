using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private  Button goBackButton;
    [SerializeField] private  Button soundSettingsButton;
    [SerializeField] private  Button saveSettingsButton;
    
    [SerializeField] private  GameObject mainMenuCanvas;
    [SerializeField] private  GameObject settingsCanvas;
    [SerializeField] private  Slider globalVolumeSlider;
    [SerializeField] private  Slider effectsVolumeSlider;
    [SerializeField] private  Slider musicVolumeSlider;
    [SerializeField] private  TMP_InputField globalVolumeInputField;
    [SerializeField] private  TMP_InputField effectsInputField;
    [SerializeField] private  TMP_InputField musicInputField;
    
    [SerializeField] private int defaultVolume = 100;
    [SerializeField] private int defaultEffectsVolume = 100;
    [SerializeField] private int defaultMusicVolume = 100;
    
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
        if (PlayerPrefs.HasKey("MasterVolume"))
            globalVolume = (int)(PlayerPrefs.GetFloat("MasterVolume") * 100f);
        else globalVolume = defaultVolume;
        if (PlayerPrefs.HasKey("SoundFXVolume"))
            effectsVolume = (int)(PlayerPrefs.GetFloat("SoundFXVolume") * 100f);
        else effectsVolume = defaultEffectsVolume;
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolume = (int)(PlayerPrefs.GetFloat("MusicVolume") * 100f);
        else musicVolume = defaultMusicVolume;
        
        Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
        Debug.Log(PlayerPrefs.GetFloat("SoundFXVolume"));
        Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
        
        UpdateGlobalVolumeUI();
        UpdateEffectsVolumeUI();
        UpdateMusicVolumeUI();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", globalVolume / 100f);
        PlayerPrefs.SetFloat("SoundFXVolume", effectsVolume / 100f);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume / 100f);
        AudioMixerManager.instance.UpdateVolume();
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
